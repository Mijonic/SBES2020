using ServiceContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ParkingServiceClient.UI;
using ParkingServiceServer.Monitoring;
using ServiceContracts.Enums;

namespace ParkingServiceClient
{
	public class Program
	{
		static void Main(string[] args)
		{
			NetTcpBinding binding = new NetTcpBinding();
			string address = "net.tcp://localhost:9998/ParkingServiceServer";

			binding.Security.Mode = SecurityMode.Transport;
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
			binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;


			EndpointAddress endpointAddress = new EndpointAddress(new Uri(address), EndpointIdentity.CreateUpnIdentity("Mijonic"));

            EServiceState state = EServiceState.UNKNOWN;
            string currentServicePort = "9999";
            bool exit = false;
        
            ClientProxy proxy = new ClientProxy(binding, endpointAddress);
     
            Console.WriteLine("Client started");

            state = proxy.GetServerState();
            Console.WriteLine("Server state: " + state.ToString());

            while (!exit)
            {
                if (state != EServiceState.PRIMARY)
                {
                    address = $"net.tcp://localhost:{currentServicePort}/ParkingServiceServer";
                    endpointAddress = new EndpointAddress(new Uri(address), EndpointIdentity.CreateUpnIdentity("Mijonic"));
                    proxy = new ClientProxy(binding, endpointAddress);
                    //Menu.MainMenu(proxy, out state);

                    currentServicePort = currentServicePort.Equals("9999") ? "9998" : "9999";
                }

                Menu.MainMenu(proxy, out state, out exit);
            }

			Console.ReadLine();
            proxy.Close();
		}

	}
}
