using Examportal.Custom_Models;
using Examportal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examportal.Handlers
{
    public class QuestionOperation
    {
        ExamportalContext db = new ExamportalContext();
        SaveExamDetails exam = new SaveExamDetails();
        
        public bool RemoveQuestion(int id)
        {
            var data = db.Questions.FirstOrDefault(d => d.Id == id);
            if (data != null)
            {
                db.Questions.Remove(db.Questions.FirstOrDefault(e => e.Id == id));
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }
        public List<ViewCustomModel> ViewQuestion(string examCode)
        {
            var data = db.Questions.Where(e => e.ExamCode == examCode); 
            if( data != null )
            {
                var obj = data.Select(a => new ViewCustomModel
                {
                    _id = a.Id,
                    questionText = a.QuestionText,
                    option1 = a.Option1,
                    option2 = a.Option2,
                    option3 = a.Option3,
                    option4 = a.Option3,
                    weightage = a.Weightage,
                    answer = a.Answer,
                    questionImage = a.QuestionImage
                }).ToList();
                return obj;
            }
            else
            {
                return null;
            }
        }
        public ViewCustomModel EditViewQuestion(int id )
        {
            var data = db.Questions.FirstOrDefault(e => e.Id == id);
          
            if (data != null)
            {
                var obj =new ViewCustomModel
                 {
                     _id = data.Id,
                     questionText = data.QuestionText,
                     option1 = data.Option1,
                     option2 = data.Option2,
                     option3 = data.Option3,
                     option4 = data.Option3,
                     weightage = data.Weightage,
                     answer = data.Answer,
                     answerType = data.AnswerType,
                     questionImage = data.QuestionImage
                 };
                return obj;
            }
            else
                return null;
        }
        //public bool UploadQuestion(HttpContext q )
        //{
        //    var req = 
        //}
    }
}
