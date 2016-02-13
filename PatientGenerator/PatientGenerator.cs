using System.ServiceProcess;

namespace PatientGenerator
{
	public partial class PatientGenerator : ServiceBase
	{
		public PatientGenerator()
		{
			InitializeComponent();
		}

		internal void Start()
		{
			OnStart(null);
		}

		protected override void OnStart(string[] args)
		{
		}

		protected override void OnStop()
		{
		}
	}
}