using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examportal.Custom_Models
{
    public class ExamCustomModel
    {
        public int _id { get; set; }
        public string examName { get; set; }
        public string examCode { get; set; }
        public string examDuration { get; set; }
        public string examStartTime { get; set; }
    }
}
