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
				Addresses = new List<Core.AddressOptions>
				{
					new Core.AddressOptions
					{
						City = "Brampton",
						Country = "Canada",
						StreetAddress = "123 Main Street West",
						StateProvince = "Ontario",
						ZipPostalCode = "L6X0C3"
					},
					new Core.AddressOptions
					{
						City = "Amritsar",
						Country = "India",
						StreetAddress = "42 Mal Road",
						StateProvince = "Punjab",
						ZipPostalCode = "143502"
					}
				},
				Names = new List<Core.NameOptions>
				{
					new Core.NameOptions
					{
						FirstName = "Barry",
						LastName = "Sanders"
					}
				}
			});
		}
    }
}
