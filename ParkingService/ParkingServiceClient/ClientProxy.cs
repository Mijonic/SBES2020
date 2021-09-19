using SecurityManager;
using ServiceContracts;
using ServiceContracts.DataModels;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Authentication;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServiceClient
{
    public class ClientProxy : ChannelFactory<IParkingService>, IParkingService, IDisposable
    {
        IParkingService factory;

        public ClientProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }


        public bool AddParkingPenaltyTicket(string registration, string zone)
        {
            try
            {

                bool condition = factory.AddParkingPenaltyTicket(registration, zone);

                if (condition)
                {
                    Console.WriteLine("\n************************************");
                    Console.WriteLine("Penalty ticket successfully added");
                    Console.WriteLine("************************************\n");
                }

                return condition;
            }
            catch (FaultException<SecurityFault> fault)
            {
                //Console.WriteLine(fault.Detail.Message);
                Console.WriteLine("\nYou don't have access to method: 'AddParkingPenaltyTicket' -> " + fault.Detail.Message.ToString() + "\n");
                return false;
            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'AddParkingPenaltyTicket' -> " + securityException.Message.ToString() + "\n");
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");

                return false;
            }
        }


        public bool AddParkingZone(ParkingZone parkingZone)
        {
            try
            {
                bool condition = factory.AddParkingZone(parkingZone);

                if (condition)
                {
                    Console.WriteLine("\n***********************************");
                    Console.WriteLine("Parking zone successfully added");
                    Console.WriteLine("***********************************\n");
                }

                return condition;
            }
            catch (FaultException<SecurityFault> fault)
            {
                //Console.WriteLine(fault.Detail.Message);
                Console.WriteLine("\nYou don't have access to method: 'AddParkingZone' -> " + fault.Detail.Message.ToString() + "\n");
                return false;
            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'AddParkingZone' -> " + securityException.Message.ToString() + "\n");
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine("\nOVDE SAM UGINUO!!!" + ex.Message.ToString() + "\n");
                return false;
            }
        }


        public bool CheckPayment(string registration, string zone)
        {
            try
            {
                bool condition = factory.CheckPayment(registration, zone);

                if (condition)
                {
                    Console.WriteLine("\n*******************************************************************************");
                    Console.WriteLine("Car with registration " + registration + " in the selected zone has payed for the parking");
                    Console.WriteLine("*******************************************************************************\n");

                }else
                {
                    Console.WriteLine("\n*******************************************************************************");
                    Console.WriteLine("Car with registration " + registration + " in the selected zone has not payed for the parking");
                    Console.WriteLine("*******************************************************************************\n");
                }
               

                return condition;

            }
            catch (FaultException<SecurityFault> fault)
            {
                //Console.WriteLine(fault.Detail.Message);
                Console.WriteLine("\nYou don't have access to method: 'CheckPayment' -> " + fault.Detail.Message.ToString() + "\n");
                return false;
            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'CheckPayment' -> " + securityException.Message.ToString() + "\n");
                return false;                 
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");
                return false;
            }
        }


        public bool ModifyParkingZone(ParkingZone parkingZone)
        {
            try
            {
                bool condition = factory.ModifyParkingZone(parkingZone);

                 if (condition)
                {
                    Console.WriteLine("\n*************************************");
                    Console.WriteLine("Parking zone successfully modified");
                    Console.WriteLine("*************************************\n");
                }

                return condition;
            }
            catch (FaultException<SecurityFault> fault)
            {
                //Console.WriteLine(fault.Detail.Message);
                Console.WriteLine("\nYou don't have access to method: 'ModifyParkingZone' -> " + fault.Detail.Message.ToString() + "\n");
                
                return false;
            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'ModifyParkingZone' -> " + securityException.Message.ToString() + "\n");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");
                return false;
            }
        }


        public bool PayParking(Car car, ParkingZone zone)
        {
            try
            {
                bool condtion = factory.PayParking(car, zone);


                if (condtion)
                {
                    Console.WriteLine("\n****************************************************************");
                    Console.WriteLine($"Parking payed for car with this registration:  {car.Registration}");
                    Console.WriteLine("****************************************************************\n");
                }

                return condtion;
            }
            catch (FaultException<SecurityFault> fault)
            {
                //Console.WriteLine(fault.Detail.Message);
                Console.WriteLine("\nYou don't have access to method: 'PayParking' -> " + fault.Detail.Message.ToString() + "\n");

                return false;
            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'PayParking' -> " + securityException.Message.ToString() + "\n");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");
                return false;
            }         
        }


        public bool RemoveParkingPenaltyTicket(string registration)
        {
			try
			{
                bool condition = factory.RemoveParkingPenaltyTicket(registration);

                if (condition)
                {
                    Console.WriteLine("\n********************************************************************************");
                    Console.WriteLine($"Penalty ticket removed for car with this registration:  {registration}");
                    Console.WriteLine("********************************************************************************\n");
                }

                return condition;
			}
            catch (FaultException<SecurityFault> fault)
            {
                //Console.WriteLine(fault.Detail.Message);
                Console.WriteLine("\nYou don't have access to method: 'RemoveParkingPenaltyTicket' -> " + fault.Detail.Message.ToString() + "\n");

                return false;
            }
            catch (SecurityAccessDeniedException securityException)
			{
                Console.WriteLine("\nYou don't have access to method: 'RemoveParkingPenaltyTicket' -> " + securityException.Message.ToString() + "\n");
                return false;
			}
			catch (Exception ex)
			{
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");

                return false;
			}
		}
        

        public List<ParkingZone> GetAllParkingZones()
        {
            try
            {
               return factory.GetAllParkingZones();

            }
            catch (SecurityAccessDeniedException securityException)
            {
                Console.WriteLine("\nYou don't have access to method: 'GetAllParkingZones' -> " + securityException.Message.ToString() + "\n");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");
                return null;
            }

        }

		public List<Car> GetAllCars()
		{
			try
			{
				return factory.GetAllCars();
			}
			catch (SecurityAccessDeniedException securityException)
			{
                Console.WriteLine("\nYou don't have access to method: 'GetAllCars' -> " + securityException.Message.ToString() + "\n");
                return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine("\n" + ex.Message.ToString() + "\n");

				return null;
			}
		}

		public List<Payment> GetAllPayments()
		{
			try
			{
				return factory.GetAllPayments();
			}
			catch (SecurityAccessDeniedException securityException)
			{
                Console.WriteLine("\nYou don't have access to method: 'GetAllPayments' -> " + securityException.Message.ToString() + "\n");
                return null;
			}
			catch (Exception ex)
			{
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");

                return null;
			}
		}

		public List<Car> GetAllCarsWithoutPayment()
		{
			try
			{
				return factory.GetAllCarsWithoutPayment();
			}
			catch (SecurityAccessDeniedException securityException)
			{
                Console.WriteLine("\nYou don't have access to method: 'GetAllCarsWithoutPayment' -> " + securityException.Message.ToString() + "\n");
                return null;
			}
			catch (Exception ex)
			{
                Console.WriteLine("\n" + ex.Message.ToString() + "\n");

                return null;
			}
		}

        public EServiceState GetServerState()
        {
            return factory.GetServerState();
        }
    }
}
