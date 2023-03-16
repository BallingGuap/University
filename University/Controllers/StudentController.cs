using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Models;
using PagedList.Mvc;
using PagedList;
using System.Web.UI.WebControls;

namespace University.Controllers
{
    public class StudentController : Controller
    {
        private UniversityContext db = new UniversityContext();
        private Student _student = null;
        public StudentController()
        {
            if (LoginSingelton.Type == LoginType.student_login)
            {
                _student = db.Students.First(s => s.Account.AccountId == LoginSingelton.Login.AccountId);
            }
        }
        public ActionResult Grades(int? page)
        {

            if (LoginSingelton.Type != LoginType.student_login)
            {
                return HttpNotFound("Not Student Login");
            }
            ViewBag.Lecturers = db.Lecturers;
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            return View(_student.Grades.ToPagedList(pageNumber, pageSize));
        }


        //public ActionResult Subjects(int? page)
        //{
        //    if (LoginSingelton.Type != LoginType.student_login)
        //    {
        //        return HttpNotFound("Not Student Login");
        //    }
        //    int pageSize = 1;
        //    int pageNumber = (page ?? 1);

        //    return View(_student.Subjects.
        //           ToPagedList(pageNumber, pageSize));
        //}


        public ActionResult Subjects(int? page)
        {
            if (LoginSingelton.Type != LoginType.student_login)
            {
                return HttpNotFound("Not Student Login");
            }
            return View(_student.Subjects);
        }



        public ActionResult LecturersSubjects(int? page)
        {
            if (LoginSingelton.Type != LoginType.student_login)
            {
                return HttpNotFound("Not Student Login");
            }
            int pageSize = 1;
            int pageNumber = (page ?? 1);

            return View(db.Lecturers.SelectMany(l=> l.Subjects).OrderBy(s => s.SubjectId).
                   ToPagedList(pageNumber, pageSize)); ;
        }

        public ActionResult SignSubject(int subjectId) 
        {

            if (LoginSingelton.Type != LoginType.student_login)
            {
                return HttpNotFound("Not Student Login");
            }

            if (!Utils.isFreeTime(db.Lecturers.
                                     SelectMany(l => l.Subjects).
                                     First(s => s.SubjectId == subjectId)
                                     , _student.Subjects))
            {
                return HttpNotFound("This Time is taken for you");
            }

            var request = new Utils.StudentRequest 
            { 
              studentRequestId = db.Requests.Count() == 0 ? 
              1 : 
              db.Requests.Max(r => r.studentRequestId),
              studentId = _student.StudentId, 
              subjectId = subjectId 
            };

         


            db.Requests.Add(request);
            db.SaveChanges();

            return Redirect("../.");

        }

        public ActionResult  GPA()
        {
            var countSubjects = _student.Subjects.GroupBy(s => s.SubjectName).Count();
            var sumMulCredGrade = _student.Grades.Select(g=> g.GradeNumber * _student.Subjects.First(s => s.SubjectName == g.Subject).Credit).Sum();
            var GPA = (sumMulCredGrade / countSubjects);
            return View(GPA);
        }
    }
}