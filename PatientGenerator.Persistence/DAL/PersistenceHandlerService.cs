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

using PatientGenerator.Core;
using PatientGenerator.Core.ComponentModel;
using PatientGenerator.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PatientGenerator.Persistence.DAL
{
	public class PersistenceHandlerService : IPersistenceService, IDisposable
	{
		private IServiceProvider context;
		private IUnitOfWork unitOfWork;

		public PersistenceHandlerService() : this(new EntityUnitOfWork(new ApplicationDbContext()))
		{
		}

		public PersistenceHandlerService(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = new EntityUnitOfWork(new ApplicationDbContext());
		}

		public IServiceProvider Context
		{
			get
			{
				return this.context;
			}
			set
			{
				this.context = value;
			}
		}

		private Person MapPerson(DemographicOptions options)
		{
			Person person = new Person();

			foreach (var item in options.Addresses)
			{
				Address address = new Address().Map(item);

				address.CreationTimestamp = DateTime.Now;

				person.Addresses.Add(address);
			}

			person.AssigningAuthority = options.Metadata.AssigningAuthority;
			person.CreationTimestamp = DateTime.Now;
			person.Gender = options.Gender;

			foreach (var item in options.OtherIdentifiers)
			{
				person.AlternateIdentifiers.Add(new AlternateIdentifier
				{
					CreationTimestamp = DateTime.Now,
					Key = item.AssigningAuthority,
					Value = item.Value
				});
			}

			foreach (var item in options.Names)
			{
				Name name = new Name
				{
					CreationTimestamp = DateTime.Now,
					FirstNames = new List<FirstName>
					{
						new FirstName
						{
							CreationTimestamp = DateTime.Now,
							Value = item.FirstName,
						}
					},
					LastNames = new List<LastName>
					{
						new LastName
						{
							CreationTimestamp = DateTime.Now,
							Value = item.LastName,
						},
					},
					Prefixes = new List<NamePrefix>
					{
						new NamePrefix
						{
							CreationTimestamp = DateTime.Now,
							Value = item.Prefix
						}
					},
					NameUse = NameUse.Legal
				};

				name.MiddleNames = new List<MiddleName>();

				foreach (var middleName in item.MiddleNames)
				{
					name.MiddleNames.Add(new MiddleName
					{
						CreationTimestamp = DateTime.Now,
						Value = middleName
					});
				}

				name.Suffixes = new List<NameSuffix>();

				if (item.Suffixes != null)
				{
					foreach (var suffix in item.Suffixes)
					{
						name.Suffixes.Add(new NameSuffix
						{
							CreationTimestamp = DateTime.Now,
							Value = suffix
						});
					}
				}

				person.Names.Add(name);
			}

			foreach (var item in options?.TelecomOptions?.EmailAddresses)
			{
				person.Telecoms.Add(new Telecom
				{
					CreationTimestamp = DateTime.Now,
					TelecomType = TelecomType.Email,
					TelecomUse = TelecomUse.Direct,
					Value = item
				});
			}

			foreach (var item in options?.TelecomOptions?.PhoneNumbers)
			{
				person.Telecoms.Add(new Telecom
				{
					CreationTimestamp = DateTime.Now,
					TelecomType = TelecomType.Phone,
					TelecomUse = TelecomUse.Direct,
					Value = item
				});
			}

			return person;
		}

		public bool Save(DemographicOptions options)
		{
			unitOfWork.PersonRepository.Add(this.MapPerson(options));
			return unitOfWork.Save();
		}

		public async Task<bool> SaveAsync(DemographicOptions options)
		{
			unitOfWork.PersonRepository.Add(this.MapPerson(options));
			return await unitOfWork.SaveAsync();
		}

		#region IDisposable Support

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					unitOfWork.Dispose();
#if DEBUG
					Trace.TraceInformation("Unit of Work disposed");
#endif
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~PersistenceHandlerService() {
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