using ParkingServiceServer.RepositoryServices;
using ServiceContracts;
using ServiceContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using AuditingManager;
using SecurityManager;
using System.Threading;
using System.ServiceModel.Security;
using System.ServiceModel;
using ServiceContracts.Enums;
using ParkingServiceServer.Monitoring;

namespace ParkingServiceServer
{
	public class ParkingServiceServer : IParkingService
	{
		CarRepositoryService carRepoService = new CarRepositoryService();
		PaymentRepositoryService paymentRepoService = new PaymentRepositoryService();
		ZoneRepositoryService zoneRepoService = new ZoneRepositoryService();

		//[PrincipalPermission(SecurityAction.Demand, Role = "ManageZone")]
		public bool AddParkingZone(ParkingZone parkingZone)
		{

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            bool condition = SecurityHelper.CheckAuthorization("ManageZone", userName, "AddParkingZone");

            if (!condition)
                throw new FaultException<SecurityFault>(new SecurityFault("Access denied!"));

            if (condition)
            {
                ParkingZone zone = zoneRepoService.Find(parkingZone.Id);
                if (zone == null)
                {

                    if (zoneRepoService.InsertIntoFile(parkingZone))
                        SecurityHelper.AuditZoneOperationSucces(parkingZone.ZoneType, "added");

                    return true;
                   
                }
                else
                    SecurityHelper.AuditZoneOperationFailure("AddParkingZone", $"Zone with Id = {parkingZone.Id}, already exists!");
            }
           
			return false;
		}


		//[PrincipalPermission(SecurityAction.Demand, Role = "ManageZone")]
		public bool ModifyParkingZone(ParkingZone parkingZone)
		{

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            bool condition = SecurityHelper.CheckAuthorization("ManageZone", userName, "ModifyParkingZone");

            if (!condition)
                throw new FaultException<SecurityFault>(new SecurityFault("Access denied!"));

            if (condition)
            {
                ParkingZone zone = zoneRepoService.Find(parkingZone.Id);
                if (zone != null)
                {
                    if(zoneRepoService.Modify(parkingZone))
                        SecurityHelper.AuditZoneOperationSucces(parkingZone.ZoneType, "modified");

                    return true;

                }
                else
                    SecurityHelper.AuditZoneOperationFailure("ModifyParkingZone", $"Zone with Id = {parkingZone.Id}, does not exists!");
            }

			return false;
		}


		//[PrincipalPermission(SecurityAction.Demand, Role = "ParkingWorker")]
		public bool AddParkingPenaltyTicket(string registration, string zone)
		{

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            bool condition = SecurityHelper.CheckAuthorization("ParkingWorker", userName, "AddParkingPenaltyTicket");

            if (!condition)
                throw new FaultException<SecurityFault>(new SecurityFault("Access denied!"));

            if (condition)
            {

                if (!CheckPayment(registration, zone))
                {
                    Car car = carRepoService.Find(registration);
                    car.PenaltyTicket = true;
                    SecurityHelper.AuditAddParkingPenaltyTicketSuccess();

                    return carRepoService.Modify(car);
                }
                else
                    SecurityHelper.AuditAddParkingPenaltyTicketFailure($"Car with registration {registration} payed parking!");
            }

			return false;
		}


		//[PrincipalPermission(SecurityAction.Demand, Role = "ParkingWorker")]
		public bool CheckPayment(string registration, string zone)
		{
            
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            bool condition = SecurityHelper.CheckAuthorization("ParkingWorker", userName, "CheckPayment");

            if (!condition)
                throw new FaultException<SecurityFault>(new SecurityFault("Access denied!"));


            if (condition)
            {

                if (paymentRepoService.FindPayment(registration, zone))
                {
                    SecurityHelper.AuditCheckPaymentSuccess();
                    return true;
                }
                else
                {
                    SecurityHelper.AuditCheckPaymentSuccess();
                    return false;
                }
            }

            return false;
		}


		//[PrincipalPermission(SecurityAction.Demand, Role = "Pay")]
		public bool PayParking(Car car, ParkingZone zone)
		{
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            bool condition = SecurityHelper.CheckAuthorization("Pay", userName, "PayParking");

            if (!condition)
                throw new FaultException<SecurityFault>(new SecurityFault("Access denied!"));


            if (condition)
            {

                Payment payment = new Payment() { Id = Guid.NewGuid().ToString(), CarId = car.Registration, ParkingZoneId = zone.Id };
                if (paymentRepoService.InsertIntoFile(payment))
                {
                    SecurityHelper.AuditPayParkingSuccess();
                    return true;
                }
                else
                {
                    SecurityHelper.AuditPayParkingSuccess(); 
                    return false;
                }  
            }

            return false;
		}


		//[PrincipalPermission(SecurityAction.Demand, Role = "ParkingWorker")]
		public bool RemoveParkingPenaltyTicket(string registration)
		{

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            bool condition = SecurityHelper.CheckAuthorization("ParkingWorker", userName, "RemoveParkingPenaltyTicket");

            if (!condition)
                throw new FaultException<SecurityFault>(new SecurityFault("Access denied!"));


            if (condition)
            {

                Car car = carRepoService.Find(registration);
                if (car != null)
                {
                    car.PenaltyTicket = false;
                    SecurityHelper.AuditRemoveParkingPenaltyTicketSuccess();
                    return carRepoService.Modify(car);
                }
                else
                    SecurityHelper.AuditRemoveParkingPenaltyTicketFailure($"Car with registration = {registration} does not exists!");
            }

			return false;
		}
		
		public List<ParkingZone> GetAllParkingZones()
		{
			return zoneRepoService.GetAll();
		}

		public List<Car> GetAllCars()
		{
			return carRepoService.GetAll();
		}

		public List<Payment> GetAllPayments()
		{
			return paymentRepoService.GetAll();
		}

		public List<Car> GetAllCarsWithoutPayment()
		{
			List<Car> allCars = GetAllCars();
			List<Payment> payments = GetAllPayments();
			List<Car> carsWithoutPayment = new List<Car>();

			if (allCars == null || allCars.Count == 0)
			{
				return null;
			}

			if (payments == null || payments.Count == 0)
			{
				return allCars;
			}


			foreach (Car car in allCars)
			{
				foreach (Payment payment in payments)
				{
					if (car.Registration.Equals(payment.CarId))
					{
						continue;
					}

					carsWithoutPayment.Add(car);
				}
			}

			return carsWithoutPayment;
		}

        public EServiceState GetServerState()
        {
            return ServiceState.ServiceConfiguration.ServerState;
        }
    }
}
