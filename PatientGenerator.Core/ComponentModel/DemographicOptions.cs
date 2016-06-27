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
 * Date: 2016-2-15
 */

using PatientGenerator.Core.Common;
using System.Collections.Generic;

namespace PatientGenerator.Core.ComponentModel
{
	/// <summary>
	/// Represents demographic options for a patient.
	/// </summary>
	public class DemographicOptions
	{
		/// <summary>
		/// Initializes a new instance of the DemographicOptions class.
		/// </summary>
		public DemographicOptions()
		{
			Addresses = new List<AddressOptions>();
			DateOfBirthOptions = new DateOfBirthOptions();
			Names = new List<NameOptions>();
			OtherIdentifiers = new Dictionary<string, string>();
			TelecomOptions = new TelecomOptions();
		}

		/// <summary>
		/// The address options for the patient.
		/// </summary>
		public List<AddressOptions> Addresses { get; set; }

		/// <summary>
		/// The date of birth options for the patient.
		/// </summary>
		public DateOfBirthOptions DateOfBirthOptions { get; set; }

		/// <summary>
		/// The names of the patient.
		/// </summary>
		public List<NameOptions> Names { get; set; }

		/// <summary>
		/// The gender of the patient.
		/// </summary>
		public string Gender { get; set; }

		/// <summary>
		/// Metadata about the patient registration.
		/// </summary>
		public Metadata Metadata { get; set; }

		/// <summary>
		/// The primary identifier for the patient.
		/// </summary>
		public string PersonIdentifier { get; set; }

		/// <summary>
		/// Any other identifiers or alt ids for the patient.
		/// </summary>
		public Dictionary<string, string> OtherIdentifiers { get; set; }

		/// <summary>
		/// Telecom options for the patient.
		/// </summary>
		public TelecomOptions TelecomOptions { get; set; }
	}
}