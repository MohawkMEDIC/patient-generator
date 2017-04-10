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

using PatientGenerator.Core;
using PatientGenerator.Core.Model;
using PatientGenerator.Core.Model.Common;
using PatientGenerator.Core.Model.ComponentModel;
using PatientGenerator.HL7v2.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace PatientGenerator.HL7v2
{
	/// <summary>
	/// Represents a service to send HL7v2 messages.
	/// </summary>
	/// <seealso cref="PatientGenerator.Core.IHL7v2SenderService" />
	public class HL7v2SenderService : IHL7v2SenderService
	{
		/// <summary>
		/// The configuration.
		/// </summary>
		private readonly HL7v2ConfigurationSection configuration = ConfigurationManager.GetSection("medic.patientgen.hl7v2") as HL7v2ConfigurationSection;

		/// <summary>
		/// Sends the specified options.
		/// </summary>
		/// <param name="options">The options.</param>
		public void Send(DemographicOptions options)
		{
			var message = NHapiUtility.GenerateCandidateRegistry(options);

			NHapiUtility.Sendv2Messages(message, configuration.Endpoints);
		}

		/// <summary>
		/// Sends the specified patients.
		/// </summary>
		/// <param name="patients">The patients.</param>
		public void Send(IEnumerable<Patient> patients)
		{
			var messages = patients.Select(patient => NHapiUtility.GenerateCandidateRegistry(patient, new Metadata
			{
				AssigningAuthority = "1.3.6.1.4.1.33349.3.1.5.102.4.20",
				ReceivingApplication = "OpenIZ",
				ReceivingFacility = "OpenIZ",
				SendingApplication = "Test",
				SendingFacility = "Test"
			}))
									.ToList();

			messages.Select(x => NHapiUtility.Sendv2Messages(x, configuration.Endpoints));
		}

		/// <summary>
		/// Sends the specified patient.
		/// </summary>
		/// <param name="patient">The patient.</param>
		public void Send(Patient patient)
		{
			var message = NHapiUtility.GenerateCandidateRegistry(patient, new Metadata
			{
				AssigningAuthority = "1.3.6.1.4.1.33349.3.1.5.102.4.20",
				ReceivingApplication = "OpenIZ",
				ReceivingFacility = "OpenIZ",
				SendingApplication = "Test",
				SendingFacility = "Test"
			});

			NHapiUtility.Sendv2Messages(message, configuration.Endpoints);
		}

		/// <summary>
		/// send as an asynchronous operation.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <returns>Task.</returns>
		public async Task SendAsync(DemographicOptions options)
		{
			await Task.Factory.StartNew(() =>
			{
				this.Send(options);
			});
		}

		/// <summary>
		/// send as an asynchronous operation.
		/// </summary>
		/// <param name="patients">The patients.</param>
		/// <returns>Task.</returns>
		public async Task SendAsync(IEnumerable<Patient> patients)
		{
			await Task.Factory.StartNew(() =>
			{
				this.Send(patients);
			});
		}

		/// <summary>
		/// send as an asynchronous operation.
		/// </summary>
		/// <param name="patient">The patient.</param>
		/// <returns>Task.</returns>
		public async Task SendAsync(Patient patient)
		{
			await Task.Factory.StartNew(() =>
			{
				this.Send(patient);
			});
		}
	}
}