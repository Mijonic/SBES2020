using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Contracts
{
	public interface IRepository<T> where T: class
	{
		List<T> GetAll();
		bool InsertAll(List<T> all);
		bool InsertIntoFile(T obj);
		bool DeleteFromFile(string id);
		T Find(string id);
		bool Modify(T obj);
	}

}
