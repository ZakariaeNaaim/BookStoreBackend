using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Books
{
    public class TbBookImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int BookId { get; set; }
        public bool IsMainImage { get; set; }
        public TbBook Book { get; set; }
    }
}
