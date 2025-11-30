using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Application.Dtos.Books
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        [DisplayName("Category Name")]
        [MaxLength(100, ErrorMessage = "Category Name cannot be longer than 100 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Display Order is required")]
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100")]
        public int DisplayOrder { get; set; }
    }
}
