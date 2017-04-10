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

using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PatientGenerator.Persistence.DAL
{
	public class EntityRepository<T> : IRepository<T> where T : class
	{
		private DbContext context;
		private DbSet<T> dbSet;

		public EntityRepository(DbContext context)
		{
			this.context = context;
			this.dbSet = context.Set<T>();
		}

		public virtual T Create()
		{
			return dbSet.Create();
		}

		public virtual void Add(T entity)
		{
			dbSet.Add(entity);
		}

		public virtual void Update(T entity)
		{
			dbSet.Attach(entity);
			context.Entry(entity).State = EntityState.Modified;
		}

		public virtual void Delete(T entity)
		{
			if (context.Entry(entity).State == EntityState.Detached)
			{
				dbSet.Attach(entity);
			}
			dbSet.Remove(entity);
		}

		public virtual void Delete(object id)
		{
			T entity = dbSet.Find(id);
			Delete(entity);
		}

		public virtual IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
		{
			IQueryable<T> query = AsQueryable();

			if (filter != null)
			{
				query = query.Where(filter);
			}

			if (orderBy != null)
			{
				return orderBy(query);
			}

			return query;
		}

		public virtual T GetById(object id)
		{
			return dbSet.Find(id);
		}

		public virtual IQueryable<T> AsQueryable()
		{
			return dbSet;
		}
	}
}