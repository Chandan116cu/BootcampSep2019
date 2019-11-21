using System;
using System.Collections.Generic;

namespace Examportal.Models
{
    public partial class CandidateAnswer
    {
        public int CandidateId { get; set; }
        public string Email { get; set; }
        public string CompletionTime { get; set; }
        public byte? CorrectStatus { get; set; }
        public string TestCode { get; set; }
        public int? Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Answer { get; set; }

        public CandidateResult EmailNavigation { get; set; }
        public Questions IdNavigation { get; set; }
    }
}
