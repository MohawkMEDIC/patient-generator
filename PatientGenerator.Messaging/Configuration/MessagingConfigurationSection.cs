using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PatientGenerator.Messaging.Configuration
{
	public class MessagingConfigurationSection
	{
		public MessagingConfigurationSection()
		{
			Endpoints = new List<ListenEndpoint>();
		}

		[XmlElement("endpoints")]
		public List<ListenEndpoint> Endpoints { get; set; }
	}
}
