
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DataModels
{
    
    [DataContract]
	[Serializable]
    public class ParkingZone
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public string ZoneType { get; set; }

        [DataMember]
        public double Duration { get; set; }
        public ParkingZone(string id, double price,string zoneType,double duration)
        {
            Id = id;
            Price = price;
            ZoneType = zoneType;
            Duration = duration;
        }
        public ParkingZone() { }
        public static ParkingZone ConvertToObject(string line)
        {
            string[] str = line.Split('|');
            if (str.Length != 4) return null;

            ParkingZone zone = new ParkingZone()
            {
                Id = str[0],
                ZoneType = str[1],
                Price = double.Parse(str[2]),
                Duration = double.Parse(str[3]),
            };
            return zone;
        }

        public override string ToString()
        {
            return $"{Id}|{ZoneType}|{Price}|{Duration}";
        }
    }
}
