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
    public class BookController : ControllerBase
    {
        private readonly IBookBL bookBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        public BookController(IBookBL bookBL, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.bookBL = bookBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }
        [Authorize(Roles = Role.Admin)]
        [HttpPost("AddBook")]
        public IActionResult AddBook(BookModel book)
        {
            try
            {
                var bookDetail = this.bookBL.AddBook(book);
                if (bookDetail != null)
                {
                    return this.Ok(new { Success = true, message = "Book Added Successfully", Response = bookDetail });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Book Added Unsuccessfully" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }
        [Authorize(Roles = Role.Admin)]
        [HttpPost("UpdateBook")]
        public IActionResult UpdateBook(UpdateBook update)
        {
            try
            {
                var user = this.bookBL.UpdateBook(update);
                if (user != null)
                {
                    return this.Ok(new { Success = true, message = "Book Details Updated Successfully", });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Book details Updated Unsuccessfully" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("DeleteBook")]
        public IActionResult DeleteBook(int bookId)
        {
            try
            {
                if (this.bookBL.DeleteBook(bookId))
                {
                    return this.Ok(new { Success = true, message = "Book Deleted Sucessfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Enter Valid Book Id" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }
        [Authorize(Roles = Role.User)]
        [HttpGet("{BookId}/Get")]
        public IActionResult GetBookByBookId(int BookId)
        {
            try
            {
                var book = this.bookBL.GetBookByBookId(BookId);
                if (book != null)
                {
                    return this.Ok(new { Success = true, message = "Book Detail Fetched Sucessfully", Response = book });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Enter Valid Book Id" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }
        [Authorize(Roles = Role.User)]
        [HttpGet("GetAllBook")]
        public IActionResult GetBook()
        {
            try
            {
                var updatedBookDetail = this.bookBL.GetAllBooks();
                if (updatedBookDetail != null)
                {
                    return this.Ok(new { Success = true, message = "Book Detail Fetched Sucessfully", Response = updatedBookDetail });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Enter Valid Book Id" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllBooksUsingRedisCache()
        {
            var cacheKey = "BookList";
            string serializedBookList;
            var BookList = new List<BookModel>();
            var redisBookList = await distributedCache.GetAsync(cacheKey);
            if (redisBookList != null)
            {
                serializedBookList = Encoding.UTF8.GetString(redisBookList);
                BookList = JsonConvert.DeserializeObject<List<BookModel>>(serializedBookList);
            }
            else
            {
                BookList = (List<BookModel>)bookBL.GetAllBooks();
                serializedBookList = JsonConvert.SerializeObject(BookList);
                redisBookList = Encoding.UTF8.GetBytes(serializedBookList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisBookList, options);
            }
            return Ok(BookList);
        }
    }
}
