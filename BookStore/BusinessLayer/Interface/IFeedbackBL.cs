using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
   public interface IFeedbackBL
   {
        public FeedbackModel AddFeedback(FeedbackModel feedback, int userId);
        public string UpdateFeedback(FeedbackModel feedback, int userId);
        public bool DeleteFeedback(int feedbackId, int userId);
        public List<GetFeedBackModel> GetRecordsByBookId(int bookId);
    }
}
