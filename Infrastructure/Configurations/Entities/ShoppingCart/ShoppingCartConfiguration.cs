using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.ShoppingCart;

namespace Infrastructure.Configurations.Entities.ShoppingCart
{
	internal class ShoppingCartConfiguration : IEntityTypeConfiguration<TbShoppingCart>
	{
		public void Configure(EntityTypeBuilder<TbShoppingCart> builder)
		{
			builder.HasKey(x => x.Id);

			builder.HasOne(x => x.Book)
				.WithMany()
				.HasForeignKey(x => x.BookId)
				.IsRequired();

			builder.HasOne(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.UserId)
				.IsRequired();

			builder.ToTable("ShoppingCarts");
		}
	}
}
