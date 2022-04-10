using BusinessLayer.Interface;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class WishlistBL : IWishlistBL
    {
        private readonly IWishlistRL wishlistRL;
        public WishlistBL(IWishlistRL wishlistRL)
        {
            this.wishlistRL = wishlistRL;
        }
        public string AddInWishlist(int bookId, int userId)
        {
            try
            {
                return this.wishlistRL.AddInWishlist(bookId, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteFromWishlist(int userId, int wishlistId)
        {
            try
            {
                return this.wishlistRL.DeleteFromWishlist(userId, wishlistId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
