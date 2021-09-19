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
    public class ZoneRepositoryService
    {
        private readonly IZoneRepository _zoneRepository = new ZoneRepository();

        public List<ParkingZone> GetAll()
        {
            return _zoneRepository.GetAll();
        }
		
		public bool InsertAll(List<ParkingZone> all)
		{
			return _zoneRepository.InsertAll(all);
		}
        public bool InsertIntoFile(ParkingZone zone)
        {
            return _zoneRepository.InsertIntoFile(zone);
        }

        public bool DeleteFromFile(string id)
        {
            return _zoneRepository.DeleteFromFile(id);
        }

        public ParkingZone Find(string id)
        {
            return _zoneRepository.Find(id);
        }

        public bool Modify(ParkingZone obj)
        {
            return _zoneRepository.Modify(obj);
        }
    }
}
