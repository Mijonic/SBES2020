using DataRepository.Contracts;
using ServiceContracts.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Repositories
{
	public class PaymentRepository: IPaymentRepository
	{
        public bool DeleteFromFile(string id)
        {
            List<Payment> payments = GetAll()?.Where(x => id != x.Id)?.ToList();

            if (payments == null)
                return false;

            using (StreamWriter sw = new StreamWriter("payments.txt"))
            {
                foreach (var payment in payments)
                {
                    sw.WriteLine(payment.ToString());
                }
            }
            return true;
        }

        public Payment Find(string id)
        {
            return GetAll()?.Find(x => x.Id.Equals(id));
        }

        public bool FindPayment(string registration, string zoneId)
        {
            List<Payment> payments = GetAll()?.Where(x => (registration == x.CarId && zoneId == x.ParkingZoneId))?.ToList();   //ne vrati null nego praznu listu    - count = 0

            if (payments == null || payments.Count == 0)  
                return false;

            return true;
        }

        public List<Payment> GetAll()
        {
            List<Payment> payments = new List<Payment>();

            if (File.Exists("payments.txt"))
            {
                string line;
                using (StreamReader sr = new StreamReader("payments.txt"))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        Payment payment = Payment.ConvertToObject(line);
                        if (payment != null)
                            payments.Add(payment);
                    }
                }
            }
            return payments;
        }

		public bool InsertAll(List<Payment> all)
		{
			if(all == null)
				return false;

			using (StreamWriter sw = new StreamWriter("payments.txt"))
			{
				foreach (var pay in all)
				{
					sw.WriteLine(pay.ToString());
				}
			}
			return true;
		}

		public bool InsertIntoFile(Payment obj)
        {
            if (obj == null)
                return false;

            using (StreamWriter sw = File.AppendText("payments.txt"))
            {
                sw.WriteLine(obj.ToString());
            }

            return true;
        }

        public bool Modify(Payment obj)
        {
            if (obj == null)
                return false;

            List<Payment> payments = GetAll();

            Payment foundPayment = payments.Find(x => x.Id.Equals(obj.Id));

            if (foundPayment == null)
                return false;
            foundPayment.Id = obj.Id;
            foundPayment.CarId = obj.CarId;
            foundPayment.ParkingZoneId = obj.ParkingZoneId;

            using (StreamWriter sw = new StreamWriter("payments.txt"))
            {
                foreach (var pay in payments)
                {
                    sw.WriteLine(pay.ToString());
                }
            }
            return true;
        }
    }
}
