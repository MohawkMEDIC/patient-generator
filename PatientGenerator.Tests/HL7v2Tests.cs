using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientGenerator.HL7v2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Tests
{
	[TestClass]
	public class HL7v2Tests
    {

		[TestInitialize]
		public void Initialize()
		{

		}

		[TestCleanup]
		public void Cleanup()
		{

		}

		[TestMethod]
		public void SendMessageTest()
		{
			NHapiUtil.GenerateCandidateRegistry(new Core.DemographicOptions
			{
			});
		}
    }
}
