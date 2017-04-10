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

using System;

namespace PatientGenerator.Core.Model.Common
{
	/// <summary>
	/// Represents a patient.
	/// </summary>
	public class Patient
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Patient"/> class.
		/// </summary>
		public Patient()
		{
		}

		/// <summary>
		/// Gets or sets the address line.
		/// </summary>
		/// <value>The address line.</value>
		public string AddressLine { get; set; }

		/// <summary>
		/// Gets or sets the city.
		/// </summary>
		/// <value>The city.</value>
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the country.
		/// </summary>
		/// <value>The country.</value>
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the date of birth.
		/// </summary>
		/// <value>The date of birth.</value>
		public DateTime DateOfBirth { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>The email.</value>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>The first name.</value>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the gender.
		/// </summary>
		/// <value>The gender.</value>
		public string Gender { get; set; }

		/// <summary>
		/// Gets or sets the health card no.
		/// </summary>
		/// <value>The health card no.</value>
		public string HealthCardNo { get; set; }

		/// <summary>
		/// Gets or sets the language.
		/// </summary>
		/// <value>The language.</value>
		public string Language { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		/// <value>The last name.</value>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the name of the middle.
		/// </summary>
		/// <value>The name of the middle.</value>
		public string MiddleName { get; set; }

		/// <summary>
		/// Gets or sets the phone no.
		/// </summary>
		/// <value>The phone no.</value>
		public string PhoneNo { get; set; }

		/// <summary>
		/// Gets or sets the postal code.
		/// </summary>
		/// <value>The postal code.</value>
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets the province.
		/// </summary>
		/// <value>The province.</value>
		public string Province { get; set; }
	}
}