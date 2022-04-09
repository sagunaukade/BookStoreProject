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
    public class AddressRL : IAddressRL
    {
        private SqlConnection sqlConnection;

        public AddressRL(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        private IConfiguration Configuration { get; }

        public string AddAddress(AddressModel add, int userId)
        {
            try
            {
                this.sqlConnection = new SqlConnection(this.Configuration["ConnectionString:BookStore"]);
                SqlCommand cmd = new SqlCommand("AddAddress", this.sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@FullAddress", add.FullAddress);
                cmd.Parameters.AddWithValue("@City", add.City);
                cmd.Parameters.AddWithValue("@State", add.State);
                cmd.Parameters.AddWithValue("@TypeId", add.TypeId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                this.sqlConnection.Open();
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                this.sqlConnection.Close();
                if (i == 2)
                {
                    return "Enter Valid TypeId For Adding Address";
                }
                else
                {
                    return " Address Added Successfully";
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

        public AddressModel UpdateAddress(AddressModel add, int addressId, int userId)
        {
            try
            {
                this.sqlConnection = new SqlConnection(this.Configuration["ConnectionString:BookStore"]);
                SqlCommand cmd = new SqlCommand("UpdateAddress", this.sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@AddressId", addressId);
                cmd.Parameters.AddWithValue("@FullAddress", add.FullAddress);
                cmd.Parameters.AddWithValue("@City", add.City);
                cmd.Parameters.AddWithValue("@State", add.State);
                cmd.Parameters.AddWithValue("@TypeId", add.TypeId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                this.sqlConnection.Open();
                int i = cmd.ExecuteNonQuery();
                this.sqlConnection.Close();
                if (i >= 1)
                {
                    return add;
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
