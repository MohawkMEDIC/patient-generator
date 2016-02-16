using MARC.HI.EHRS.SVC.Core;
using System.Diagnostics;
using System.ServiceProcess;

namespace PatientGenerator
{
	public partial class PatientGenerator : ServiceBase
	{
		public PatientGenerator()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			ExitCode = ServiceUtil.Start(typeof(Program).GUID);
			if (ExitCode != 0)
				Stop();

			Debug.WriteLine("Service Started");
		}

		protected override void OnStop()
		{
			Debug.WriteLine("Service Stopped");
			ServiceUtil.Stop();
		}
	}
}