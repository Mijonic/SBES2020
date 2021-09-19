using ParkingServiceServer.Monitoring;
using ParkingServiceServer.ServiceReplicator;
using SecurityManager;
using ServiceContracts;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Policy;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;

namespace ParkingServiceServer
{
	public class Program
	{
		static void Main(string[] args)
		{

			#region ParkingService

			NetTcpBinding binding = new NetTcpBinding();
			string address = ConfigurationManager.AppSettings["parkingServiceAddress"];
			Console.WriteLine($"************************** { address} *************************");

			binding.Security.Mode = SecurityMode.Transport;
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;


			ServiceHost host = new ServiceHost(typeof(ParkingServiceServer));
			host.AddServiceEndpoint(typeof(IParkingService), binding, address);


			host.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();

			// podesavamo custom polisu, odnosno nas objekat principala
			host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
			List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
			policies.Add(new CustomAuthorizationPolicy());
			host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            host.Description.Behaviors.Add(newAudit);

            #endregion


            #region monitor

            NetTcpBinding monitorBinding = new NetTcpBinding();
			string monitoringAddress = ConfigurationManager.AppSettings["monitoringAddress"];

			Console.WriteLine($"************************** { monitoringAddress} *************************");
		
			ServiceHost monitoringHost = new ServiceHost(typeof(ServiceState));
			monitoringHost.AddServiceEndpoint(typeof(IServiceState), monitorBinding, monitoringAddress);

			#endregion

			host.Open();
			monitoringHost.Open();

			Console.WriteLine("Server started");

		
			//replicator thread
			Thread replicatorThread = new Thread(ReplicatorHelper.Replicate);
			replicatorThread.IsBackground = true;
			replicatorThread.Start();

			Console.ReadLine();

			host.Close();
			monitoringHost.Close();
		}
	
	}
}
