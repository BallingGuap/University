using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace University.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public string Subject { get; set; }
        public int GradeNumber { get; set; }
        public DateTime GradeTime { get; set; }
        public int LecturerId { get; set; }
    }
}