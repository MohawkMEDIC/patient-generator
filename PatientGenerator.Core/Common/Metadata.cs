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

namespace PatientGenerator.Core.Common
{
	/// <summary>
	/// Represents metadata about patient registration.
	/// </summary>
	public class Metadata
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PatientGenerator.Core.ComponentModel.Metadata"/> class.
		/// </summary>
		public Metadata()
		{
		}

		/// <summary>
		/// The assigning authority to which the patient belongs.
		/// </summary>
		public string AssigningAuthority { get; set; }

		/// <summary>
		/// The receiving application.
		/// </summary>
		public string ReceivingApplication { get; set; }

		/// <summary>
		/// The receiving facility.
		/// </summary>
		public string ReceivingFacility { get; set; }

		/// <summary>
		/// The sending application.
		/// </summary>
		public string SendingApplication { get; set; }

		/// <summary>
		/// The sending facility.
		/// </summary>
		public string SendingFacility { get; set; }

		/// <summary>
		/// When true, the application will generate patients using FHIR messages.
		/// </summary>
		public bool UseFhir { get; set; }

		/// <summary>
		/// When true, the application will generate patients using HL7v2 messages.
		/// </summary>
		public bool UseHL7v2 { get; set; }

		/// <summary>
		/// When true, the application will generate patients using HL7v3 messages.
		/// </summary>
		public bool UseHL7v3 { get; set; }
	}
}