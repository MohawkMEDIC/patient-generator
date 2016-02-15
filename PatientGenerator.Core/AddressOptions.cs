using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Core
{
	public class AddressOptions
	{
		public AddressOptions()
		{
		}

		public string City { get; set; }

		public string Country { get; set; }

		public string ZipPostalCode { get; set; }

		public string StateProvince { get; set; }

		public string StreetAddress { get; set; }
	}
}
