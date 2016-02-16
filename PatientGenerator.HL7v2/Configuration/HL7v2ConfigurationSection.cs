using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PatientGenerator.HL7v2.Configuration
{
	public class HL7v2ConfigurationSection
	{
		public HL7v2ConfigurationSection()
		{
			Endpoints = new List<LlpEndpoint>();
		}

		[XmlElement("endpoints")]
		public List<LlpEndpoint> Endpoints { get; set; }
	}
}
