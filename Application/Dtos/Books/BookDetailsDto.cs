using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Books
{
	public class BookDetailsDto
    {
        public BookDetailsForAdminDto BookDetails { get; set; } = null!;

        [Range(1, 1000)]
        public int Quantity { get; set; }
    }
}