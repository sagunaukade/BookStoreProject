using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IBookRL
    {
        public BookModel AddBook(BookModel book);
        public UpdateBook UpdateBook(UpdateBook update);
        public bool DeleteBook(int bookId);
        public BookModel GetBookByBookId(int BookId);
    }
}
