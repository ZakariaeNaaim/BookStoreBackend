using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateBookListView : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
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
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("DROP VIEW [dbo].[BookList_View];");
		}
	}
}
