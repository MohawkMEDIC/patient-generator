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
	public class MessageHandlerService : IMessageHandlerService
	{
		/// <summary>
		/// The IServiceProvider, an inherited member of the IUsesHostContext interface.
		/// </summary>
		private IServiceProvider context;

		/// <summary>
		/// The ServiceHost for WCF services.
		/// </summary>
		private ServiceHost serviceHost;

		/// <summary>
		/// The IServiceProvider context.
		/// </summary>
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

		/// <summary>
		/// Starts the message handler service.
		/// </summary>
		/// <returns>Returns true if the service(s) started successfully.</returns>
		public bool Start()
		{
			bool status = false;

			serviceHost = new ServiceHost(typeof(GenerationService));

			try
			{
				serviceHost.Open();
				status = true;

				Trace.TraceInformation("Message handler started successfully");
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
			bool status = false;

			try
			{
				serviceHost.Close();
				status = true;

				Trace.TraceInformation("Message handler stopped successfully");
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