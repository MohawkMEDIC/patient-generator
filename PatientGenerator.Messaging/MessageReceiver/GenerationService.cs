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
using PatientGenerator.Messaging.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientGenerator.Core.ComponentModel;
using PatientGenerator.Messaging.Validation;
using PatientGenerator.Messaging.Model;
using PatientGenerator.Core.Validation;
using PatientGenerator.Core;
using MARC.HI.EHRS.SVC.Core;

namespace PatientGenerator.Messaging.MessageReceiver
{
	public class GenerationService : IGenerationService, IDisposable
	{
		private HostContext hostContext;
		private IPersistenceService persistenceService;

		public GenerationService()
		{
			hostContext = new HostContext();
			persistenceService = hostContext.GetService(typeof(IPersistenceService)) as IPersistenceService;
		}

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

		#endregion
	}
}
