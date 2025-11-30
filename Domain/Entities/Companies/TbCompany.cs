using Domain.Entities.Common;
using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Companies
{
	public class TbCompany
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public AddressInfo AddressInfo { get; set; } = null!;
        public string? PhoneNumber { get; set; }

		public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
