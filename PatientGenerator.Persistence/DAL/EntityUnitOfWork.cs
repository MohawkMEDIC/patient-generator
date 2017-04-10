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
 * Date: 2016-2-27
 */

using PatientGenerator.Persistence.Model;
using System.Threading.Tasks;

namespace PatientGenerator.Persistence.DAL
{
	public class EntityUnitOfWork : IUnitOfWork
	{
		private ApplicationDbContext context;

		private IRepository<Address> addressRepository;
		private IRepository<AlternateIdentifier> alternateIdentifierRepository;
		private IRepository<Name> nameRepository;
		private IRepository<Person> personRepository;
		private IRepository<Telecom> telecomRepository;

		public EntityUnitOfWork()
			: this(new ApplicationDbContext())
		{
		}

		public EntityUnitOfWork(ApplicationDbContext context)
		{
			this.context = context;
		}

		#region IUnitOfWork

		public IRepository<Address> AddressRepository
		{
			get
			{
				if (this.addressRepository == null)
				{
					this.addressRepository = new EntityRepository<Address>(context);
				}

				return this.addressRepository;
			}
		}

		public IRepository<AlternateIdentifier> AlternateIdentifierRepository
		{
			get
			{
				if (this.alternateIdentifierRepository == null)
				{
					this.alternateIdentifierRepository = new EntityRepository<AlternateIdentifier>(context);
				}

				return this.alternateIdentifierRepository;
			}
		}

		public IRepository<Name> NameRepository
		{
			get
			{
				if (this.nameRepository == null)
				{
					this.nameRepository = new EntityRepository<Name>(context);
				}

				return this.nameRepository;
			}
		}

		public IRepository<Person> PersonRepository
		{
			get
			{
				if (this.personRepository == null)
				{
					this.personRepository = new EntityRepository<Person>(context);
				}

				return this.personRepository;
			}
		}

		public IRepository<Telecom> TelecomRepository
		{
			get
			{
				if (this.telecomRepository == null)
				{
					this.telecomRepository = new EntityRepository<Telecom>(context);
				}

				return this.telecomRepository;
			}
		}

		public bool Save()
		{
			// if the change count is greater than 0, then changes were saved.
			int changeCount = context.SaveChanges();

			return changeCount > 0;
		}

		public async Task<bool> SaveAsync()
		{
			int changeCount = await context.SaveChangesAsync();

			return changeCount > 0;
		}

		#endregion IUnitOfWork

		#region IDisposable Support

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					context.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~GenerationService() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}

		#endregion IDisposable Support
	}
}