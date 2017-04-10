/*
 * Copyright 2016-2017 Mohawk College of Applied Arts and Technology
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

using System.Collections.Generic;
using System.Xml.Serialization;

namespace PatientGenerator.FHIR.Configuration
{
	/// <summary>
	/// Represents a Fhir configuration section.
	/// </summary>
	public class FhirConfigurationSection
	{
		/// <summary>
		/// Initializes a new instance of the FhirConfigurationSection class.
		/// </summary>
		public FhirConfigurationSection()
		{
			Endpoints = new List<FhirEndpoint>();
		}

		/// <summary>
		/// The list of endpoints for the fhir configuration section.
		/// </summary>
		[XmlElement("endpoints")]
		public List<FhirEndpoint> Endpoints { get; set; }
	}
}