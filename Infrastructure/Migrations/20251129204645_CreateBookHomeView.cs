using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateBookHomeView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DROP VIEW [dbo].[BookHome_View];");
		}
    }
}
