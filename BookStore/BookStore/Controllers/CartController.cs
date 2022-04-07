using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartBL cartBL;
        public CartController(ICartBL cartBL)
        {
            this.cartBL = cartBL;
        }
        [HttpPost("AddCart")]
        public IActionResult AddCart(CartModel cart)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var cartData = this.cartBL.AddCart(cart, userId);
                if (cartData != null)
                {
                    return this.Ok(new { success = true, message = "Book Added SuccessFully in Cart ", response = cartData });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Cart Added Unsuccessfully" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, response = ex.Message });
            }
        }

        [HttpPut("UpdateCart")]
        public IActionResult UpdateCart(CartModel cart)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var cartData = this.cartBL.UpdateCart(cart, userId);
                if (cartData != null)
                {
                    return this.Ok(new { success = true, message = "Book Updated in Cart ", response = cartData });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "cart Update failed" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, response = ex.Message });
            }
        }
    }
}
    
    
           
