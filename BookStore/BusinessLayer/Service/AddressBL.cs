using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class AddressBL : IAddressBL
    {
        private readonly IAddressRL addressRL;
        public AddressBL(IAddressRL addressRL)
        {
            this.addressRL = addressRL;
        }
        public string AddAddress(AddressModel add, int userId)
        {
            try
            {
                return this.addressRL.AddAddress(add, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public AddressModel UpdateAddress(AddressModel add, int AddressId)
        {
            try
            {
                return this.addressRL.UpdateAddress(add,AddressId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteAddress(int addressId)
        {
            try
            {
                return this.addressRL.DeleteAddress(addressId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
