using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Companies
{
    public class CompanyDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        [StringLength(150, ErrorMessage = "Company name cannot be longer than 150 characters")]
        [DisplayName("Company Name")]
        public string Name { get; set; } = null!;

        [StringLength(150, ErrorMessage = "Street address cannot be longer than 150 characters")]
        [DisplayName("Street Address")]
        public string? StreetAddress { get; set; }

        [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters")]
        public string? City { get; set; }

        [StringLength(100, ErrorMessage = "State cannot be longer than 100 characters")]
        public string? State { get; set; }

        [StringLength(20, ErrorMessage = "Postal code cannot be longer than 20 characters")]
        [RegularExpression(@"^[A-Za-z0-9\s\-]+$", ErrorMessage = "Invalid postal code format.")]
        [DisplayName("Postal Code")]
        public string? PostalCode { get; set; }

        [StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters")]
        [RegularExpression(@"^\+?[0-9\s\-]{7,20}$", ErrorMessage = "Invalid phone number format.")]
        [DisplayName("Phone Number")]
        public string? PhoneNumber { get; set; }
    }
}
