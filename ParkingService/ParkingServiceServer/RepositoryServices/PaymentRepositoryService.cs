using DataRepository.Contracts;
using DataRepository.Repositories;
using ServiceContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingServiceServer.RepositoryServices
{
    public class PaymentRepositoryService
    {
        private readonly IPaymentRepository _paymentRepository = new PaymentRepository();

        public List<Payment> GetAll()
        {
            return _paymentRepository.GetAll();
        }

		public bool InsertAll(List<Payment> all)
		{
			return _paymentRepository.InsertAll(all);
		}

        public bool InsertIntoFile(Payment payment)
        {
            return _paymentRepository.InsertIntoFile(payment);
        }

        public bool DeleteFromFile(string id)
        {
            return _paymentRepository.DeleteFromFile(id);
        }

        public Payment Find(string id)
        {
            return _paymentRepository.Find(id);
        }

        public bool Modify(Payment obj)
        {
            return _paymentRepository.Modify(obj);
        }

        public bool FindPayment(string registration, string zoneId)
        {
            return _paymentRepository.FindPayment(registration, zoneId);
        }
    }
}
