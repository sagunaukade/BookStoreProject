using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class BookModel
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public int Rating { get; set; }
        public int TotalView { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public string BookDetails { get; set; }
        public string BookImage { get; set; }
    }
}
