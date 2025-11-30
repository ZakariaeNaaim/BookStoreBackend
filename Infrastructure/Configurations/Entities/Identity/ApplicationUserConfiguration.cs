using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.Identity;

namespace Infrastructure.Configurations.Entities.Identity
{
    internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
			builder.Property(x => x.Name)
				.HasMaxLength(100)
				.IsRequired();

			builder.OwnsOne(x => x.AddressInfo, contact =>
			{
				contact.Property(x => x.City)
					.HasMaxLength(50)
					.IsRequired(false)
					.HasColumnName("City");

				contact.Property(x => x.State)
					.HasMaxLength(100)
					.IsRequired(false)
					.HasColumnName("State");

				contact.Property(x => x.StreetAddress)
					.HasMaxLength(150)
					.IsRequired(false)
					.HasColumnName("StreetAddress");

				contact.Property(x => x.PostalCode)
					.HasMaxLength(20)
					.IsRequired(false)
					.HasColumnName("PostalCode");
			});

			builder.Property(x => x.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired(false);

			builder.HasOne(x => x.Company)
				.WithMany(x => x.Users)
				.HasForeignKey(x => x.CompanyId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);
        }
    }
}
