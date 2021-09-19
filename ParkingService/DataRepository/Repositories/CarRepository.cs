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
	public class CarRepository : ICarRepository
	{

		public List<Car> GetAll()
		{
			List<Car> cars = new List<Car>();

			if (File.Exists("cars.txt"))
			{
				string line;
				using (StreamReader sr = new StreamReader("cars.txt"))
				{
					while ((line = sr.ReadLine()) != null)
					{
						Car car = Car.ConvertToObject(line);
						if(car!=null)
							cars.Add(car);
					}
				}
			}
			return cars;
		}
		
		public bool DeleteFromFile(string id)
		{
			
			List<Car> cars = GetAll()?.Where(x => id!=x.Registration)?.ToList();

			if (cars==null)
				return false;
			
			using (StreamWriter sw = new StreamWriter("cars.txt"))
			{
				foreach (var car in cars)
				{
					sw.WriteLine(car.ToString());
				}
			}
			return true;
		}

		public Car Find(string id)
		{
			return GetAll()?.Find(x => x.Registration.Equals(id));
		}

		public bool InsertIntoFile(Car obj)
		{
			if (obj == null)
				return false;

			using (StreamWriter sw = File.AppendText("cars.txt"))
			{
				sw.WriteLine(obj.ToString());
			}

			return true;
		}

		public bool Modify(Car obj)
		{
			if (obj == null)
				return false;

			List<Car> cars = GetAll();

			Car foundCar = cars.Find(x => x.Registration.Equals(obj.Registration));

			if (foundCar == null)
				return false;
			
			foundCar.Registration = obj.Registration;
			foundCar.Brand = obj.Brand;
			foundCar.Color = obj.Color;
            foundCar.PenaltyTicket = obj.PenaltyTicket;
			
			using (StreamWriter sw = new StreamWriter("cars.txt"))
			{					
				foreach (var car in cars)
				{
					sw.WriteLine(car.ToString());
				}
			}			
			return true;
		}

		public bool InsertAll(List<Car> all)
		{
			if (all == null)
				return false;

			using (StreamWriter sw = new StreamWriter("cars.txt"))
			{
				foreach (var car in all)
				{
					sw.WriteLine(car.ToString());
				}
			}
			return true;

		}
	}
}
