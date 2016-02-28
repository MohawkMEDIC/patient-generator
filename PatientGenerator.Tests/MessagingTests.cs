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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientGenerator.Core.ComponentModel;
using PatientGenerator.Messaging.MessageReceiver;
using System;
using System.Collections.Generic;

namespace PatientGenerator.Tests
{
	[TestClass]
	public class MessagingTests
	{
		private DemographicOptions options;

		[TestCleanup]
		public void Cleanup()
		{
			options = null;
		}

		[TestInitialize]
		public void Initialize()
		{
			options = new DemographicOptions
			{
				AssigningAuthority = "1.3.6.1.4.1.33349.3.1.2.99121.283",
				Addresses = new List<AddressOptions>
				{
					new AddressOptions
					{
						City = "New York City",
						Country = "United States of America",
						StreetAddress = "250 Madison Ave.",
						StateProvince = "New York",
						ZipPostalCode = "07008"
					},
					//new Core.AddressOptions
					//{
					//	City = "Friedberg",
					//	Country = "Germany",
					//	StreetAddress = "Grüner Weg 6",
					//	StateProvince = "Tempa",
					//	ZipPostalCode = "61169"
					//},
					new AddressOptions
					{
						City = "Salinas",
						Country = "United States of America",
						StreetAddress = "30 Mortensen Avenue",
						StateProvince = "California",
						ZipPostalCode = "93905"
					}
				},
				DateOfBirthOptions = new DateOfBirthOptions
				{
					Exact = new DateTime(new Random().Next(1900, 2014), new Random().Next(1, 12), new Random().Next(1, 28))
				},
				Gender = "M",
				Names = new List<NameOptions>
				{
					new NameOptions
					{
						FirstName = "Larry",
						LastName = "McDavid"
					}
				},
				OtherIdentifiers = new Dictionary<string, string>
				{
					{
						"1.3.6.1.4.1.33349.3.1.2.2016.27.02.0." + new Random(DateTime.Now.Millisecond).Next(100, 10000), Guid.NewGuid().ToString("N")
					},
					{
						"1.3.6.1.4.1.33349.3.1.2.2016.27.02.1." + new Random(DateTime.Now.Second).Next(100, 10000), Guid.NewGuid().ToString("N")
					},
					{
						"1.3.6.1.4.1.33349.3.1.2.2016.27.02.2." + new Random(DateTime.Now.Minute).Next(100, 10000), Guid.NewGuid().ToString("N")
					},
					{
						"1.3.6.1.4.1.33349.3.1.2.2016.27.02.3." + new Random(DateTime.Now.Hour).Next(100, 10000), Guid.NewGuid().ToString("N")
					}
				},
				PersonIdentifier = Guid.NewGuid().ToString("N"),
				ReceivingApplication = "CRTEST",
				ReceivingFacility = "Mohawk College of Applied Arts and Technology",
				SendingApplication = "SEEDER",
				SendingFacility = "SEEDING",
				UseHL7v2 = true
			};
		}

		[TestMethod]
		public void MissingAssigningAuthorityTestv2()
		{
			GenerationService service = new GenerationService();

			options.AssigningAuthority = null;
			options.ReceivingApplication = null;
			options.ReceivingFacility = null;
			options.SendingApplication = null;
			options.SendingFacility = null;
			options.UseHL7v2 = true;

			var result = service.GeneratePatients(options);

			Assert.IsTrue(result.HasErrors);
		}

		[TestMethod]
		public void MissingAssigningAuthorityTestv3()
		{
			GenerationService service = new GenerationService();

			options.AssigningAuthority = null;
			options.ReceivingApplication = null;
			options.ReceivingFacility = null;
			options.SendingApplication = null;
			options.SendingFacility = null;
			options.UseHL7v2 = false;

			var result = service.GeneratePatients(options);

			Assert.IsTrue(result.HasErrors);
		}

		[TestMethod]
		public void MissingDateOfBirthOptionsTest()
		{
			GenerationService service = new GenerationService();

			options.DateOfBirthOptions = null;

			var result = service.GeneratePatients(options);

			Assert.IsFalse(result.HasErrors);
		}

		[TestMethod]
		public void NoEndDateOfBirthOptionsTest()
		{
			GenerationService service = new GenerationService();

			options.DateOfBirthOptions = new DateOfBirthOptions
			{
				Start = new DateTime(1925, 01, 01),
				Exact = DateTime.Now
			};

			var result = service.GeneratePatients(options);

			Assert.IsTrue(result.HasErrors);
		}

		[TestMethod]
		public void NoStartDateOfBirthOptionsTest()
		{
			GenerationService service = new GenerationService();

			options.DateOfBirthOptions = new DateOfBirthOptions
			{
				End = new DateTime(1990, 12, 31),
				Exact = DateTime.Now
			};

			var result = service.GeneratePatients(options);

			Assert.IsTrue(result.HasErrors);
		}

		[TestMethod]
		public void RangeDateOfBirthTest()
		{
			GenerationService service = new GenerationService();

			options.DateOfBirthOptions = new DateOfBirthOptions
			{
				Start = new DateTime(1925, 01, 01),
				End = new DateTime(1990, 12, 31),
			};

			var result = service.GeneratePatients(options);

			Assert.IsFalse(result.HasErrors);
		}

		[TestMethod]
		public void RangeDateOfBirthWithExactTest()
		{
			GenerationService service = new GenerationService();

			options.DateOfBirthOptions = new DateOfBirthOptions
			{
				Start = new DateTime(1925, 01, 01),
				End = new DateTime(1990, 12, 31),
				Exact = DateTime.Now
			};

			var result = service.GeneratePatients(options);

			Assert.IsTrue(result.HasErrors);
		}

		[TestMethod]
		public void ValidMessageTestv2()
		{
			GenerationService service = new GenerationService();

			var result = service.GeneratePatients(options);

			Assert.IsFalse(result.HasErrors);
		}

		[TestMethod]
		public void ValidMessageTestv3()
		{
			GenerationService service = new GenerationService();

			options.ReceivingApplication = null;
			options.ReceivingFacility = null;
			options.SendingApplication = null;
			options.SendingFacility = null;
			options.UseHL7v2 = false;

			var result = service.GeneratePatients(options);

			Assert.IsFalse(result.HasErrors);
		}
	}
}