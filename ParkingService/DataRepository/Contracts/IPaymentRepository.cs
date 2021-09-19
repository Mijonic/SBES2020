using ServiceContracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Contracts
{
	public interface IPaymentRepository : IRepository<Payment>
	{
        bool FindPayment(string registration, string zoneId);
	}
}
