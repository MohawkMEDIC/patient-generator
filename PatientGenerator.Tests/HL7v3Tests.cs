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
using PatientGenerator.HL7v3;
using System;
using System.Collections.Generic;

namespace PatientGenerator.Tests
{
	[TestClass]
	public class HL7v3Tests
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
						City = "Brampton",
						Country = "Canada",
						StreetAddress = "123 Main Street West",
						StateProvince = "Ontario",
						ZipPostalCode = "L6X0C3"
					},
					new AddressOptions
					{
						City = "New York City",
						Country = "United States of America",
						StreetAddress = "250 Madison Ave.",
						StateProvince = "New York",
						ZipPostalCode = "07008"
					},
					new AddressOptions
					{
						City = "Friedberg",
						Country = "Germany",
						StateProvince = "Elbonia",
						StreetAddress = "Grüner Weg 6",
						ZipPostalCode = "578233"
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
						FirstName = "David",
						LastName = "Nutley",
						MiddleNames = new List<string>
						{
							"Kimberly",
							"Rob",
							"Nutley"
						},
						Prefix = "Ms.",
						Suffixes = new List<string>
						{
							"PhD",
							"MSc"
						}
					}
				},
				OtherIdentifiers = new Dictionary<string, string>
				{
					{
						"1.3.6.1.4.1.33349.3.1.3.12", Guid.NewGuid().ToString("N")
					},
					{
						"1.2.840.114350.1.13.99998.8734", Guid.NewGuid().ToString("N")
					},
					{
						"1.3.6.1.4.1.33349.3.1.2.99121.9992", new Random(DateTime.Now.Second).Next(100, 10000).ToString()
					},
					{
						"1.3.6.1.4.1.33349.3.1.3.201203.1.0", new Random(DateTime.Now.Millisecond).Next(100, 10000).ToString()
					}
				},
				PersonIdentifier = Guid.NewGuid().ToString("N"),
				ReceivingApplication = "CRTEST",
				ReceivingFacility = "Mohawk College of Applied Arts and Technology",
				SendingApplication = "SEEDER",
				SendingFacility = "SEEDING"
			};
		}

		[TestMethod]
		public void NoAddressTest()
		{
			options.Addresses.Clear();

			options.Names.Clear();

			options.Names.Add(new NameOptions
			{
				FirstName = "Harry",
				LastName = "Homeless",
			});

			var message = EverestUtil.GenerateCandidateRegistry(options);

			var result = EverestUtil.Sendv3Messages(message, "cr");

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void NoDateOfBirthTest()
		{
			options.DateOfBirthOptions = null;
			options.Names.Clear();

			options.Names.Add(new NameOptions
			{
				FirstName = "Strawberry",
				LastName = "Shortcake",
			});

			var message = EverestUtil.GenerateCandidateRegistry(options);

			var result = EverestUtil.Sendv3Messages(message, "cr");

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void NoNameTest()
		{
			options.Names.Clear();

			var message = EverestUtil.GenerateCandidateRegistry(options);

			var result = EverestUtil.Sendv3Messages(message, "cr");

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void NoAlternateIdentifiersTest()
		{
			options.OtherIdentifiers.Clear();
			options.Names.Clear();

			options.Names.Add(new NameOptions
			{
				FirstName = "Walter",
				LastName = "Gretzky",
			});

			var message = EverestUtil.GenerateCandidateRegistry(options);

			var result = EverestUtil.Sendv3Messages(message, "cr");

			Assert.IsTrue(result);
		}

		//[TestMethod]
		//public void PartialAddressTest()
		//{
		//	options.Addresses.Clear();

		//	options.Addresses.Add(new AddressOptions
		//	{
		//		City = "Chicago",
		//		Country = "United States of America",
		//		StateProvince = "Illinois"
		//	});

		//	options.Names.Clear();

		//	options.Names.Add(new NameOptions
		//	{
		//		FirstName = "Joel",
		//		LastName = "Quinnville",
		//	});

		//	var message = EverestUtil.GenerateCandidateRegistry(options);

		//	var result = EverestUtil.Sendv3Messages(message, "cr");

		//	Assert.IsTrue(result);
		//}

		[TestMethod]
		public void ValidMessageTest()
		{
			var message = EverestUtil.GenerateCandidateRegistry(options);

			Assert.IsTrue(EverestUtil.Sendv3Messages(message, "cr"));
		}
	}
}