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
	/// Represents a result detail.
	/// </summary>
	/// <seealso cref="PatientGenerator.Core.Validation.IResultDetail" />
	[Serializable]
	public class ResultDetail : IResultDetail
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ResultDetail"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public ResultDetail(string message)
		{
			this.Message = message;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ResultDetail"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="message">The message.</param>
		public ResultDetail(ResultDetailType type, string message) : this(type, message, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ResultDetail"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		public ResultDetail(ResultDetailType type, string message, Exception exception) : this(type, message, null, exception)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ResultDetail"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="message">The message.</param>
		/// <param name="location">The location.</param>
		/// <param name="exception">The exception.</param>
		public ResultDetail(ResultDetailType type, string message, string location, Exception exception)
		{
			this.Type = type;
			this.Message = message;
			this.Location = location;
			this.Exception = exception;
		}

		/// <summary>
		/// Gets the exception.
		/// </summary>
		/// <value>The exception.</value>
		public Exception Exception { get; }

		/// <summary>
		/// Gets or sets the location.
		/// </summary>
		/// <value>The location.</value>
		public string Location { get; set; }

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value>The message.</value>
		public virtual string Message { get; }

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
		public ResultDetailType Type { get; protected set; }

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return nameof(Type) + " " + this.Message == null ? "" : this.Message + " " + Location == null ? this.Location : "";
		}
	}
}