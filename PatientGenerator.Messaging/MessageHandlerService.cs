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
 * Date: 2016-2-15
 */

using MARC.HI.EHRS.SVC.Core.Services;
using PatientGenerator.Messaging.MessageReceiver;
using System;
using System.Diagnostics;
using System.ServiceModel;

namespace PatientGenerator.Messaging
{
	/// <summary>
	/// The message handler service.
	/// </summary>
	public class MessageHandlerService : IMessageHandlerService, IDisposable
	{
		/// <summary>
		/// The ServiceHost for WCF services.
		/// </summary>
		private ServiceHost serviceHost;

		/// <summary>
		/// Fired when the object is starting up.
		/// </summary>
		public event EventHandler Started;

		/// <summary>
		/// Fired when the object is starting.
		/// </summary>
		public event EventHandler Starting;

		/// <summary>
		/// Fired when the service has stopped.
		/// </summary>
		public event EventHandler Stopped;

		/// <summary>
		/// Fired when the service is stopping.
		/// </summary>
		public event EventHandler Stopping;

		/// <summary>
		/// Gets the running state of the message handler.
		/// </summary>
		public bool IsRunning => this.serviceHost?.State == System.ServiceModel.CommunicationState.Opened;

		#region IDisposable Support

		private bool disposedValue = false; // To detect redundant calls

		// This code added to correctly implement the disposable pattern.

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					((IDisposable)this.serviceHost)?.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~MessageHandlerService() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		#endregion IDisposable Support

		/// <summary>
		/// Starts the message handler service.
		/// </summary>
		/// <returns>Returns true if the service(s) started successfully.</returns>
		public bool Start()
		{
			var status = false;

			serviceHost = new ServiceHost(typeof(GenerationService));

			try
			{
				this.Starting?.Invoke(this, EventArgs.Empty);
				serviceHost.Open();
				status = true;

				Trace.TraceInformation("Message handler started successfully");

				this.Started?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception e)
			{
				Trace.TraceError("Unable to start message handler");
				Trace.TraceError(e.ToString());
				status = false;
			}

			return status;
		}

		/// <summary>
		/// Stops the message handler service.
		/// </summary>
		/// <returns>Returns true if the service(s) stopped successfully.</returns>
		public bool Stop()
		{
			var status = false;

			try
			{
				this.Stopping?.Invoke(this, EventArgs.Empty);
				serviceHost.Close();
				status = true;

				Trace.TraceInformation("Message handler stopped successfully");

				this.Stopped?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception e)
			{
				Trace.TraceError("Unable to stop message handler");
				Trace.TraceError(e.ToString());
				status = false;
			}

			return status;
		}
	}
}