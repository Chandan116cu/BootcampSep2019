using System;
using System.Collections.Generic;

namespace Examportal.Models
{
    public partial class Questions
    {
        public Questions()
        {
            CandidateAnswer = new HashSet<CandidateAnswer>();
        }

        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string Answer { get; set; }
        public string AnswerType { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public int? Weightage { get; set; }
        public string ExamCode { get; set; }
        public string QuestionImage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ExamDetails ExamCodeNavigation { get; set; }
        public ICollection<CandidateAnswer> CandidateAnswer { get; set; }
    }
}
