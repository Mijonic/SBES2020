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
    public class Payment
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string CarId { get; set; }

        [DataMember]
        public string ParkingZoneId { get; set; }

        public static Payment ConvertToObject(string line)
        {
            string[] str = line.Split('|');
            if (str.Length != 3) return null;

            Payment payment = new Payment()
            {
                Id = str[0],
                CarId = str[1],
                ParkingZoneId = str[2],
            };
            return payment;
        }

        public override string ToString()
        {
            return $"{Id}|{CarId}|{ParkingZoneId}";
        }

    }
}
