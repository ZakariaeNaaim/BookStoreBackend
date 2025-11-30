using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.Orders;

namespace Infrastructure.Configurations.Entities.Orders
{
	internal class OrderConfiguration : IEntityTypeConfiguration<TbOrder>
	{
		public void Configure(EntityTypeBuilder<TbOrder> builder)
		{
			// Set the primary key
			builder.HasKey(x => x.Id);

			// Configure properties
			builder.Property(x => x.OrderDate)
				.IsRequired();

			builder.Property(x => x.ShippingDate)
				.IsRequired();

			builder.Property(x => x.OrderTotal)
				.HasColumnType("decimal(18,2)")
				.IsRequired();

			builder.Property(x => x.OrderStatus)
				.HasMaxLength(50)
				.IsRequired(false);

			builder.Property(x => x.PaymentStatus)
				.HasMaxLength(50)
				.IsRequired(false);

			builder.Property(x => x.TrackingNumber)
				.HasMaxLength(100)
				.IsRequired(false);

			builder.Property(x => x.Carrier)
				.HasMaxLength(50)
				.IsRequired(false);

			builder.Property(x => x.PaymentIntentId)
				.HasMaxLength(100)
				.IsRequired(false);

			builder.Property(x => x.PaymentDate)
				.IsRequired();

			builder.Property(x => x.PaymentDueDate)
				.IsRequired();

			builder.Property(x => x.PhoneNumber)
				.HasMaxLength(20)
				.IsRequired();

			builder.Property(x => x.StreetAddress)
				.HasMaxLength(150)
				.IsRequired();

			builder.Property(x => x.City)
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(x => x.State)
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(x => x.PostalCode)
				.HasMaxLength(20)
				.IsRequired();

			builder.Property(x => x.Name)
				.HasMaxLength(100)
				.IsRequired();

			// Configure the relationship with ApplicationUser (User)
			builder.HasOne(x => x.User)
				.WithMany() // Assuming User doesn't have a collection of Orders
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Restrict)
				.IsRequired();

			// Configure the relationship with OrderDetails
			builder.HasMany(x => x.OrderDetails)
				.WithOne(x => x.Order)
				.HasForeignKey(x => x.OrderId)
				.OnDelete(DeleteBehavior.Restrict)
				.IsRequired();

            builder.Property(o => o.OrderStatus)
				.HasConversion<string>()
				.HasMaxLength(50)
				.IsRequired();

            builder.Property(o => o.PaymentStatus)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();


            builder.ToTable("Orders");
		}
	}
}
