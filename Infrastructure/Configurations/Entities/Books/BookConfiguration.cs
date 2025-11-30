using Domain.Entities.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Entities.Books
{
    internal class BookConfiguration : IEntityTypeConfiguration<TbBook>
    {
        public void Configure(EntityTypeBuilder<TbBook> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.ISBN)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Author)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.ListPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Price50)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Price100)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.CategoryId)
                .IsRequired();

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Books");
        }
    }
}
