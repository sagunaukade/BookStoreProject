using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ICartRL
    {
        public CartModel AddCart(CartModel cart, int userId);
        public CartModel UpdateCart(CartModel cart, int userId);
        public bool DeleteCart(int cartId, int userId);



    }
}
