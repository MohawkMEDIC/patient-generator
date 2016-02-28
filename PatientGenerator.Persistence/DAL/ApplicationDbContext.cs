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
 * Date: 2016-2-27
 */

using PatientGenerator.Persistence.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PatientGenerator.Persistence.DAL
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext()
			: base("PatientGeneratorDbConnection")
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
			modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
		}

		public DbSet<Address> Addresses { get; set; }

		public DbSet<AlternateIdentifier> AlternateIdentifiers { get; set; }

		public DbSet<Name> Names { get; set; }

		public DbSet<FirstName> FirstNames { get; set; }

		public DbSet<LastName> LastNames { get; set; }

		public DbSet<MiddleName> MiddleNames { get; set; }

		public DbSet<NamePrefix> NamePrefixes { get; set; }

		public DbSet<NameSuffix> NameSuffixes { get; set; }

		public DbSet<Person> Persons { get; set; }

		public DbSet<Telecom> Telecoms { get; set; }
	}
}