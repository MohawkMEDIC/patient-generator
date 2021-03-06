﻿/*
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
 * Date: 2016-2-27
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientGenerator.Core.Model.ComponentModel;
using System;
using System.Collections.Generic;

namespace PatientGenerator.Tests
{
	[TestClass]
	public class PersistenceTests
	{
		private Demographic options;

		[TestCleanup]
		public void Cleanup()
		{
			options = null;
		}

		[TestInitialize]
		public void Initialize()
		{
			options = new Demographic
			{
				Addresses = new List<Address>
				{
					new Address
					{
						City = "Brampton",
						Country = "Canada",
						StreetAddress = "123 Main Street West",
						StateProvince = "Ontario",
						ZipPostalCode = "L6X0C3"
					},
					new Address
					{
						City = "New York City",
						Country = "United States of America",
						StreetAddress = "250 Madison Ave.",
						StateProvince = "New York",
						ZipPostalCode = "07008"
					},
					new Address
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
				Gender = "F",
				Metadata = new Core.Model.Metadata
				{
					AssigningAuthority = "1.3.6.1.4.1.33349.3.1.2.99121.283",
					ReceivingApplication = "CRTEST",
					ReceivingFacility = "Mohawk College of Applied Arts and Technology",
					SendingApplication = "SEEDER",
					SendingFacility = "SEEDING"
				},
				Names = new List<Name>
				{
					new Name
					{
						FirstName = "Samantha",
						LastName = "Richtofen",
						MiddleNames = new List<string>
						{
							"John",
							"David",
							"Kirkley"
						},
						Prefix = "Dr. "
					}
				},
				OtherIdentifiers = new List<AlternateIdentifier>
				{
					new AlternateIdentifier("1.3.6.1.4.1.33349.3.1.2.2016.27.02.0." + new Random(DateTime.Now.Millisecond).Next(100, 10000), Guid.NewGuid().ToString("N")),
					new AlternateIdentifier("1.3.6.1.4.1.33349.3.1.2.2016.27.02.1." + new Random(DateTime.Now.Second).Next(100, 10000), Guid.NewGuid().ToString("N")),
					new AlternateIdentifier("1.3.6.1.4.1.33349.3.1.2.2016.27.02.2." + new Random(DateTime.Now.Minute).Next(100, 10000), Guid.NewGuid().ToString("N")),
					new AlternateIdentifier("1.3.6.1.4.1.33349.3.1.2.2016.27.02.3." + new Random(DateTime.Now.Hour).Next(100, 10000), Guid.NewGuid().ToString("N"))
				},
				PersonIdentifier = Guid.NewGuid().ToString("N")
			};
		}

		//[TestMethod]
		//public void TestNoAddress()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	options.Addresses.Clear();

		//	var result = persistenceHandlerService.Save(options);

		//	Assert.IsTrue(result);
		//}

		//[TestMethod]
		//public async Task TestNoAddressAsync()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	options.Addresses.Clear();

		//	var result = await persistenceHandlerService.SaveAsync(options);

		//	Assert.IsTrue(result);
		//}

		//[TestMethod]
		//public void TestNoAlternateIdentifiers()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	options.OtherIdentifiers.Clear();

		//	var result = persistenceHandlerService.Save(options);

		//	Assert.IsTrue(result);
		//}

		//[TestMethod]
		//public async Task TestNoAlternateIdentifiersAsync()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	options.OtherIdentifiers.Clear();

		//	var result = await persistenceHandlerService.SaveAsync(options);

		//	Assert.IsTrue(result);
		//}

		//[TestMethod]
		//public void TestNoDateOfBirth()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	options.DateOfBirthOptions = null;

		//	var result = persistenceHandlerService.Save(options);

		//	Assert.IsTrue(result);
		//}

		//[TestMethod]
		//public async Task TestNoDateOfBirthAsync()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	options.DateOfBirthOptions = null;

		//	var result = await persistenceHandlerService.SaveAsync(options);

		//	Assert.IsTrue(result);
		//}

		//[TestMethod]
		//public void TestNoGender()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	options.Gender = null;

		//	var result = persistenceHandlerService.Save(options);

		//	Assert.IsTrue(result);
		//}

		//[TestMethod]
		//public async Task TestNoGenderAsync()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	options.Gender = null;

		//	var result = await persistenceHandlerService.SaveAsync(options);

		//	Assert.IsTrue(result);
		//}

		//[TestMethod]
		//public void TestNoName()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	options.Names.Clear();

		//	var result = persistenceHandlerService.Save(options);

		//	Assert.IsTrue(result);
		//}

		//[TestMethod]
		//public async Task TestNoNameAsync()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	options.Names.Clear();

		//	var result = await persistenceHandlerService.SaveAsync(options);

		//	Assert.IsTrue(result);
		//}

		//[TestMethod]
		//public void TestNoPersonIdentifier()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	options.PersonIdentifier = null;

		//	var result = persistenceHandlerService.Save(options);

		//	Assert.IsTrue(result);
		//}

		//[TestMethod]
		//public async Task TestNoPersonIdentifierAsync()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	options.PersonIdentifier = null;

		//	var result = await persistenceHandlerService.SaveAsync(options);

		//	Assert.IsTrue(result);
		//}

		//[TestMethod]
		//public void TestPartialAddress()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	options.Addresses.Clear();

		//	options.Addresses.Add(new AddressOptions
		//	{
		//		City = "Miami",
		//		Country = "United States of America",
		//		StateProvince = "Florida"
		//	});

		//	var result = persistenceHandlerService.Save(options);

		//	Assert.IsTrue(result);
		//}

		//[TestMethod]
		//public async Task TestPartialAddressAsync()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	options.Addresses.Clear();

		//	options.Addresses.Add(new AddressOptions
		//	{
		//		City = "Miami",
		//		Country = "United States of America",
		//		StateProvince = "Florida"
		//	});

		//	var result = await persistenceHandlerService.SaveAsync(options);

		//	Assert.IsTrue(result);
		//}

		//[TestMethod]
		//public void TestValidMessage()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	var result = persistenceHandlerService.Save(options);

		//	Assert.IsTrue(result);
		//}

		//[TestMethod]
		//public async Task TestValidMessageAsync()
		//{
		//	PersistenceHandlerService persistenceHandlerService = new PersistenceHandlerService();

		//	var result = await persistenceHandlerService.SaveAsync(options);

		//	Assert.IsTrue(result);
		//}
	}
}