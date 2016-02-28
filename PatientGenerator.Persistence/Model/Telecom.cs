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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientGenerator.Persistence.Model
{
	public class Telecom
	{
		public Telecom()
		{
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public DateTime CreationTimestamp { get; set; }

		public string Value { get; set; }

		public int TelecomTypeId { get; set; }

		[ForeignKey("TelecomTypeId")]
		public virtual TelecomType TelecomType { get; set; }

		public int TelecomUseId { get; set; }

		[ForeignKey("TelecomUseId")]
		public virtual TelecomUse TelecomUse { get; set; }
	}
}