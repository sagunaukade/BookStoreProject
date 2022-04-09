using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IAddressBL
    {
        public string AddAddress(AddressModel add, int userId);
        public AddressModel UpdateAddress(AddressModel add, int addressId, int userId);

    }
}
