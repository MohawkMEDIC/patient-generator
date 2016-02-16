using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PatientGenerator.Messaging.Configuration
{
	[XmlRoot("endpoint")]
	public class ListenEndpoint
	{
		public ListenEndpoint()
		{

		}

		[XmlAttribute("address")]
		public string Address { get; set; }

		[XmlAttribute("name")]
		public string Name { get; set; }
	}
}
