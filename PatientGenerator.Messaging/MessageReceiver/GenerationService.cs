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

using MARC.HI.EHRS.SVC.Core;
using PatientGenerator.Core;
using PatientGenerator.Core.ComponentModel;
using PatientGenerator.Core.Validation;
using PatientGenerator.Messaging.Model;
using PatientGenerator.Messaging.Services;
using PatientGenerator.Messaging.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientGenerator.Messaging.MessageReceiver
{
	/// <summary>
	/// Provides operations to generate patients.
	/// </summary>
	public class GenerationService : IGenerationService, IDisposable
	{
		private HostContext hostContext;
		private IFhirSenderService fhirSenderService;
		private IHL7v2SenderService hl7v2SenderService;
		private IHL7v3SenderService hl7v3SenderService;
		private IPersistenceService persistenceService;

		/// <summary>
		/// Initializes a new instance of the GenerationService class.
		/// </summary>
		public GenerationService()
		{
			hostContext = new HostContext();

			fhirSenderService = hostContext.GetService(typeof(IFhirSenderService)) as IFhirSenderService;
			hl7v2SenderService = hostContext.GetService(typeof(IHL7v2SenderService)) as IHL7v2SenderService;
			hl7v3SenderService = hostContext.GetService(typeof(IHL7v3SenderService)) as IHL7v3SenderService;
			persistenceService = hostContext.GetService(typeof(IPersistenceService)) as IPersistenceService;
		}

		/// <summary>
		/// Generates patients using the provided options.
		/// </summary>
		/// <param name="options">The options to use to generate patients.</param>
		/// <returns>Returns a GenerationResponse.</returns>
		public GenerationResponse GeneratePatients(DemographicOptions options)
		{
			GenerationResponse response = new GenerationResponse();

			IEnumerable<IResultDetail> details = ValidationUtil.ValidateMessage(options);

			if (details.Count(x => x.Type == ResultDetailType.Error) > 0)
			{
				response.Messages = details.Select(x => x.ToString()).ToList();
				response.HasErrors = true;
			}
			else
			{
				// no validation errors, save the options
				persistenceService?.Save(options);

				// send to fhir endpoints 
				fhirSenderService?.Send(options);

				// send to hl7v2 endpoints 
				hl7v2SenderService?.Send(options);

				// send to hl7v3 endpoints 
				hl7v3SenderService?.Send(options);

			}

			return response;
		}

		/// <summary>
		/// Generates patients using the provided options.
		/// </summary>
		/// <param name="options">The options to use to generate patients.</param>
		/// <returns>Returns a GenerationResponse.</returns>
		public async Task<GenerationResponse> GeneratePatientsAsync(DemographicOptions options)
		{
			GenerationResponse response = new GenerationResponse();

			IEnumerable<IResultDetail> details = ValidationUtil.ValidateMessage(options);

			if (details.Count(x => x.Type == ResultDetailType.Error) > 0)
			{
				response.Messages = details.Select(x => x.ToString()).ToList();
				response.HasErrors = true;
			}
			else
			{
				// no validation errors, save the options
				await persistenceService?.SaveAsync(options);

				// send to fhir endpoints 
				await fhirSenderService?.SendAsync(options);

				// send to hl7v2 endpoints 
				await hl7v2SenderService?.SendAsync(options);

				// send to hl7v3 endpoints 
				await hl7v3SenderService?.SendAsync(options);
			}

			return response;
		}

		#region IDisposable Support

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					hostContext.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~GenerationService() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}

		#endregion IDisposable Support
	}
}