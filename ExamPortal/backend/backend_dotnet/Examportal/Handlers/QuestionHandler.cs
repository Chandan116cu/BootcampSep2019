using Examportal.Custom_Models;
using Examportal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Examportal.Handlers
{
    public class QuestionHandler
    {
        ExamportalContext db = new ExamportalContext();

        public string radioOrCheckBoxValue(dynamic value)
        {
            string joinValue;
            int len = value.CheckedOption.Length;
            if (len == 1)
            {
                joinValue = string.Join("", value.CheckedOption);
            }
            else
            {
                joinValue = string.Join(" ", value.CheckedOption);
            }

            return joinValue;
        }

        public void SaveCorrectOption(dynamic checkAnswer, dynamic existingAnswer, string email, QuestionCustomModel value, string checkedOption)
        {
            CandidateAnswer answerDetails;
            CandidateResult resultDetails;
            Boolean status;
            if (existingAnswer == null)
            {
                answerDetails = AnswerDetailsObject(email,value.Code, value.QId,1,checkedOption);
                resultDetails = ResultDetailsObject(checkAnswer.Weightage,email, value.Code, 0);
                db.CandidateAnswer.Add(answerDetails);
                db.CandidateResult.Add(resultDetails);
                db.SaveChanges();
            }
            else
            {
                int updatedScore = existingAnswer.TotalScore + checkAnswer.Weightage;
                status = CheckExisitingRightOption(checkedOption, value.QId, value.Code, email, updatedScore);
                if (!status)
                {
                    answerDetails = AnswerDetailsObject(email, value.Code, value.QId, 1, checkedOption);
                    db.CandidateAnswer.Add(answerDetails);
                    db.CandidateResult.Where(s => s.Email == email && s.TestCode == value.Code).ToList().ForEach(x => x.TotalScore = updatedScore);
                    db.SaveChanges();
                }
            }
        }

        public Boolean CheckExisitingRightOption(string checkedOption, string QId, string examCode, string email, int updatedScore)
        {
            var status = db.CandidateAnswer.Where(s => s.Email == email && s.Id == Convert.ToInt16(QId) && s.TestCode == examCode).ToList();
            if(status.Count != 0)
            {
                if (status[0].CorrectStatus == 0)
                {
                    db.CandidateAnswer.Where(s => s.Email == email && s.TestCode == examCode && s.Id == Convert.ToInt16(QId)).ToList().ForEach(s => { s.CorrectStatus = 1; s.Answer = checkedOption; });
                    db.CandidateResult.Where(s => s.Email == email && s.TestCode == examCode).ToList().ForEach(s => s.TotalScore = updatedScore);
                    db.SaveChanges();
                }
                return true;
            }
            return false;
        }

        public Boolean CheckExistingWrongOption(string checkedOption, string QId, string examCode, string email, int updatedScore)
        {
            var status = db.CandidateAnswer.Where(s => s.Email == email && s.Id == Convert.ToInt16(QId) && s.TestCode == examCode).ToList();
            if(status.Count != 0)
            {
                if(status[0].CorrectStatus == 0)
                {
                    db.CandidateAnswer.Where(s => s.Email == email && s.TestCode == examCode && s.Id == Convert.ToInt16(QId)).ToList().ForEach(s => s.Answer = checkedOption);
                    db.SaveChanges();
                }
                else
                {
                    db.CandidateAnswer.Where(s => s.Email == email && s.TestCode == examCode && s.Id == Convert.ToInt16(QId)).ToList().ForEach(toUpdate => { toUpdate.CorrectStatus = 0; toUpdate.Answer = checkedOption; });
                    db.SaveChanges();
                    db.CandidateResult.Where(s => s.Email == email && s.TestCode == examCode).ToList().ForEach(data => data.TotalScore = updatedScore);
                    db.SaveChanges();

                }
                return true;
            }
            return false;
        }

        public void SaveIncorrectOption(dynamic checkAnswer, dynamic existingAnswer, string email, QuestionCustomModel value, string checkedOption)
        {
            CandidateAnswer answerDetails;
            CandidateResult resultDetails;
            Boolean status;
            if (existingAnswer == null)
            {
                answerDetails = AnswerDetailsObject(email, value.Code, value.QId, 0, checkedOption);
                resultDetails = ResultDetailsObject(0, email, value.Code, 0);
                db.CandidateAnswer.Add(answerDetails);
                db.CandidateResult.Add(resultDetails);
                db.SaveChanges();
            }
            else
            {
                int updatedScore = existingAnswer.TotalScore - checkAnswer.Weightage;
                status = CheckExistingWrongOption(checkedOption, value.QId, value.Code, email, updatedScore);
                if (!status)
                {
                    answerDetails = AnswerDetailsObject(email, value.Code, value.QId, 0, checkedOption);
                    db.CandidateAnswer.Add(answerDetails);
                    db.SaveChanges();
                }
            }

        }

        public CandidateAnswer AnswerDetailsObject(string email, string examCode, string QId, byte correctStatus, string checkedOption)
        {
            var answerDetails = new CandidateAnswer()
            {
                Email = email,
                CompletionTime = DateTime.Now.ToString(),
                CorrectStatus = correctStatus,
                Id = Convert.ToInt16(QId),
                CreatedDate = DateTime.Now,
                TestCode = examCode,
                Answer = checkedOption
            };
            return answerDetails;
        }

        public CandidateResult ResultDetailsObject(int score, string email, String examCode, byte submitStatus)
        {
            var resultDetails = new CandidateResult()
            {
                Email = email,
                TotalScore = score,
                SubmitExam = submitStatus,
                TestCode = examCode

            };
            return resultDetails;
        }

        public void submitAllQuestions(SubmitAnswerCustomModel value, Dictionary<string, string> email)
        {
            var allQuestions = db.Questions.Where(s => s.ExamCode == value.code).ToList();
            var savedQuestions = db.CandidateAnswer.Where(s => s.TestCode == value.code && s.Email == email["Email"]).ToList();
           
            int i;
            if(savedQuestions.Count != 0)
            {
                for (i = 0; i < allQuestions.Count; i++)
                {
                    var status = db.CandidateAnswer.Where(s => s.Id == allQuestions[i].Id).FirstOrDefault();
                    if (status == null)
                    {
                        String id = allQuestions[i].Id.ToString();

                        CandidateAnswer answerDetails = AnswerDetailsObject(email["Email"], value.code, id, 0, null);
                        db.CandidateAnswer.Add(answerDetails);
                        db.SaveChanges();
                    }
                }
                db.CandidateResult.Where(s => s.Email == email["Email"] && s.TestCode == value.code).ToList().ForEach(x => x.SubmitExam = 1);
                db.SaveChanges();
                //if (allQuestions.Count == savedQuestions.Count)
                //{
                //    db.CandidateResult.Where(s => s.Email == email["Email"] && s.TestCode == value.code).ToList().ForEach(x=> x.SubmitExam = 1);
                //    db.SaveChanges();
                //    return;
                //}
            }
            else
            {
                CandidateResult resultDetails = ResultDetailsObject(0, email["Email"], value.code, 0);
                db.CandidateResult.Add(resultDetails);
                db.SaveChanges();
                for (i = 0; i < allQuestions.Count; i++)
                {
                    String id = allQuestions[i].Id.ToString();
                    CandidateAnswer answerDetails = AnswerDetailsObject(email["Email"], value.code, id, 0, null);
                    db.CandidateResult.Where(s => s.Email == email["Email"] && s.TestCode == value.code).ToList().ForEach(x => x.SubmitExam = 1);
                    db.CandidateAnswer.Add(answerDetails);
                    db.SaveChanges();
                }
            }
        }

        public ExamDetails CheckAccessKey(ExamDetails value)
        {
            var existingExam = db.ExamDetails.FirstOrDefault(s => s.ExamCode == value.ExamCode);
            return existingExam;
        }
    }
}
