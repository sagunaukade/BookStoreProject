using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IAddressRL
    {
        public string AddAddress(AddressModel add, int userId);
        public AddressModel UpdateAddress(AddressModel add, int AddressId);
        public bool DeleteAddress(int addressId);
        public List<AddressModel> GetAllAddress(int userId);
    }
}
