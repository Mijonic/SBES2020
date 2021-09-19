using SecurityManager;
using ServiceContracts.DataModels;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    [ServiceContract]
    public interface IParkingService
    {
        //ManageZone
        [OperationContract]
        [FaultContract(typeof(SecurityFault))]
        bool ModifyParkingZone(ParkingZone parkingZone);

        //ManageZone
        [OperationContract]
        [FaultContract(typeof(SecurityFault))]
        bool AddParkingZone(ParkingZone parkingZone);

        //Pay
        [OperationContract]
        [FaultContract(typeof(SecurityFault))]
        bool PayParking(Car car, ParkingZone zone);

        //ParkingWorker
        [OperationContract]
        [FaultContract(typeof(SecurityFault))]
        bool CheckPayment(string registration, string zone);

        //ParkingWorker
        [OperationContract]
        [FaultContract(typeof(SecurityFault))]
        bool AddParkingPenaltyTicket(string registration, string zone);

        //ParkingWorker
        [OperationContract]
        [FaultContract(typeof(SecurityFault))]
        bool RemoveParkingPenaltyTicket(string registration);

        [OperationContract]
        List<ParkingZone> GetAllParkingZones();

		[OperationContract]
		List<Car> GetAllCars();

		[OperationContract]
		List<Payment> GetAllPayments();

		[OperationContract]
		List<Car> GetAllCarsWithoutPayment();

        [OperationContract]
        EServiceState GetServerState();     
        
    }
}
