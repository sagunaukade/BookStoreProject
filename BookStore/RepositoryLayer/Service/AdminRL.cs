using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Service
{
    public class AdminRL : IAdminRL
    {
        private SqlConnection sqlConnection;
        public AdminRL(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        private IConfiguration Configuration { get; }

        public AdminLogin AdminLogin(string email, string password)
        {
            try
            {
                this.sqlConnection = new SqlConnection(this.Configuration["ConnectionString:BookStore"]);
                SqlCommand cmd = new SqlCommand("LoginAdmin", this.sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                this.sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                AdminLogin admin = new AdminLogin();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        admin.AdminId = Convert.ToInt32(reader["AdminId"] == DBNull.Value ? default : reader["AdminId"]);
                        admin.FullName = Convert.ToString(reader["FullName"] == DBNull.Value ? default : reader["FullName"]);
                        admin.Email = Convert.ToString(reader["Email"] == DBNull.Value ? default : reader["Email"]);
                        admin.PhoneNumber = Convert.ToString(reader["PhoneNumber"] == DBNull.Value ? default : reader["PhoneNumber"]);
                    }

                    this.sqlConnection.Close();
                    admin.Token = this.GenerateJWTToken(admin);
                    return admin;
                }
                else
                {
                    throw new Exception("Email Or Password Is Wrong");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GenerateJWTToken(AdminLogin admin)
        {
            // header
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // payload
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("Email", admin.Email),
                new Claim("AdminId", admin.AdminId.ToString())
            };

            // signature
            var token = new JwtSecurityToken(
            this.Configuration["Jwt:Issuer"],
            this.Configuration["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
