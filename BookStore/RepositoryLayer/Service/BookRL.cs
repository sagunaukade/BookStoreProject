using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryLayer.Service
{
    public class BookRL : IBookRL
    {
        private SqlConnection sqlConnection;
        public BookRL(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        private IConfiguration Configuration { get; }

        public BookModel AddBook(BookModel book)
        {
            try
            {
                this.sqlConnection = new SqlConnection(this.Configuration["ConnectionString:Bookstore"]);
                SqlCommand cmd = new SqlCommand("AddBook", this.sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@bookName", book.BookName);
                cmd.Parameters.AddWithValue("@authorName", book.AuthorName);
                cmd.Parameters.AddWithValue("@rating", book.Rating);
                cmd.Parameters.AddWithValue("@totalview", book.TotalView);
                cmd.Parameters.AddWithValue("@originalPrice", book.OriginalPrice);
                cmd.Parameters.AddWithValue("@discountPrice", book.DiscountPrice);
                cmd.Parameters.AddWithValue("@bookdetails", book.BookDetails);
                cmd.Parameters.AddWithValue("@bookImage", book.BookImage);
                this.sqlConnection.Open();
                int i = cmd.ExecuteNonQuery();
                this.sqlConnection.Close();
                if (i >= 1)
                {
                    return book;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.sqlConnection.Close();
            }
        }
    }
}
        
