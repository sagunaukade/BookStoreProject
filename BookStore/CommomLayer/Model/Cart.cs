using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class AddCart
    {
        public int CartId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
