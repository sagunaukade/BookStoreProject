using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IWishlistRL
    {
        public string AddInWishlist(int bookId, int userId);
        public bool DeleteFromWishlist(int userId, int wishlistId);
    }
}
