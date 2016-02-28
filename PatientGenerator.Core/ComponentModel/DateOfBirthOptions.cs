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

using System;

namespace PatientGenerator.Core.ComponentModel
{
	/// <summary>
	/// Date of birth options for a patient.
	/// </summary>
	public class DateOfBirthOptions
	{
		/// <summary>
		/// Initializes a new instance of the DateOfBirthOptions class.
		/// </summary>
		public DateOfBirthOptions()
		{
		}

		/// <summary>
		/// The end range for the date of birth.
		/// </summary>
		public DateTime? End { get; set; }

		/// <summary>
		/// The start range for the date of birth.
		/// </summary>
		public DateTime? Start { get; set; }

		/// <summary>
		/// The exact date of birth to use for the patient.
		/// </summary>
		public DateTime? Exact { get; set; }
	}
}