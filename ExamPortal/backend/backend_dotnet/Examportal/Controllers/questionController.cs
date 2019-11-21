using System;
using Examportal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Examportal.Controllers;
using Examportal.Custom_Models;
using Examportal.Auth;
using Examportal.Handlers;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Examportal.Controllers
{
    [ApiController]
    public class QuestionController : ControllerBase
    {
        ExamportalContext db = new ExamportalContext();

        [Authorize]
        [Route("/question")]
        [HttpGet]
        public IActionResult TestQuestions()
        {
            Boolean lastQuesStatus;
            string examcode = HttpContext.Request.Headers["examCode"].ToString();
            int pageNum = Int32.Parse(HttpContext.Request.Query["pageNumber"]);
            var ques = db.Questions.Where(s => s.ExamCode == examcode).Skip(pageNum).Take(1).Select(a=> new {a.AnswerType,a.Option1,a.Option2,a.Option3,a.Option4,_id = a.Id,a.QuestionImage,a.QuestionText }).ToList();

            var lastQuestion = db.Questions.Where(s => s.ExamCode == examcode).Select(a=> new { _id = a.Id,   QuestionText = a.QuestionText}).ToList();

            if (lastQuestion[lastQuestion.Count()-1].QuestionText == ques[0].QuestionText) lastQuesStatus = true;
            else lastQuesStatus = false;
            var time = db.ExamDetails.Where(s => s.ExamCode == examcode).ToList();
            
            return Ok(new
            {
                allQuestions = lastQuestion,
                lastQuestionStatus = lastQuesStatus,
                startTime = time[0].ExamStartTime,
                duration = time[0].ExamDuration,
                examName = time[0].ExamName,
                questions = ques,
                pageNumber = pageNum
            });
        }

        [Authorize]
        [Route("/question")]
        [HttpPost]
        public IActionResult SaveCandidateAnswer([FromBody] QuestionCustomModel value)
        {
            Dictionary<string, string> email = new Dictionary<string, string>();

            QuestionHandler qh = new QuestionHandler();
            Authentication auth = new Authentication();
            email = auth.getAllClaims(HttpContext);

            string joinValue = qh.radioOrCheckBoxValue(value);
            var checkAnswer = db.Questions.Where(s => s.Id == Int32.Parse(value.QId)).Select(a => new { a.Answer, a.Weightage }).FirstOrDefault();
            var existingAnswer = db.CandidateResult.Where(s => s.Email == email["Email"] && s.TestCode == value.Code).FirstOrDefault();

            if (checkAnswer.Answer == joinValue)
            {
                qh.SaveCorrectOption(checkAnswer, existingAnswer, email["Email"], value, joinValue);
            }
            else
            {
                qh.SaveIncorrectOption(checkAnswer, existingAnswer, email["Email"], value, joinValue);
            }
            return Ok(new { msg="data saved"});
        }

        [Route("/myRoute")]
        [HttpGet]
        public IActionResult Get()
        {
            //var data = db.CandidateResult.Join(db.CandidateAnswer, a => a.Email, b => b.Email, (a, b) => new { data1 = a, data2 = b }).Where(c => CandidateAnswers.Email == "himanshuchauhan@gmail.com");
            //var data = db.CandidateResult.Join(db.CandidateAnswer, str1 => str1.Email, str2=> str2.Email,(str1,str2)=>str1);
            var data = db.CandidateResult.Include(answer => answer.CandidateAnswer).ToList();
            return Ok(data);
        }

    }
}

