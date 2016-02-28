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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientGenerator.Persistence.Model
{
	public class Name
	{
		public Name()
		{
		}

		[Required]
		public DateTime CreationTimestamp { get; set; }

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public virtual ICollection<FirstName> FirstNames { get; set; }

		public virtual ICollection<LastName> LastNames { get; set; }

		public virtual ICollection<MiddleName> MiddleNames { get; set; }

		public virtual ICollection<NamePrefix> Prefixes { get; set; }

		public virtual ICollection<NameSuffix> Suffixes { get; set; }

		[Required]
		public NameUse NameUse { get; set; }

		public int PersonId { get; set; }

		[ForeignKey("PersonId")]
		public virtual Person Person { get; set; }
	}
}