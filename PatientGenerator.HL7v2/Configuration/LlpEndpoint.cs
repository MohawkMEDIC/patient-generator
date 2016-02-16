using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PatientGenerator.HL7v2.Configuration
{
	[XmlRoot("endpoint")]
	public class LlpEndpoint
	{
		public LlpEndpoint()
		{
		}

		[XmlAttribute("address")]
		public string Address { get; set; }

		[XmlAttribute("name")]
		public string Name { get; set; }
	}
}
