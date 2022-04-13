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
    [Authorize(Roles = Role.User)]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishlistBL wishlistBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;

        public WishListController(IWishlistBL wishlistBL, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.wishlistBL = wishlistBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }
        [HttpPost("AddWishlist")]
        public IActionResult AddInWishlist(int bookId)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var result = this.wishlistBL.AddInWishlist(bookId, userId);
                if (result.Equals("Successfully Book added in Wishlist"))
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
        [HttpDelete("DeleteFromWishlist")]
        public IActionResult DeleteFromWishlist(int wishlistId)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                if (this.wishlistBL.DeleteFromWishlist(userId, wishlistId))
                {
                    return this.Ok(new { Status = true, Message = "Successfully Deleted From Wishlist" });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Something Went Wrong" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }
        [HttpGet("{UserId}/ GetAllRecordFromWishlist")]
        public IActionResult GetAllRecordFromWishlist()
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                var wishData = this.wishlistBL.GetAllFromWishlist(userId);
                if (wishData != null)
                {
                    return this.Ok(new { success = true, message = "All Wishlist Records Fetched Successfully ", response = wishData });
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
            var cacheKey = "WishList";
            string serializedWishList;
            var WishList = new List<WishlistModel>();
            var redisWishList = await distributedCache.GetAsync(cacheKey);
            if (redisWishList != null)
            {
                serializedWishList = Encoding.UTF8.GetString(redisWishList);
                WishList = JsonConvert.DeserializeObject<List<WishlistModel>>(serializedWishList);
            }
            else
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                WishList = (List<WishlistModel>)wishlistBL.GetAllFromWishlist(userId);
                serializedWishList = JsonConvert.SerializeObject(WishList);
                redisWishList = Encoding.UTF8.GetBytes(serializedWishList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisWishList, options);
            }
            return Ok(WishList);
        }
    }
}