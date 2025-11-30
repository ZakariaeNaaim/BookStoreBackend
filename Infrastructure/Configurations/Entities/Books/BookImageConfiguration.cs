using Domain.Entities.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations.Entities.Books
{
    internal class BookImageConfiguration : IEntityTypeConfiguration<TbBookImage>
    {
        public void Configure(EntityTypeBuilder<TbBookImage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ImageUrl)
                .IsRequired();

            builder.HasOne(x => x.Book)
                .WithMany(x => x.BookImages)
                .HasForeignKey(x => x.BookId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("BookImages");
        }
    }
}
