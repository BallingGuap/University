using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace University.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string DayOfTheWeek { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
        public float Credit { get; set; }
    }
}