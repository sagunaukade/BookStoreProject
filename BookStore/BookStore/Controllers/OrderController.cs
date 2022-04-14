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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderBL orderBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        public OrdersController(IOrderBL orderBL, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.orderBL = orderBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }
        [HttpPost("AddOrder")]
        public IActionResult AddOrders(OrderModel ordersModel)
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "Id").Value);
                var orderData = this.orderBL.AddOrder(ordersModel, userId);
                if (orderData != null)
                {
                    return this.Ok(new { Status = true, Message = "Order Placed Successfully", Response = orderData });
                }
                else 
                {
                   return this.BadRequest(new { Status = false, Message = "Enter Valid BookId" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Status = false, Message = ex.Message });
            }
        }
        
        [HttpGet("GetAllOrders")]
        public IActionResult GetAllOrders()
        {
            try
            {
                var userId = Convert.ToInt32(User.Claims.FirstOrDefault(u => u.Type == "Id").Value);
                var orderData = this.orderBL.GetAllOrder(userId);
                if (orderData != null)
                {
                    return this.Ok(new { Status = true, Message = "Order Fetched Successfully", Response = orderData });
                }
                else
                {
                    return this.BadRequest(new { Status = false, Message = "Please First Login" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Status = false, ex.Message });
            }
        }
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllOrdersUsingRedisCache()
        {
            var cacheKey = "OrderList";
            string serializedOrderList;
            var OrderList = new List<OrderModel>();
            var redisOrderList = await distributedCache.GetAsync(cacheKey);
            if (redisOrderList != null)
            {
                serializedOrderList = Encoding.UTF8.GetString(redisOrderList);
                OrderList = JsonConvert.DeserializeObject<List<OrderModel>>(serializedOrderList);
            }
            else
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "Id").Value);
                OrderList = (List<OrderModel>)orderBL.GetAllOrder(userId);
                serializedOrderList = JsonConvert.SerializeObject(OrderList);
                redisOrderList = Encoding.UTF8.GetBytes(serializedOrderList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisOrderList, options);
            }
            return Ok(OrderList);
        }
    }
}
