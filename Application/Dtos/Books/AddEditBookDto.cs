using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Domain.Entities.Books;

namespace Application.Dtos.Books
{
    public class AddEditBookDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        [DisplayName("Book Title")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [DisplayName("Book Description")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "ISBN is required")]
        [StringLength(20, ErrorMessage = "ISBN cannot be longer than 20 characters")]
        public string ISBN { get; set; } = null!;

        [Required(ErrorMessage = "Author is required")]
        [StringLength(100, ErrorMessage = "Author name cannot be longer than 100 characters")]
        [DisplayName("Author Name")]
        public string Author { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "List Price must be a positive number")]
        [DisplayName("List Price")]
        public decimal ListPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price for +50 units must be a positive number")]
        [DisplayName("Price for +50 units")]
        public decimal Price50 { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price for +100 units must be a positive number")]
        [DisplayName("Price for +100 units")]
        public decimal Price100 { get; set; }


        [Required(ErrorMessage = "You must celect a category")]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [DisplayName("Main Image")]
        public string? MainImage { get; set; }

		[DisplayName("Other Images")]
		public List<TbBookImage> BookImages { get; set; } = [];

        public List<CategoryDto> Categories { get; set; } = [];
    }
}