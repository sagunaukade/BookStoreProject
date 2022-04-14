using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartBL cartBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        public CartController(ICartBL cartBL, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.cartBL = cartBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }
        [Authorize(Roles = Role.User)]
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
        [Authorize(Roles = Role.User)]
        [HttpPut("UpdateCart")]
        public IActionResult UpdateCart(CartModel cart)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var cartData = this.cartBL.UpdateCart(cart, userId);
                if (cartData != null)
                {
                    return this.Ok(new { success = true, message = "Book Updated Successfully in Cart ", response = cartData });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Cart Updated failed" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, response = ex.Message });
            }
        }
        [Authorize(Roles = Role.User)]
        [HttpDelete("DeleteCart")]
        public IActionResult DeleteCart(int cartId)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                if (this.cartBL.DeleteCart(cartId, userId))
                {
                    return this.Ok(new { success = true, message = "Book Deleted Successfully from Cart " });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Cart Deleted failed" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, response = ex.Message });
            }
        }
        [Authorize(Roles = Role.User)]
        [HttpGet("{UserId}/GetCart")]
        public IActionResult GetCart()
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var cartData = this.cartBL.GetCartByUserId(userId);
                if (cartData != null)
                {
                    return this.Ok(new { success = true, message = "Cart Data Fetched Successfully ", response = cartData });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Enter Valid UserId" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, response = ex.Message });
            }
        }
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllBooksUsingRedisCache()
        {
            var cacheKey = "CartList";
            string serializedCartList;
            var CartList = new List<CartModel>();
            var redisCartList = await distributedCache.GetAsync(cacheKey);
            if (redisCartList != null)
            {
                serializedCartList = Encoding.UTF8.GetString(redisCartList);
                CartList = JsonConvert.DeserializeObject<List<CartModel>>(serializedCartList);
            }
            else
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                CartList = (List<CartModel>)cartBL.GetCartByUserId(userId);
                serializedCartList = JsonConvert.SerializeObject(CartList);
                redisCartList = Encoding.UTF8.GetBytes(serializedCartList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisCartList, options);
            }
            return Ok(CartList);
        }
    }
}
    
    
           
