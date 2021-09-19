using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServiceClient.UI
{
	public class Menu
	{
		public static void MainMenu(ClientProxy proxy, out EServiceState state, out bool exit)
		{
			int choice = -1;
            state = EServiceState.UNKNOWN;
            exit = false;
			while (choice != 7)
			{
                try
                {
                    state = proxy.GetServerState();
                }
                catch
                {
                    state = EServiceState.UNKNOWN;
                    break;
                }

                if (state != EServiceState.PRIMARY)
                    break;

				MenuLibrary.PrintMenu();

				choice = GetInput();

				switch (choice)
				{
					case 1:
						MenuLibrary.AddNewParkingZone(proxy);
						break;
					case 2:
						MenuLibrary.ModifyParkingZone(proxy);
						break;
					case 3:
						MenuLibrary.CheckUsersPayment(proxy);
						break;
					case 4:
						MenuLibrary.AddParkingPenaltyTicket(proxy);
						break;
					case 5:
						MenuLibrary.ReomveParkingPenaltyTicket(proxy);
						break;
					case 6:
						MenuLibrary.PayParking(proxy);
						break;
					case 7:
                        exit = true;
						MenuLibrary.Exit();
						break;

				}
			}

		}

		public static int GetInput()
		{
			int choice = -1;
			bool condition;

			do
			{
				Console.Write("\nEnter positive integer number: ");
				condition = Int32.TryParse(Console.ReadLine(), out choice);

			} while (!condition && choice < 0);

			return choice;
		}
	}
}
