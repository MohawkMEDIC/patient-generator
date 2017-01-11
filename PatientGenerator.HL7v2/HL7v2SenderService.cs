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
 * Date: 2016-2-28
 */

using NHapi.Base.Model;
using PatientGenerator.Core;
using PatientGenerator.Core.Common;
using PatientGenerator.Core.ComponentModel;
using PatientGenerator.HL7v2.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace PatientGenerator.HL7v2
{
	public class HL7v2SenderService : IHL7v2SenderService
	{
		private IServiceProvider context;

        HL7v2ConfigurationSection configuration = ConfigurationManager.GetSection("medic.patientgen.hl7v2") as HL7v2ConfigurationSection;

        public IServiceProvider Context
		{
			get
			{
				return this.context;
			}

			set
			{
				this.context = value;
			}
		}

		public void Send(DemographicOptions options)
		{
			var message = NHapiUtil.GenerateCandidateRegistry(options);
            
            NHapiUtil.Sendv2Messages(message, configuration.Endpoints);
		}

		public void Send(IEnumerable<Patient> patients)
		{
			List<IMessage> messages = new List<IMessage>();

			foreach (var patient in patients)
			{
				messages.Add(NHapiUtil.GenerateCandidateRegistry(patient, new Metadata
				{
					AssigningAuthority = "1.3.6.1.4.1.33349.3.1.2.99121.283",
					ReceivingApplication = "CRTEST",
					ReceivingFacility = "Mohawk College of Applied Arts and Technology",
					SendingApplication = "SEEDER",
					SendingFacility = "SEEDING"
				}));
			}

			messages.Select(x => NHapiUtil.Sendv2Messages(x, configuration.Endpoints));
		}

		public void Send(Patient patient)
		{
			var message = NHapiUtil.GenerateCandidateRegistry(patient, new Metadata
			{
				AssigningAuthority = "1.3.6.1.4.1.33349.3.1.2.99121.283",
				ReceivingApplication = "CRTEST",
				ReceivingFacility = "Mohawk College of Applied Arts and Technology",
				SendingApplication = "SEEDER",
				SendingFacility = "SEEDING"
			});

			NHapiUtil.Sendv2Messages(message, configuration.Endpoints);
		}

		public async Task SendAsync(DemographicOptions options)
		{
			await Task.Factory.StartNew(() =>
			{
				this.Send(options);
			});
		}

		public async Task SendAsync(IEnumerable<Patient> patients)
		{
			await Task.Factory.StartNew(() =>
			{
				this.Send(patients);
			});
		}

		public async Task SendAsync(Patient patient)
		{
			await Task.Factory.StartNew(() =>
			{
				this.Send(patient);
			});
		}
	}
}