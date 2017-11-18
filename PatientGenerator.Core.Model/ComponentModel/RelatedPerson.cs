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
 * Date: 2017-01-11
 */

using System.Collections.Generic;

namespace PatientGenerator.Core.Model.ComponentModel
{
	/// <summary>
	/// Represents related person options.
	/// </summary>
	public class RelatedPerson
	{
		/// <summary>
		/// Represents a patient relationship type.
		/// </summary>
		public enum PatientRelationshipType
		{
			/// <summary>
			/// Represents a relationship type of Mother.
			/// </summary>
			MTH,

			/// <summary>
			/// Represents a relationship type of Father.
			/// </summary>
			FTH
		}

		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		/// <value>The address.</value>
		public List<Address> Address { get; set; }

		/// <summary>
		/// Gets or sets the names.
		/// </summary>
		/// <value>The names.</value>
		public List<Name> Names { get; set; }

		/// <summary>
		/// Gets or sets the phone.
		/// </summary>
		/// <value>The phone.</value>
		public string Phone { get; set; }

		/// <summary>
		/// Gets or sets the relationship.
		/// The relationship of this person to the patient. Stored as a relationship code.
		/// https://www.hl7.org/fhir/v2/0063/index.html
		/// </summary>
		/// <value>The relationship.</value>
		public PatientRelationshipType Relationship { get; set; }
	}
}