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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Core.Validation
{
	[Serializable]
	public class ResultDetail : IResultDetail
	{
		public ResultDetail(string message)
		{
			this.Message = message;
		}

		public ResultDetail(ResultDetailType type, string message) : this(type, message, null)
		{
		}

		public ResultDetail(ResultDetailType type, string message, Exception exception) : this(type, message, null, exception)
		{

		}

		public ResultDetail(ResultDetailType type, string message, string location, Exception exception)
		{
			this.Type = type;
			this.Message = message;
			this.Location = location;
			this.Exception = exception;
		}

		public Exception Exception { get; }

		public string Location { get; set; }

		public virtual string Message { get; }

		public ResultDetailType Type { get; protected set; }

		public override string ToString()
		{
			return nameof(Type) + " " + this.Message == null ? "" : this.Message + " " + Location == null ? this.Location : "";
		}
	}
}
