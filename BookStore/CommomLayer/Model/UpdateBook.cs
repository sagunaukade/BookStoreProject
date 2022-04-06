using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class UpdateBook
    {
        public long BookId { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public int OriginalPrice { get; set; }
        public int DiscountPrice { get; set; }
        public string BookDetails { get; set; }
        public string BookImage { get; set; }

    }
}
