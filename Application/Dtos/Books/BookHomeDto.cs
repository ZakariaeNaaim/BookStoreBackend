namespace Application.Dtos.Books
{
    public class BookHomeDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public decimal ListPrice { get; set; }

        public decimal Price100 { get; set; }

        public string? MainImageUrl { get; set; } 
	}
}
