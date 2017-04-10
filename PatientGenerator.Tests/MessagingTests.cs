/*
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
 * Date: 2016-2-15
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientGenerator.Core.Model.ComponentModel;
using PatientGenerator.Messaging.MessageReceiver;
using System;
using System.Collections.Generic;

namespace PatientGenerator.Tests
{
	/// <summary>
	/// Contains tests for the messaging component.
	/// </summary>
	[TestClass]
	public class MessagingTests
	{
		/// <summary>
		/// The demographic options.
		/// </summary>
		private DemographicOptions options;

		/// <summary>
		/// Runs cleanup after each test execution.
		/// </summary>
		[TestCleanup]
		public void Cleanup()
		{
			options = null;
		}

		/// <summary>
		/// Runs initialization before each test execution.
		/// </summary>
		[TestInitialize]
		public void Initialize()
		{
			options = new DemographicOptions
			{
				Addresses = new List<Address>
				{
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
						StreetAddress = "Grüner Weg 6",
						StateProvince = "Tempa",
						ZipPostalCode = "61169"
					},
					new Address
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
				Metadata = new Core.Model.Metadata
				{
					AssigningAuthority = "1.3.6.1.4.1.33349.3.1.2.99121.283",
					ReceivingApplication = "CRTEST",
					ReceivingFacility = "Mohawk College of Applied Arts and Technology",
					SendingApplication = "SEEDER",
					SendingFacility = "SEEDING",
					UseHL7v2 = true
				},
				Names = new List<Name>
				{
					new Name
					{
						FirstName = "Larry",
						LastName = "McDavid"
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

		/// <summary>
		/// Tests that a date of birth range contains the start and end values and no exact value.
		/// </summary>
		[TestMethod]
		public void TestDateOfBirthRange()
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

		/// <summary>
		/// Tests that the model validation fails when a start date of birth and an exact date of birth are provided.
		/// </summary>
		[TestMethod]
		public void TestDateOfBirthRangeNoEnd()
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

		/// <summary>
		/// Tests that the model validation fails when a end date of birth and an exact date of birth are provided.
		/// </summary>
		[TestMethod]
		public void TestDateOfBirthRangeNoStart()
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

		/// <summary>
		/// Tests that the model validation fails when a start date of birth, an end date of birth, and an exact date of birth are provided.
		/// </summary>
		[TestMethod]
		public void TestDateOfBirthRangeWithExact()
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

		/// <summary>
		/// Tests that the model validation passes when no date of birth options are provided.
		/// </summary>
		[TestMethod]
		public void TestEmptyDateOfBirth()
		{
			GenerationService service = new GenerationService();

			options.DateOfBirthOptions = null;

			var result = service.GeneratePatients(options);

			Assert.IsFalse(result.HasErrors);
		}

		/// <summary>
		/// Tests that the model validation fails when no assigning authority is provided.
		/// </summary>
		[TestMethod]
		public void TestNullAssigningAuthorityV2()
		{
			GenerationService service = new GenerationService();

			options.Metadata.AssigningAuthority = null;
			options.Metadata.ReceivingApplication = null;
			options.Metadata.ReceivingFacility = null;
			options.Metadata.SendingApplication = null;
			options.Metadata.SendingFacility = null;
			options.Metadata.UseHL7v2 = true;

			var result = service.GeneratePatients(options);

			Assert.IsTrue(result.HasErrors);
		}

		/// <summary>
		/// Tests that the model validation fails when no assigning authority is provided.
		/// </summary>
		[TestMethod]
		public void TestNullAssigningAuthorityV3()
		{
			GenerationService service = new GenerationService();

			options.Metadata.AssigningAuthority = null;
			options.Metadata.ReceivingApplication = null;
			options.Metadata.ReceivingFacility = null;
			options.Metadata.SendingApplication = null;
			options.Metadata.SendingFacility = null;
			options.Metadata.UseHL7v2 = false;

			var result = service.GeneratePatients(options);

			Assert.IsTrue(result.HasErrors);
		}
	}
}