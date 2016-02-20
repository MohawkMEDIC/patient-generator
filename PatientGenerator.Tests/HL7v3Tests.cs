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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Tests
{
	[TestClass]
	public class HL7v3Tests
	{
		[TestCleanup]
		public void Cleanup()
		{

		}

		[TestInitialize]
		public void Initialize()
		{

		}

		[TestMethod]
		public void SendMessageTest()
		{
			var message = EverestUtil.GenerateCandidateRegistry(new DemographicOptions
			{
				Addresses = new List<AddressOptions>
				{
					//new Core.AddressOptions
					//{
					//	City = "Brampton",
					//	Country = "Canada",
					//	StreetAddress = "123 Main Street West",
					//	StateProvince = "Ontario",
					//	ZipPostalCode = "L6X0C3"
					//},
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
				PersonIdentifier = Guid.NewGuid().ToString("N")
			});

			Assert.IsTrue(EverestUtil.Sendv3Messages(message, "cr"));
		}
	}
}
