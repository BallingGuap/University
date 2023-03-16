using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace University.Models
{
    public class UniversityDbInitializer
         : DropCreateDatabaseAlways<UniversityContext>
    {

        protected override void Seed(UniversityContext context)
        {

            Lecturer lec1 = new Lecturer
            {
                LecturerId = 1,
                Account = new Account
                {
                    CreatedDate = DateTime.Now,
                    Email = "Petrovich@gmail.com",
                    FirstName = "Alexandr",
                    Gender = "male123", 
                    AccountId = 1,
                    LastName = "Petrovich",
                    Password = "123123123"
                },
            };

            lec1.Subjects.Add(new Subject { Credit = 5, DayOfTheWeek = "Monday", From = TimeSpan.Parse("16:20"), To = TimeSpan.Parse("17:30"), SubjectId = 1, SubjectName = "Math" });

            Lecturer lec2 = new Lecturer
            {
                LecturerId = 2,
                Account = new Account
                {
                    CreatedDate = DateTime.Now,
                    Email = "Egorovich@gmail.com",
                    FirstName = "Metin",
                    Gender = "male123",
                    AccountId = 2,
                    LastName = "Sidiropulo",
                    Password = "123asdf23"
                },
            };


            lec2.Subjects.Add(new Subject { Credit = 3, DayOfTheWeek = "Friday", From = TimeSpan.Parse("11:20"), To = TimeSpan.Parse("13:25"), SubjectId = 2, SubjectName = "History" });

            context.Lecturers.Add(lec2);
            context.Lecturers.Add(lec2);


            Student std1 = new Student
            {
                StudentId = 1,
                Account = new Account
                {
                    CreatedDate = DateTime.Now,
                    Email = "Mxim@gmail.com",
                    FirstName = "Maxim",
                    Gender = "male123",
                    AccountId = 3,
                    LastName = "Darunov",
                    Password = "123asdf23"
                },
                Lecturers = {lec1, lec2},
            };

            std1.Subjects.Add(new Subject { Credit = 5, DayOfTheWeek = "Monday", From = TimeSpan.Parse("16:20"), To = TimeSpan.Parse("17:30"), SubjectId = 1, SubjectName = "Math" });

            std1.Grades.Add(new Grade
            {
                GradeId = 1,
                LecturerId = lec1.LecturerId,
                Subject = "Math",
                GradeNumber = 2,
                GradeTime = DateTime.Now
            });


            std1.Grades.Add(new Grade
            {
                GradeId = 2,
                LecturerId = lec1.LecturerId,
                Subject = "Math",
                GradeNumber = 4,
                GradeTime = DateTime.Now
            }); 

            Student std2 = new Student
            {
                StudentId = 2,
                Account = new Account
                {
                    CreatedDate = DateTime.Now,
                    Email = "Alexanrd@gmail.com",
                    FirstName = "Alexandr",
                    Gender = "male123",
                    AccountId = 4,
                    LastName = "Gornov",
                    Password = "123asdf23"
                },
                Lecturers = {  lec2 }
            };

            std2.Subjects.Add(new Subject { Credit = 5, DayOfTheWeek = "Monday", From = TimeSpan.Parse("16:20"), To = TimeSpan.Parse("17:30"), SubjectId = 1, SubjectName = "Math" });
            std2.Subjects.Add(new Subject { Credit = 3, DayOfTheWeek = "Friday", From = TimeSpan.Parse("11:20"), To = TimeSpan.Parse("13:25"), SubjectId = 2, SubjectName = "History" });

            std2.Grades.Add(new Grade
            {
                GradeId = 3,
                LecturerId = lec1.LecturerId,
                Subject = "Math",
                GradeNumber = 5,
                GradeTime = DateTime.Now
            });

            std2.Grades.Add(new Grade
            {
                GradeId = 4,
                LecturerId = lec2.LecturerId,
                Subject = "History",
                GradeNumber = 10,
                GradeTime = DateTime.Now
            });


            context.Students.Add(std1);
            context.Students.Add(std2);

            base.Seed(context);

        }
    }
}