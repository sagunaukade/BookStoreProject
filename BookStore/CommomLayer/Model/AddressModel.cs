using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class AddressModel
    {
        public int AddressId { get; set; }
        public string FullAddress { get; set; }
       //public int AddressType { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int TypeId { get; set; }
       public int UserId { get; set; }

    }
}
