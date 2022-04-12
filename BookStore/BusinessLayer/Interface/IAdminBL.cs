using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IAdminBL
    {
        public AdminLogin AdminLogin(string email, string password);
    }
}
