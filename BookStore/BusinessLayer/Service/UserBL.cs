using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }
        public UserModel Register(UserModel user)
        {
            try
            {
                return this.userRL.Register(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserLogin Login(string Email, string Password)
        {
            try
            {
                return this.userRL.Login(Email, Password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ForgotPassword(string email)
        {
            try
            {
                return this.userRL.ForgotPassword(email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ResetPassword(string email, string newPassword, string confirmPassword)
        {
            try
            {
                return this.userRL.ResetPassword(email, newPassword, confirmPassword);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
