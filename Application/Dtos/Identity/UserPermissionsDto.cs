using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Dtos.Identity
{
	public class UserPermissionsDto
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public string Role { get; set; }

		[DisplayName("Company")]
		public int? CompanyId { get; set; }

		public List<RoleDto> Roles { get; set; }

		public List<CompanyForPermissionsDto> Companies { get; set; }
	}

	public class RoleDto
	{
		public string Text { get; set; }
		public string Value { get; set; }
	}

	public class CompanyForPermissionsDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}


}
