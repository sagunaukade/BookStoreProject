using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishlistBL wishlistBL;

        public WishListController(IWishlistBL wishlistBL)
        {
            this.wishlistBL = wishlistBL;
        }
        [HttpPost("AddWishlist")]
        public IActionResult AddInWishlist(int bookId)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var result = this.wishlistBL.AddInWishlist(bookId, userId);
                if (result.Equals("Book added in Wishlist"))
                {
                    return this.Ok(new { Status = true, Message = result });
                }
                else if (result.Equals("Book is Already in Wishlist"))
                {
                    return this.Ok(new { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = result });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }
    }
}

