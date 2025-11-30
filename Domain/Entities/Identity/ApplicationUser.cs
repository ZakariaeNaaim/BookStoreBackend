using Domain.Entities.Common;
using Domain.Entities.Companies;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
	public class ApplicationUser : IdentityUser<int>
	{
		public string Name { get; set; } = null!;

		public AddressInfo AddressInfo { get; set; } = new();

		public int? CompanyId { get; set; }
		public TbCompany? Company { get; set; }
	}
}
