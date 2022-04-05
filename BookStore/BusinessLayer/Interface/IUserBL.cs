using CommomLayer.Model;
using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public UserModel Register(UserModel user);
        public UserLogin Login(string Email, string Password);
        public string ForgotPassword(string email);

    }
}
