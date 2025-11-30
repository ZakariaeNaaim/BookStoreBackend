using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserListView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
                CREATE VIEW [dbo].[UserList_View]
                AS
                SELECT
					u.Id,
                    u.Name, 
                    u.Email, 
                    u.PhoneNumber AS Phone, 
                    c.Name AS Company, 
                    r.Name AS Role,
					CASE  
						WHEN u.LockoutEnd > SYSDATETIMEOFFSET() THEN CAST(1 AS BIT)
						else CAST(0 AS BIT)
					END AS IsLocked
                FROM 
                    AspNetUserRoles ur
                INNER JOIN 
                    AspNetUsers u ON ur.UserId = u.Id
                LEFT JOIN 
                    AspNetRoles r ON ur.RoleId = r.Id
                LEFT JOIN 
                    Companies c ON u.CompanyId = c.Id;
            ");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DROP VIEW [dbo].[UserList_View];");
		}
    }
}
