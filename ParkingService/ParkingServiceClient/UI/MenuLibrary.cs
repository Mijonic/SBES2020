using ServiceContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServiceClient.UI
{
	public class MenuLibrary
	{
		#region publicMethods

		public static void PrintMenu()
		{
			Console.WriteLine("\t-------------------Menu-------------------");
			Console.WriteLine("\t1. Add new parking zone");
			Console.WriteLine("\t2. Modify parking zone");
			Console.WriteLine("\t3. Check users payment");
			Console.WriteLine("\t4. Add parking penalty ticket");
			Console.WriteLine("\t5. Remove parking penalty ticket");
			Console.WriteLine("\t6. Pay parking");
			Console.WriteLine("\t7. Exit\n");
			Console.Write("Choice: ");
		}

		public static void AddNewParkingZone(ClientProxy proxy)
		{
			ParkingZone zone = new ParkingZone();

			zone.Id = Guid.NewGuid().ToString();

			zone = GetZoneFromUsersInput(zone);

            proxy.AddParkingZone(zone);
          

		}

		public static void ModifyParkingZone(ClientProxy proxy)
		{
			List<ParkingZone> zones = GetAllZones(proxy);

			if (zones==null)
			{
				Console.WriteLine("\n\tThere are no parking zones\n");
				return;
			}

			PrintAllZones(zones);
			Console.WriteLine("\n\tChoose which zone you want to modify:");

			int ch = ChoooseNumber(zones.Count);
			ParkingZone zone = zones[ch - 1];
			zone = GetZoneFromUsersInput(zone);

            proxy.ModifyParkingZone(zone);

           
        }

		public static void CheckUsersPayment(ClientProxy proxy)
		{

			

			List<ParkingZone> zones = GetAllZones(proxy);
			
			if (zones==null)
			{
				Console.WriteLine("\n\tThere are no parking zones\n");
				return;
			}

			List<Car> cars = GetAllCars(proxy);

			if (cars == null)
			{
				Console.WriteLine("\n\tThere are no cars\n");
				return;
			}

			PrintAllZones(zones);
			Console.WriteLine("\n\tChoose which zone you want to check: ");

			int chZ = ChoooseNumber(zones.Count);	
			ParkingZone choosenZone = new ParkingZone();
			choosenZone = zones[chZ - 1];

			PrintAllCars(cars);
			Console.WriteLine("Chose car you want to check: ");

			int chC = ChoooseNumber(cars.Count);
			Car choosenCar = cars[chC - 1];

            proxy.CheckPayment(choosenCar.Registration, choosenZone.Id);
		
				
			
		}

		public static void AddParkingPenaltyTicket(ClientProxy proxy)
		{

			List<ParkingZone> zones = GetAllZones(proxy);

			if (zones == null)
			{
				Console.WriteLine("\n\tThere are no parking zones\n");
				return;
			}

			List<Car> cars = GetAllCarsWithoutPayment(proxy);

			if (cars==null)
			{
				Console.WriteLine("\n\tThere are no cars without payment\n");
				return;
			}

            cars = cars.FindAll(x => !x.PenaltyTicket.Equals(true));

            if (cars == null)
            {
                Console.WriteLine("\nThere are no cars without payment\n");
            }

            PrintAllZones(zones);
			Console.WriteLine("Chose zone you want to check:");

			int chZ = ChoooseNumber(zones.Count());
			ParkingZone choosenZone = new ParkingZone();
			choosenZone = zones[chZ - 1];

			PrintAllCars(cars);
			Console.WriteLine("Choose car you want to add penalty ticket:");

			int chC = ChoooseNumber(cars.Count());
			Car choosenCar = new Car();
			choosenCar = cars[chC - 1];

            proxy.AddParkingPenaltyTicket(choosenCar.Registration, choosenZone.Id);
           
		
		}

		public static void ReomveParkingPenaltyTicket(ClientProxy proxy)
		{
			List<Car> cars = GetAllCars(proxy);

			if (cars==null)
			{
				Console.WriteLine("\n\tThere are no cars without payment\n");
				return;
			}

			cars = cars.FindAll(x => x.PenaltyTicket.Equals(true));

			if (cars == null || cars.Count == 0)
			{
				Console.WriteLine("\n\tThere are no cars with penalty ticket!\n");
				return;
			}

			Console.WriteLine("\n\tList of all cars with penalty tickets:");
			
			PrintAllCars(cars);

			int select = ChoooseNumber(cars.Count());
			Car choosenCar = cars[select - 1];

            proxy.RemoveParkingPenaltyTicket(choosenCar.Registration);
           
		}

		public static void PayParking(ClientProxy proxy)
		{
			List<Car> cars = GetAllCars(proxy);

			if (cars == null)
			{
				Console.WriteLine("\n\tThere are no cars without payment\n");
				return;
			}

            PrintAllCars(cars);

			int select=ChoooseNumber(cars.Count());
			Car choosenCar = cars[select - 1];

			List<ParkingZone> zones = GetAllZones(proxy);

			if (zones == null)
			{
				Console.WriteLine("\n\tThere are no parking zones\n");
				return;
			}

            PrintAllZones(zones);

			select = ChoooseNumber(zones.Count());
			ParkingZone choosenParking = zones[select - 1];

            proxy.PayParking(choosenCar, choosenParking);
          
		}

		public static void Exit()
		{
			Console.WriteLine("\nSee you later :)");
		}

		#endregion

		#region inputMethods

		private static ParkingZone GetZoneFromUsersInput(ParkingZone zone)
		{
			Console.Write("\n\tEnter  parking zone name: ");
			zone.ZoneType = Console.ReadLine();

			Console.Write("\n\tEnter parking zone price: ");
			zone.Price = InputChoiceParam();

			Console.Write("\n\tEnter parking zone duration: ");
			zone.Duration = InputChoiceParam();

			return zone;
		}

		private static int InputChoice()
		{
			int choice = -1;
			bool condition;

			do
			{
				Console.Write("\n\tEnter positive integer number: ");
				condition = Int32.TryParse(Console.ReadLine(), out choice);

			} while (!condition || choice < 0);

			return choice;
		}

		private static double InputChoiceParam()
		{
			double choice = -1;
			bool condition;

			do
			{
				Console.Write("\n\tEnter positive number: ");
				condition = double.TryParse(Console.ReadLine(), out choice);

			} while (!condition || choice < 0);

			return choice;
		}

		private static int ChoooseNumber(int objCount)
		{
			int ch = -1;
			do
			{
                if(objCount == 1)
                    Console.Write("\tEnter number: ");
                else
				    Console.Write("\tEnter number between 1 and " + objCount + ": ");

				ch = InputChoice();

			} while (ch < 1 || ch > objCount);

			return ch;
		}


		#endregion

		#region printMethods

		private static void PrintAllZones(List<ParkingZone> zones)
		{
			
			Console.WriteLine("\n----------------------All zones----------------------");

			int i = 0;
            
			foreach (ParkingZone z in zones)
			{
				i++;
				Console.WriteLine(i + ". " + "Zone name:" + z.ZoneType + " \tPrice:" + z.Price + " \tDuration " + z.Duration);
			}
            Console.WriteLine("-----------------------------------------------------");
		}

		private static void PrintAllCars(List<Car> cars)
		{

			Console.WriteLine("\n-----------------------------------All cars-----------------------------------");

			int i = 0;

			foreach (Car c in cars)
			{
				i++;
                Console.WriteLine(i + "." + "Car registration:  " + c.Registration + " \tBrand: " + c.Brand + " \tColor: " + c.Color);
			}
            Console.WriteLine("------------------------------------------------------------------------------");
		}

		#endregion

		#region proxyMethods

		private static List<ParkingZone> GetAllZones(ClientProxy proxy)
		{
			List<ParkingZone> zones= proxy.GetAllParkingZones();

			if (zones == null || zones.Count == 0)
			{
				return null;
			}

			return zones;
		}

		private static List<Car> GetAllCars(ClientProxy proxy)
		{
			List<Car> cars = proxy.GetAllCars();

			if (cars == null || cars.Count == 0)
			{
				return null;
			}

			return cars;
		}

		private static List<Car> GetAllCarsWithoutPayment(ClientProxy proxy)
		{
			List<Car> cars = proxy.GetAllCarsWithoutPayment();

			if (cars == null || cars.Count == 0)
			{
				return null;
			}

			return cars;
		}

		private static List<Payment> GetAllPayments(ClientProxy proxy)
		{
			List<Payment> payments = proxy.GetAllPayments();

			if(payments == null || payments.Count == 0)
			{
				return null;
			}

			return payments;
		}

		#endregion

	}
}
