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
    public class Car
    {
        [DataMember]
        public string Registration { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public string Brand { get; set; }

        [DataMember]
        public bool PenaltyTicket { get; set; }

		public override string ToString()
		{
			return $"{Registration}|{Color}|{Brand}|{PenaltyTicket}";
		}

		public static Car ConvertToObject(string line)
		{
			string[] str = line.Split('|');
			if (str.Length != 4) return null;

			Car car = new Car()
			{
				Registration = str[0],
				Color = str[1],
				Brand = str[2],
                PenaltyTicket = Boolean.Parse(str[3])
            };
			return car;
		}

	}
}
