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
    public class ZoneRepository : IZoneRepository
    {
        public bool DeleteFromFile(string id)
        {
            List<ParkingZone> zones = GetAll()?.Where(x => id != x.Id)?.ToList();

            if (zones == null)
                return false;

            using (StreamWriter sw = new StreamWriter("zones.txt"))
            {
                foreach (var zone in zones)
                {
                    sw.WriteLine(zone.ToString());
                }
            }
            return true;
        }

        public ParkingZone Find(string id)
        {
            return GetAll()?.Find(x => x.Id.Equals(id));
        }

        public List<ParkingZone> GetAll()
        {
            List<ParkingZone> zones = new List<ParkingZone>();

            if (File.Exists("zones.txt"))
            {
                string line;
                using (StreamReader sr = new StreamReader("zones.txt"))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        ParkingZone zone = ParkingZone.ConvertToObject(line);
                        if (zone != null)
                            zones.Add(zone);
                    }
                }
            }
            return zones;
        }

		public bool InsertAll(List<ParkingZone> all)
		{
			if (all == null)
				return false;

			using (StreamWriter sw = new StreamWriter("zones.txt"))
			{
				foreach (var zone in all)
				{
					sw.WriteLine(zone.ToString());
				}
			}
			return true;

		}

		public bool InsertIntoFile(ParkingZone obj)
        {
            if (obj == null)
                return false;

            using (StreamWriter sw = File.AppendText("zones.txt"))
            {
                sw.WriteLine(obj.ToString());
            }

            return true;
        }

        public bool Modify(ParkingZone obj)
        {
            if (obj == null)
                return false;

            List<ParkingZone> zones = GetAll();

            ParkingZone foundZone = zones.Find(x => x.Id.Equals(obj.Id));

            if (foundZone == null)
                return false;

            foundZone.Price = obj.Price;
            foundZone.ZoneType = obj.ZoneType;
            foundZone.Duration = obj.Duration;

            using (StreamWriter sw = new StreamWriter("zones.txt"))
            {
                foreach (var zone in zones)
                {
                    sw.WriteLine(zone.ToString());
                }
            }
            return true;
        }
    }
}
