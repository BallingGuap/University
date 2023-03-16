
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace University.Models
{
   
       

       

        public class Student 
        {
            [Key]
            public int StudentId { get; set; }
            public Account Account { get; set; }
            public virtual ICollection<Lecturer> Lecturers { get; set; }
            public virtual ICollection<Grade> Grades { get; set; }
            public virtual ICollection<Subject> Subjects { get; set; }
            public Student()
            {
               Lecturers = new List<Lecturer>();
               Grades = new List<Grade>();
               Subjects = new List<Subject>();
            }
        }

        public class Lecturer
        {
            [Key]
            public int LecturerId { get; set; }
            public Account Account { get; set; }
  
            public virtual ICollection<Student> Students { get; set; }
            public virtual ICollection<Subject> Subjects { get; set; }
            public Lecturer()
            {
            Students = new List<Student>();
            Subjects = new List<Subject>();
            }

        }

   

}