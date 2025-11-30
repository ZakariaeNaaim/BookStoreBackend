using Application.Dtos.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Entities.Books
{
    internal class BookHomeDtoConfiguration : IEntityTypeConfiguration<BookHomeDto>
	{
		public void Configure(EntityTypeBuilder<BookHomeDto> builder)
		{
			builder.HasNoKey()
				.ToView("BookHome_View");

            builder.Property(b => b.ListPrice)
				.HasColumnType("decimal(18,2)");

            builder.Property(b => b.Price100)
				.HasColumnType("decimal(18,2)");

			builder.Property(b => b.Price100)
				.HasColumnType("decimal(18,2)");
		}
	}
}
