/*
 * Copyright 2016-2016 Mohawk College of Applied Arts and Technology
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you
 * may not use this file except in compliance with the License. You may
 * obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * User: Nityan
 * Date: 2016-2-21
 */

using System.Configuration;
using System.Diagnostics;
using System.Xml;

namespace PatientGenerator.FHIR.Configuration
{
	/// <summary>
	/// Represents the FHIR configuration section handler.
	/// </summary>
	public class ConfigurationSectionHandler : IConfigurationSectionHandler
	{
		/// <summary>
		/// The tracer.
		/// </summary>
		private readonly TraceSource tracer = new TraceSource("PatientGenerator.FHIR");

		/// <summary>
		/// Creates a configuration section handler.
		/// </summary>
		/// <param name="parent">Parent object.</param>
		/// <param name="configContext">Configuration context object.</param>
		/// <param name="section">Section XML node.</param>
		/// <returns>The created section handler object.</returns>
		/// <exception cref="ConfigurationErrorsException">Element 'endpoints' not found</exception>
		public object Create(object parent, object configContext, XmlNode section)
		{
			var configurationSection = new FhirConfigurationSection();

			var endpointsNode = section.SelectSingleNode("./*[local-name() = 'endpoints']") as XmlElement;

			if (endpointsNode == null)
			{
				this.tracer.TraceEvent(TraceEventType.Error, 0, "Element 'endpoints' not found");
				throw new ConfigurationErrorsException("Element 'endpoints' not found");
			}

			var endpoints = endpointsNode.SelectNodes("./*[local-name() = 'endpoint']");

			if (endpoints == null)
			{
				this.tracer.TraceEvent(TraceEventType.Error, 0, "'endpoints' must have at least one 'endpoint'");
				throw new ConfigurationErrorsException("'endpoints' must have at least one 'endpoint'");
			}

			foreach (XmlNode item in endpoints)
			{
				var endpoint = new FhirEndpoint
				{
					Address = item.Attributes["address"].Value,
					Name = item.Attributes["name"].Value
				};

				this.tracer.TraceEvent(TraceEventType.Verbose, 0, $"Adding endpoint: {endpoint.Address} {endpoint.Name}");
				configurationSection.Endpoints.Add(endpoint);
			}

			return configurationSection;
		}
	}
}