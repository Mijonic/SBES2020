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
	public class CarRepositoryService
	{
		private readonly ICarRepository _carRepository = new CarRepository();

		public List<Car> GetAll()
		{
			return _carRepository.GetAll();
		}

		public bool InsertAll(List<Car> all)
		{
			return _carRepository.InsertAll(all);
		}

		public bool InsertIntoFile(Car car)
		{
			return _carRepository.InsertIntoFile(car);
		}

		public bool DeleteFromFile(string id)
		{
			return _carRepository.DeleteFromFile(id);
		}

		public Car Find(string id)
		{
			return _carRepository.Find(id);
		}

		public bool Modify(Car obj)
		{
			return _carRepository.Modify(obj);
		}
	}
}
