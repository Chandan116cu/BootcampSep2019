using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examportal.Custom_Models
{
    public class ViewCustomModel
    {
        public int _id { get; set; }
        public string questionText {get; set;}
        public string option1 { get; set; }
        public string option2 { get; set; }
        public string option3 { get; set; }
        public string option4 { get; set; }
        public int? weightage { get; set; }
        public string answer { get; set; }
        public string questionImage { get; set; }
        public string answerType { get; set; }
    }
}
