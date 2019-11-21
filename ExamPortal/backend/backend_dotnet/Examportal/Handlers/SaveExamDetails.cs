using Examportal.Auth;
using Examportal.Custom_Models;
using Examportal.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examportal.Handlers
{
    public class SaveExamDetails
    {
        ExamportalContext db = new ExamportalContext();
        public string FetchEmail(HttpContext httpContext)
        {
            Dictionary<string, string> email = new Dictionary<string, string>();

            Authentication auth = new Authentication();
            email = auth.getAllClaims(httpContext);
            return email["Email"];
        }
        public bool SaveExam(ExamDetails exam, HttpContext http)
        {

            var data = db.ExamDetails.FirstOrDefault(e => e.ExamCode == exam.ExamCode);
            if (data == null)
            {
                exam.CreatedDate = DateTime.Now;
                exam.Email = FetchEmail(http);
                exam.CreatedBy = db.Users.FirstOrDefault(e => e.Email == exam.Email).Name;
                db.ExamDetails.Add(exam);
                db.SaveChanges();
                return true;
            }
            return false;
        }
        public List<ExamCustomModel> ViewExamDetails(HttpContext httpContext)
        {
            var data = db.ExamDetails.Where(e => e.Email == FetchEmail(httpContext));
                
            if (data != null )
            {
                var obj = data.Select(a => new ExamCustomModel
                {
                    _id = a.Id,
                    examName = a.ExamName,
                    examCode = a.ExamCode,
                    examDuration = a.ExamDuration,
                    examStartTime = a.ExamStartTime
                }).ToList();
                return obj;
            }
                return null;
        }
        public ExamCustomModel ViewExamDeatilForUpdate(int id)
        {
            var data = db.ExamDetails.Where(e => e.Id == id);
            if( data != null)
            {
                var obj = data.Select(a => new ExamCustomModel
                {
                    _id = a.Id,
                    examName = a.ExamName,
                    examCode = a.ExamCode,
                    examDuration = a.ExamDuration,
                    examStartTime = a.ExamStartTime
                }).FirstOrDefault();
                return obj;
            }
            return null;
        }
        public bool EditExamDetails(int id, ExamDetails val,HttpContext httpContext)
        {
            
            var data = db.ExamDetails.FirstOrDefault(e => e.Id == id);

            if (data != null)
            {
                data.ExamName = val.ExamName;
                data.ExamDuration = val.ExamDuration;
                data.ExamStartTime = val.ExamStartTime;
                
                data.ModifiedDate = DateTime.Now;
                db.ExamDetails.Where(s=> s.Id == id).ToList().ForEach(e=> {
                    e.ExamName = val.ExamName;
                    e.ExamDuration = val.ExamDuration;
                    e.ExamStartTime = val.ExamStartTime;
                    e.ModifiedBy = db.Users.FirstOrDefault(a => a.Email == FetchEmail(httpContext)).Name;
                });
                db.SaveChanges();
                return true;
            }
            return false;         
        }
        public bool RemoveExamDetails( int id)
        {
            String examCode = db.ExamDetails.FirstOrDefault(e => e.Id == id).ExamCode;

            if (examCode != null)
            {
                var details = db.Questions.Where(s => s.ExamCode == examCode).ToList();

                foreach (var detail in details)
                {
                    db.Questions.Remove(detail);
                }
                db.ExamDetails.Remove(db.ExamDetails.FirstOrDefault(e => e.Id == id));
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
