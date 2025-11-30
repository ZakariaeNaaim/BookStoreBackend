namespace Domain.Entities.Books
{
    public class TbBook
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string ISBN { get; set; } = null!;

        public string Author { get; set; } = null!;

        public decimal ListPrice { get; set; }

        public decimal Price { get; set; }

        public decimal Price50 { get; set; }

        public decimal Price100 { get; set; }

        public int CategoryId { get; set; }

        public TbCategory Category { get; set; } = null!;

        public ICollection<TbBookImage> BookImages { get; set; } = new List<TbBookImage>();
    }
}
