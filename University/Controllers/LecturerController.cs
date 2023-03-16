using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using University.Models;
using Microsoft.Ajax.Utilities;
using System.Web.UI;

namespace University.Controllers
{
    public class LecturerController : Controller
    {
        private UniversityContext db = new UniversityContext();
        private Lecturer _lecturer = null;
        public LecturerController()
        {
            if (LoginSingelton.Type == LoginType.lecturer_login)
            {
                _lecturer = db.Lecturers.First(l => l.Account.AccountId == LoginSingelton.Login.AccountId);
            }
        }
        public ActionResult Students(int? page)
        {

            if (LoginSingelton.Type != LoginType.lecturer_login)
            {
                return HttpNotFound("Not Lecturer Login");
            }
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            return View(_lecturer.Students.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult StudentGrades(int studentId, int? page)
        {

            if (LoginSingelton.Type != LoginType.lecturer_login)
            {
                return HttpNotFound("Not Lecturer Login");
            }
            var student = _lecturer.Students.First(s => s.StudentId == studentId);
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            ViewBag.Student = student;
            ViewBag.Lecturers = db.Lecturers;
            return View(student.Grades.
                   Where(g => g.LecturerId == _lecturer.LecturerId ).
                   ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddGrade(int studentId)
        {

            if (LoginSingelton.Type != LoginType.lecturer_login)
            {
                return HttpNotFound("Not Lecturer Login");
            }
            var student = _lecturer.Students.First(s => s.StudentId == studentId);
            return View(student);
        }

        [HttpPost]
        public ActionResult AddGrade(int studentId, [Bind(Include = "Subject,GradeNumber")] Grade grade)
        {
            if(_lecturer.Subjects.FirstOrDefault(s => s.SubjectName == grade.Subject) == null)
            {
                return HttpNotFound($"You do not have competence to rate in {grade.Subject} ");
            }


            var student = _lecturer.Students.First(s => s.StudentId == studentId);

            if (student.Subjects.FirstOrDefault(s => s.SubjectName == grade.Subject) == null)
            {
                return HttpNotFound($"Student do not have {grade.Subject} ");
            }

            student.Grades.Add(new Grade
            { 
                GradeId = db.Students.Max(s => s.Grades.Max(g => g.GradeId)) + 1,
                GradeNumber = grade.GradeNumber,
                Subject = grade.Subject,
                GradeTime = DateTime.Now,
                LecturerId = _lecturer.LecturerId
            });
            db.SaveChanges();
            return Redirect($@"./StudentGrades?studentId={studentId}"); ;
        }


        public ActionResult Subjects(int? page)
        {
            if (LoginSingelton.Type != LoginType.lecturer_login)
            {
                return HttpNotFound("Not Lecturer Login");
            }
            int pageSize = 1;
            int pageNumber = (page ?? 1);

            return View(_lecturer.Subjects.
                   ToPagedList(pageNumber, pageSize));
        }



    }
}