using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientGenerator.Core
{
	public class NameOptions
	{
		public NameOptions()
		{

		}

		public string Prefix { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public List<string> MiddleNames { get; set; }

		public List<string> Suffixes { get; set; }
	}
}
