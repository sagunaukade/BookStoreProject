using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IBookBL
    {
        public BookModel AddBook(BookModel book);
        public UpdateBook UpdateBook(UpdateBook update);
        public bool DeleteBook(int bookId);

    }
}
