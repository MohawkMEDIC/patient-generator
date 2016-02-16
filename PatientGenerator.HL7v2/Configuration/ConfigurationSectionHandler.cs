using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PatientGenerator.HL7v2.Configuration
{
	public class ConfigurationSectionHandler : IConfigurationSectionHandler
	{
		public object Create(object parent, object configContext, XmlNode section)
		{
			var retVal = new HL7v2ConfigurationSection();

			XmlElement endpointsNode = section.SelectSingleNode("./*[local-name() = 'endpoints']") as XmlElement;

			XmlNodeList endpoints = endpointsNode.SelectNodes("./*[local-name() = 'endpoint']") as XmlNodeList;

			foreach (XmlNode item in endpoints)
			{
				LlpEndpoint endpoint = new LlpEndpoint
				{
					Address = item.Attributes["address"].Value,
					Name = item.Attributes["name"].Value
				};

				retVal.Endpoints.Add(endpoint);
			}

			return retVal;
		}
	}
}
