//using Models.ViewModels.Admin.Books;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Infrastructure.Configurations
//{
//    internal class BookListViewModelConfiguration : IEntityTypeConfiguration<BookListViewModel>
//    {
//        public void Configure(EntityTypeBuilder<BookListViewModel> builder)
//        {
//            builder.HasNoKey()
//                .ToView("BookList_View");

//            builder.Property(b => b.Price)
//                   .HasColumnType("decimal(18,2)");
//        }
//    }
//}
