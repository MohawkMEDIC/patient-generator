using PatientGenerator.Messaging.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientGenerator.Core.ComponentModel;
using PatientGenerator.Messaging.Validation;
using PatientGenerator.Messaging.Model;

namespace PatientGenerator.Messaging.MessageReceiver
{
	public class GenerationService : IGenerationService, IDisposable
	{
		public GenerationService()
		{

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
					// TODO: dispose managed state (managed objects).
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
