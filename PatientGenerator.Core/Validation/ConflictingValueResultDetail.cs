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

namespace PatientGenerator.Core.Validation
{
	/// <summary>
	/// Represents a result detail indicating a conflicting value.
	/// </summary>
	/// <seealso cref="PatientGenerator.Core.Validation.ResultDetail" />
	public class ConflictingValueResultDetail : ResultDetail
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConflictingValueResultDetail"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public ConflictingValueResultDetail(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConflictingValueResultDetail"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="message">The message.</param>
		public ConflictingValueResultDetail(ResultDetailType type, string message) : base(type, message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConflictingValueResultDetail"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		public ConflictingValueResultDetail(ResultDetailType type, string message, Exception exception) : base(type, message, exception)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConflictingValueResultDetail"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="message">The message.</param>
		/// <param name="location">The location.</param>
		/// <param name="exception">The exception.</param>
		public ConflictingValueResultDetail(ResultDetailType type, string message, string location, Exception exception) : base(type, message, location, exception)
		{
		}
	}
}