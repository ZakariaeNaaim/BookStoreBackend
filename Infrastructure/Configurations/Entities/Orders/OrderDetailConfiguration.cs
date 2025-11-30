using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.Orders;

internal class OrderDetailConfiguration : IEntityTypeConfiguration<TbOrderDetail>
{
	public void Configure(EntityTypeBuilder<TbOrderDetail> builder)
	{
		// Set the primary key
		builder.HasKey(x => x.Id);

		// Configure properties
		builder.Property(x => x.Quantity)
			.IsRequired();

		builder.Property(x => x.Price)
			.HasColumnType("decimal(18,2)")
			.IsRequired();

		// Configure relationship with TbOrder
		builder.HasOne(x => x.Order)
			.WithMany(x => x.OrderDetails) // An order has many order details
			.HasForeignKey(x => x.OrderId)
			.OnDelete(DeleteBehavior.Restrict)
			.IsRequired();

		// Configure relationship with TbBook
		builder.HasOne(x => x.Book)
			.WithMany() // Assuming a book doesn't have a collection of order details
			.HasForeignKey(x => x.BookId)
			.OnDelete(DeleteBehavior.Restrict)
			.IsRequired();

		// Table name
		builder.ToTable("OrderDetails");
	}
}
