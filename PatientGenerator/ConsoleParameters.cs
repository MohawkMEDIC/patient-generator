using MohawkCollege.Util.Console.Parameters;
using System.ComponentModel;

namespace PatientGenerator
{
	/// <summary>
	/// Console parameters for startup.
	/// </summary>
	public class ConsoleParameters
	{
		/// <summary>
		/// When true, parameters should be shown.
		/// </summary>
		[Description("Shows help and exits")]
		[Parameter("?")]
		[Parameter("help")]
		public bool ShowHelp { get; set; }

		/// <summary>
		/// When true console mode should be enabled.
		/// </summary>
		[Description("Instructs the host process to run in console mode")]
		[Parameter("c")]
		[Parameter("console")]
		public bool ConsoleMode { get; set; }
	}
}