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
    public class FeedbackRL : IFeedbackRL
    {
        private SqlConnection sqlConnection;

        public FeedbackRL(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        private IConfiguration Configuration { get; }

        public FeedbackModel AddFeedback(FeedbackModel feedback, int userId)
        {
            try
            {
                this.sqlConnection = new SqlConnection(this.Configuration["ConnectionString:BookStore"]);
                SqlCommand cmd = new SqlCommand("AddFeedback", this.sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //cmd.Parameters.AddWithValue("@Reviews", feedback.Rewiews);
                cmd.Parameters.AddWithValue("@Comment", feedback.Comment);
                cmd.Parameters.AddWithValue("@Rating", feedback.Rating);
                cmd.Parameters.AddWithValue("@TotalRating", feedback.TotalRating);
                cmd.Parameters.AddWithValue("@BookId", feedback.BookId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                this.sqlConnection.Open();
                cmd.ExecuteNonQuery();
                this.sqlConnection.Close();
                return feedback;
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
        public string UpdateFeedback(FeedbackModel feedback, int userId)
        {
            try
            {
                this.sqlConnection = new SqlConnection(this.Configuration["ConnectionString:BookStore"]);
                SqlCommand cmd = new SqlCommand("UpdateFeedback", this.sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Comment", feedback.Comment);
                cmd.Parameters.AddWithValue("@Rating", feedback.Rating);
                cmd.Parameters.AddWithValue("@BookId", feedback.BookId);
                this.sqlConnection.Open();
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                this.sqlConnection.Close();
                if (i == 2)
                {
                    return "Enter Valid Book Id";
                }

                if (i == 1)
                {
                    return "Feedback Already Added for this book";
                }
                else
                {
                    return "Feedback Updated For this Book Successfully";
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
        public bool DeleteFeedback(int feedbackId, int userId)
        {
            try
            {
                this.sqlConnection = new SqlConnection(this.Configuration["ConnectionString:BookStore"]);
                SqlCommand cmd = new SqlCommand("DeleteFeedback", this.sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@FeedbackId", feedbackId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                this.sqlConnection.Open();
                int i = cmd.ExecuteNonQuery();
                this.sqlConnection.Close();
                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
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
        public List<GetFeedBackModel> GetRecordsByBookId(int bookId)
        {
            try
            {
                this.sqlConnection = new SqlConnection(this.Configuration["ConnectionString:BookStore"]);
                SqlCommand cmd = new SqlCommand("GetAllFeedback", this.sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@BookId", bookId);
                this.sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    List<GetFeedBackModel> feedbackModel = new List<GetFeedBackModel>();
                    while (reader.Read())
                    {
                        GetFeedBackModel feedback = new GetFeedBackModel();
                        UserModel user = new UserModel
                        {
                            Fullname = reader["FullName"].ToString()
                        };

                        feedback.FeedbackId = Convert.ToInt32(reader["FeedbackId"]);
                        feedback.Comment = reader["Comment"].ToString();
                        feedback.Rating = Convert.ToInt32(reader["Rating"]);
                        feedback.BookId = Convert.ToInt32(reader["BookId"]);
                        feedbackModel.Add(feedback);
                    }

                    this.sqlConnection.Close();
                    return feedbackModel;
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
