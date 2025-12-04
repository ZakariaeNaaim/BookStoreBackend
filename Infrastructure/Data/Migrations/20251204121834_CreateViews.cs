using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create BookHome_View
            migrationBuilder.Sql(@"
                CREATE VIEW [dbo].[BookHome_View]
                AS
                SELECT 
                    b.Id, 
                    b.Title, 
                    b.Author, 
                    b.ListPrice, 
                    b.Price100,
                    bi.ImageUrl AS MainImageUrl
                FROM 
                    Books b
                LEFT JOIN 
                    BookImages bi 
                ON 
                    b.Id = bi.BookId AND bi.IsMainImage = 1;
            ");

            // Create BookList_View
            migrationBuilder.Sql(@"
                CREATE VIEW [dbo].[BookList_View]
                AS
                SELECT 
                    b.Id, 
                    b.Title, 
                    b.Author, 
                    b.ISBN, 
                    b.ListPrice AS Price, 
                    c.Name AS Category
                FROM 
                    Books b
                INNER JOIN 
                    Categories c 
                ON 
                    b.CategoryId = c.Id;
            ");

            // Create UserList_View
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
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[BookHome_View];");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[BookList_View];");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[UserList_View];");
        }
    }
}
