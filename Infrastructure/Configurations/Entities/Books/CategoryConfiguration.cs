using Domain.Entities.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Entities.Books
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<TbCategory>
    {
        public void Configure(EntityTypeBuilder<TbCategory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.ToTable("Categories");
        }
    }
}
