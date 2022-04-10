using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IWishlistBL
    {
        public string AddInWishlist(int bookId, int userId);
    }
}
