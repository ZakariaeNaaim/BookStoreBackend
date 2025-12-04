using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Application.Dtos.Books;

namespace Infrastructure.Configurations
{
    internal class BookListViewModelConfiguration : IEntityTypeConfiguration<BookListDto>
    {
        public void Configure(EntityTypeBuilder<BookListDto> builder)
        {
            builder.HasNoKey()
                .ToView("BookList_View");

            builder.Property(b => b.Price)
                   .HasColumnType("decimal(18,2)");
        }
    }
}
