using CryptographyManager;
using ParkingServiceServer.Menager;
using ParkingServiceServer.Monitoring;
using ParkingServiceServer.RepositoryServices;
using SecurityManager;
using ServiceContracts;
using ServiceContracts.DataModels;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading;
using Formatter = ParkingServiceServer.Menager.Formatter;

namespace ParkingServiceServer.ServiceReplicator
{
	public class ReplicatorHelper
	{
		public static void Replicate()
		{
			bool opened = false;
			bool connected = false;
			ServiceHost replicatorHost = null;
			WCFReplicator replicatorProxy=null;
			EServiceState currentState;

			bool isInvalidIssuer = false;

            int replicationTimer;
            bool isTimerValid = Int32.TryParse(ConfigurationManager.AppSettings["replicatorTimer"], out replicationTimer);

            if (!isTimerValid)
                replicationTimer = 5;

            Console.WriteLine($"\n\nREPLICATOR TIMER: {replicationTimer} \n\n");

            while (true)
			{

				currentState = ServiceState.ServiceConfiguration.ServerState;

				try
				{
					if (currentState.Equals(EServiceState.PRIMARY))
					{
						WorkAsPrimary(ref opened, replicatorHost);
					}
					else if (currentState.Equals(EServiceState.SECONDARY))
					{
						WorkAsSecondary(ref connected, ref replicatorProxy, ref isInvalidIssuer);
					}

				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
				Thread.Sleep(replicationTimer);
			}

		}

		private static void WorkAsPrimary(ref bool opened, ServiceHost replicatorHost)
		{
			if (!opened)
			{
				string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

				NetTcpBinding replicatorBinding = new NetTcpBinding();
				replicatorBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

				string replicatorAddress = ConfigurationManager.AppSettings["replicatorAddress"];
				replicatorHost = new ServiceHost(typeof(Replicator));
				replicatorHost.AddServiceEndpoint(typeof(IReplicator), replicatorBinding, replicatorAddress);

				replicatorHost.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
				replicatorHost.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();

				///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
				replicatorHost.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

				///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
				X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
				
				replicatorHost.Credentials.ServiceCertificate.Certificate = cert;

				try
				{
					replicatorHost.Open();
					opened = true;
				}
				catch (Exception e)
				{
					opened = false;
					Console.WriteLine(e.Message);
					replicatorHost.Close();
				}
			}
		}

		private static void WorkAsSecondary(ref bool connected, ref WCFReplicator replicatorProxy, ref bool isInvalidIssuer)
		{
			try
			{
				if (isInvalidIssuer)
				{
					return;
				}     

				if (!connected)
				{

					string currentIdentity = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
					string srvCertCN = "";

					if (currentIdentity.Equals("primary"))
					{
						srvCertCN = "secondary";
					}
					else
					{
						srvCertCN = "primary";
					}

					NetTcpBinding bindingReplicator = new NetTcpBinding();
					bindingReplicator.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

					/// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
					string replicatorAddress = ConfigurationManager.AppSettings["replicatorAddress"];
					X509Certificate2 cert2 = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
					
					X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
					EndpointAddress address = new EndpointAddress(new Uri(replicatorAddress),
											  new X509CertificateEndpointIdentity(srvCert));

					replicatorProxy = new WCFReplicator(bindingReplicator, address);
					connected = true;
				}

				GetDataFromPrimary(replicatorProxy, ref isInvalidIssuer);	

			}
			catch (CommunicationException ex)
			{
				Console.WriteLine($"Error - communication did not succeed : {ex.Message}");
				connected = false;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				connected = false;
			}
		}

		private static void GetDataFromPrimary(WCFReplicator replicatorProxy, ref bool isInvalidIssuer)
		{

			GetCarsFromPrimary(replicatorProxy, ref isInvalidIssuer);
			GetPaymentsFromPrimary(replicatorProxy, ref isInvalidIssuer);
			GetZonesFromPrimary(replicatorProxy, ref isInvalidIssuer);

            Console.WriteLine("Data successfully transfered to backup server.");

            SecurityHelper.AuditReplicatorSuccess("Cars, zones and payments");			
		}

		private static void GetCarsFromPrimary(WCFReplicator replicatorProxy, ref bool isInvalidIssuer)
		{
			CarRepositoryService carRepoService = new CarRepositoryService();

            //encrypt
            KeyValuePair<byte[], byte[]> carsPairs = replicatorProxy.TransferCars();

			if (carsPairs.Equals(default(KeyValuePair<byte[], byte[]>)))
			{
				isInvalidIssuer = true;
				return;
			}

			//decrypt	
            List<Car> cars = CryptographyService<Car>.Decrypt(carsPairs);

            if(cars == null)
            {
                Console.WriteLine("Data decryption went wrong");
                SecurityHelper.AuditReplicatorFailure("Data decryption went wrong");
                return;
            }

            carRepoService.InsertAll(cars);
		}

		private static void GetPaymentsFromPrimary(WCFReplicator replicatorProxy, ref bool isInvalidIssuer)
		{
			PaymentRepositoryService paymentRepoService = new PaymentRepositoryService();

            KeyValuePair<byte[], byte[]> paymentsPairs = replicatorProxy.TransferPayments();


            if (paymentsPairs.Equals(default(KeyValuePair<byte[], byte[]>)))
            {
                isInvalidIssuer = true;
                return;
            }

			List<Payment> payments = CryptographyService<Payment>.Decrypt(paymentsPairs);

            if (payments == null)
            {
                Console.WriteLine("Data decryption went wrong");
                SecurityHelper.AuditReplicatorFailure("Data decryption went wrong");
                return;
            }

            paymentRepoService.InsertAll(payments);
		}

		private static void GetZonesFromPrimary(WCFReplicator replicatorProxy, ref bool isInvalidIssuer)
		{
			ZoneRepositoryService zoneRepoService = new ZoneRepositoryService();

            KeyValuePair<byte[], byte[]> zonesPairs = replicatorProxy.TransferZones();

			if (zonesPairs.Equals(default(KeyValuePair<byte[], byte[]>)))
			{
				isInvalidIssuer = true;
				return;
			}

			List<ParkingZone> zones = CryptographyService<ParkingZone>.Decrypt(zonesPairs);

            if(zones == null)
            {
                Console.WriteLine("Data decryption went wrong");
                SecurityHelper.AuditReplicatorFailure("Data decryption went wrong");
                return;
            }

            zoneRepoService.InsertAll(zones);
		}
	}
}
