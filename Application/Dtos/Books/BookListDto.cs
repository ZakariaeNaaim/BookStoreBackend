using Microsoft.EntityFrameworkCore;

namespace Application.Dtos.Books
{
    public class BookListDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string ISBN { get; set; } = null!;

        [Precision(18, 2)]
        public decimal Price { get; set; }

        public string Category { get; set; } = null!;
    }
}
