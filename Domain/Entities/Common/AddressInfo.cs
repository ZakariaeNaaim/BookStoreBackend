using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Common
{
	public class AddressInfo
	{
		public string? StreetAddress { get; set; }

		public string? City { get; set; }

		public string? State { get; set; }

		public string? PostalCode { get; set; }
	}
}
