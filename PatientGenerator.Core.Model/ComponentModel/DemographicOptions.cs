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
 * Date: 2017-4-9
 */

using System.Collections.Generic;

namespace PatientGenerator.Core.Model.ComponentModel
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
			this.Addresses = new List<AddressOptions>();
			this.DateOfBirthOptions = new DateOfBirthOptions();
			this.Names = new List<NameOptions>();
			this.OtherIdentifiers = new List<AlternateIdentifierOptions>();
			this.RelatedPersons = new List<RelatedPersonOptions>();
			this.TelecomOptions = new TelecomOptions();
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
		/// The gender of the patient.
		/// </summary>
		public string Gender { get; set; }

		/// <summary>
		/// Metadata about the patient registration.
		/// </summary>
		public Metadata Metadata { get; set; }

		/// <summary>
		/// The names of the patient.
		/// </summary>
		public List<NameOptions> Names { get; set; }

		/// <summary>
		/// Any other identifiers or alt ids for the patient.
		/// </summary>
		public List<AlternateIdentifierOptions> OtherIdentifiers { get; set; }

		/// <summary>
		/// The primary identifier for the patient.
		/// </summary>
		public string PersonIdentifier { get; set; }

		/// <summary>
		/// Gets or sets the list of related persons of the patient.
		/// </summary>
		public List<RelatedPersonOptions> RelatedPersons { get; set; }

		/// <summary>
		/// Telecom options for the patient.
		/// </summary>
		public TelecomOptions TelecomOptions { get; set; }
	}
}