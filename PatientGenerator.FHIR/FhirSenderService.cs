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

using PatientGenerator.Core;
using PatientGenerator.Core.Common;
using PatientGenerator.Core.ComponentModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientGenerator.FHIR
{
	public class FhirSenderService : IFhirSenderService
	{
		private IServiceProvider context;

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
			var patient = FhirUtil.GenerateCandidateRegistry(options);

			FhirUtil.SendFhirMessages(patient);
		}

		public void Send(IEnumerable<Patient> patients)
		{
			throw new NotImplementedException();
		}

		public void Send(Patient patient)
		{
			throw new NotImplementedException();
		}

		public async Task SendAsync(DemographicOptions options)
		{
			await Task.Factory.StartNew(() =>
			{
				this.Send(options);
			});
		}

		public Task SendAsync(IEnumerable<Patient> patients)
		{
			throw new NotImplementedException();
		}

		public Task SendAsync(Patient patient)
		{
			throw new NotImplementedException();
		}
	}
}