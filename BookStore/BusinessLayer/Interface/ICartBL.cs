using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface ICartBL
    {
        public CartModel AddCart(CartModel cart, int userId);
        public CartModel UpdateCart(CartModel cart, int userId);
        public bool DeleteCart(int cartId, int userId);
        public List<CartModel> GetCartByUserId(int userId);

    }
}
