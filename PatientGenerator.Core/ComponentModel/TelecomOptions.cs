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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Core.ComponentModel
{
	/// <summary>
	/// Represents telecom options for a patient.
	/// </summary>
	public class TelecomOptions
	{
		/// <summary>
		/// Initializes a new instance of the TelecomOptions class.
		/// </summary>
		public TelecomOptions()
		{
			EmailAddresses = new List<string>();
			PhoneNumbers = new List<string>();
		}

		/// <summary>
		/// The email addresses of the patient.
		/// </summary>
		public List<string> EmailAddresses { get; set; }

		/// <summary>
		/// The phone numbers of the patient.
		/// </summary>
		public List<string> PhoneNumbers { get; set; }
	}
}
