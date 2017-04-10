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
	/// Represents name options for a patient.
	/// </summary>
	public class NameOptions
	{
		/// <summary>
		/// Initializes a new instance of the NameOptions class.
		/// </summary>
		public NameOptions()
		{
			this.MiddleNames = new List<string>();
			this.Suffixes = new List<string>();
		}

		/// <summary>
		/// The first name of the patient.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// The last name of the patient.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// The middle names of the patient.
		/// </summary>
		public List<string> MiddleNames { get; set; }

		/// <summary>
		/// The prefix of the patient's name.
		/// </summary>
		public string Prefix { get; set; }

		/// <summary>
		/// The suffixes of the patient's name.
		/// </summary>
		public List<string> Suffixes { get; set; }
	}
}