using System;
using System.Collections.Generic;

namespace Examportal.Models
{
    public partial class ExamDetails
    {
        public ExamDetails()
        {
            Questions = new HashSet<Questions>();
        }

        public int Id { get; set; }
        public string ExamName { get; set; }
        public string ExamCode { get; set; }
        public string Email { get; set; }    
        public string ExamDuration { get; set; }
        public string ExamStartTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public Users EmailNavigation { get; set; }
        public ICollection<Questions> Questions { get; set; }
    }
}
