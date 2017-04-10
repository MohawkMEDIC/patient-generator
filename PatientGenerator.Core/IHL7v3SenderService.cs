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
 * Date: 2016-2-28
 */

using MARC.HI.EHRS.SVC.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using PatientGenerator.Core.Model.Common;
using PatientGenerator.Core.Model.ComponentModel;

namespace PatientGenerator.Core
{
	/// <summary>
	/// Represents a service to send HL7v3 messages.
	/// </summary>
	public interface IHL7v3SenderService
	{
		/// <summary>
		/// Sends the specified options.
		/// </summary>
		/// <param name="options">The options.</param>
		void Send(DemographicOptions options);

		/// <summary>
		/// Sends the specified patients.
		/// </summary>
		/// <param name="patients">The patients.</param>
		void Send(IEnumerable<Patient> patients);

		/// <summary>
		/// Sends the specified patient.
		/// </summary>
		/// <param name="patient">The patient.</param>
		void Send(Patient patient);

		/// <summary>
		/// Sends the asynchronous.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <returns>Task.</returns>
		Task SendAsync(DemographicOptions options);

		/// <summary>
		/// Sends the asynchronous.
		/// </summary>
		/// <param name="patients">The patients.</param>
		/// <returns>Task.</returns>
		Task SendAsync(IEnumerable<Patient> patients);

		/// <summary>
		/// Sends the asynchronous.
		/// </summary>
		/// <param name="patient">The patient.</param>
		/// <returns>Task.</returns>
		Task SendAsync(Patient patient);
	}
}