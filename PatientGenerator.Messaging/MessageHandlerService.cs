using MARC.HI.EHRS.SVC.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Messaging
{
	public class MessageHandlerService : IMessageHandlerService
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

		public bool Start()
		{
			Debug.WriteLine("Message handler started");
			return true;
		}

		public bool Stop()
		{
			Debug.WriteLine("Message handler stopped");
			return true;
		}
	}
}
