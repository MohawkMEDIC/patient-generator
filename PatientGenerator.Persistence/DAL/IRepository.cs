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

using System;
using System.Linq;
using System.Linq.Expressions;

namespace PatientGenerator.Persistence.DAL
{
	public interface IRepository<T> where T : class
	{
		/// <summary>
		/// Create an entity.
		/// </summary>
		/// <returns></returns>
		T Create();

		/// <summary>
		/// Add an entity to the repository.
		/// </summary>
		/// <param name="entity"></param>
		void Add(T entity);

		/// <summary>
		/// Update an existing entity.
		/// </summary>
		/// <param name="entity"></param>
		void Update(T entity);

		/// <summary>
		/// Delete an entity from the repository.
		/// </summary>
		/// <param name="entity"></param>
		void Delete(T entity);

		/// <summary>
		/// Delete an entity from the repository by its id.
		/// </summary>
		/// <param name="id"></param>
		void Delete(object id);

		/// <summary>
		/// Query the repository.
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="orderBy"></param>
		/// <returns></returns>
		IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

		/// <summary>
		/// Get an entity from the repository by its id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		T GetById(object id);

		/// <summary>
		/// Get the repository as a queryable.
		/// </summary>
		/// <returns></returns>
		IQueryable<T> AsQueryable();
	}
}