using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using PagedList;
using System.Web.Mvc;
using University.Models;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.UI;

namespace University.Controllers
{
    public class LecturerAdminController : Controller
    {
        private UniversityContext db = new UniversityContext();

        // GET: Lecturer
        public async Task<ActionResult> Index()
        {
            if (LoginSingelton.Type != LoginType.admin_login)
            {
                return HttpNotFound("Not Admin Login");
            }

            return View(await db.Lecturers.ToListAsync());

        }

        // GET: Lecturer/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (LoginSingelton.Type != LoginType.admin_login)
            {
                return HttpNotFound("Not Admin Login");
            }

            if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Lecturer lecturer = await db.Lecturers.FindAsync(id);
                if (lecturer == null)
                {
                    return HttpNotFound();
                }
                return View(lecturer);
          
        }

        // GET: Lecturer/Create
        public ActionResult Create()
        {
            if (LoginSingelton.Type != LoginType.admin_login)
            {
                return HttpNotFound("Not Admin Login");
            }
            return View();
        }

        // POST: Lecturer/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Account")] Lecturer lecturer)
        {
          
                lecturer.LecturerId = await db.Lecturers.MaxAsync(l => l.LecturerId);
                lecturer.Account.AccountId = 
                await db.Lecturers.MaxAsync(l => l.Account.AccountId) > await db.Students.MaxAsync(s => s.Account.AccountId) 
                                             ? await db.Lecturers.MaxAsync(l => l.Account.AccountId)
                                             : await db.Students.MaxAsync(s => s.Account.AccountId);
                lecturer.Account.CreatedDate = DateTime.Now;
                db.Lecturers.Add(lecturer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
        }

        // GET: Lecturer/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (LoginSingelton.Type != LoginType.admin_login)
            {
                return HttpNotFound("Not Admin Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecturer lecturer = await db.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return HttpNotFound();
            }
            return View(lecturer);
        }

        // POST: Lecturer/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LecturerId,Account")] Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lecturer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(lecturer);
        }

        // GET: Lecturer/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (LoginSingelton.Type != LoginType.admin_login)
            {
                return HttpNotFound("Not Admin Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecturer lecturer = await db.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return HttpNotFound();
            }
            return View(lecturer);
        }

        // POST: Lecturer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Lecturer lecturer = await db.Lecturers.FindAsync(id);
            db.Lecturers.Remove(lecturer);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public ActionResult AddSubject(int lecturerId)
        {
            if (LoginSingelton.Type != LoginType.admin_login)
            {
                return HttpNotFound("Not Admin Login");
            }
            var lecturer = db.Lecturers.First(l => l.LecturerId == lecturerId);
            return View(lecturer);
        }



        [HttpPost]
        public ActionResult AddSubject(int lecturerId, [Bind(Include = "SubjectName, GradeNumber, Credit, DayOfTheWeek")] Subject subject, string From, string To) 
        {
            if (!Utils.isDayOfTheWeek(subject.DayOfTheWeek))
            {
                return HttpNotFound("Uncorrect Day of the Week");
            }

            var lecturer = db.Lecturers.First(l => l.LecturerId == lecturerId);
            Regex timeRegex = new Regex("^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$");
            MatchCollection matchedFrom = timeRegex.Matches(From);
            MatchCollection matchedTo= timeRegex.Matches(To);
            if(matchedFrom.Count == 0 
                ||  matchedTo.Count == 0)
            {
                return HttpNotFound("Uncorrect Time Format");
            }
            subject.From = TimeSpan.Parse(From);
            subject.To = TimeSpan.Parse(To);
            if (subject.From >= subject.To)
            {
                return HttpNotFound("To equal or bigger than From");
            }


            if (!Utils.isFreeTime(subject, lecturer.Subjects))
            {
                return HttpNotFound("This Time is taken for you");
            }


            //foreach (var s in lecturer.Subjects)
            //{

            //        if ((subject.DayOfTheWeek == s.DayOfTheWeek) && ((subject.From > s.From && subject.From < s.To) || 
            //            (subject.To > s.From && subject.To < s.To)     ||
            //            (subject.From <= s.From && subject.To >= s.To)))
            //        {
            //            //return HttpNotFound("subject.From >= s.From && subject.From <= s.To");
            //            return HttpNotFound("This Time is taken for you");
            //        }
            //        //else if(subject.To >= s.From && subject.To <= s.To)
            //        //{
            //        //    return HttpNotFound("subject.To >= s.From && subject.To <= s.To");
            //        //}
            //        //else if(subject.From < s.From && subject.To > s.To)
            //        //{
            //        //    return HttpNotFound("subject.From < s.From && subject.To > s.To");
            //        //}

            //}





            lecturer.Subjects.Add(new Subject
            {
                SubjectId = db.Lecturers.Max(l => l.Subjects.Max(s => s.SubjectId)) + 1,
                From = subject.From,
                Credit =  subject.Credit,
                DayOfTheWeek = subject.DayOfTheWeek,
                SubjectName = subject.SubjectName,
                To = subject.To
            });
            db.SaveChanges();
            return Redirect("../.");
        }

        public ActionResult StudentsRequests(int? page)
        {
            if (LoginSingelton.Type != LoginType.admin_login)
            {
                return HttpNotFound("Not Admin Login");
            }
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            var subjects = db.Lecturers.SelectMany(l => l.Subjects);
            var col = new List<Utils.StudentsAndSubjects>();
            foreach(var r in db.Requests)
            {
                var studentRequest = new Utils.StudentsAndSubjects
                {
                    id = r.studentRequestId,
                    student = db.Students.First(s => s.StudentId == r.studentId),
                    subject = db.Lecturers.SelectMany(l => l.Subjects).First(s => s.SubjectId == r.subjectId)
                };
                col.Add(studentRequest);
            }

            return View(col.
                        ToPagedList(pageNumber, pageSize));
        }





        public ActionResult Decline(int requestId)
        {
            if (LoginSingelton.Type != LoginType.admin_login)
            {
                return HttpNotFound("Not Admin Login");
            }
            var req = db.Requests.First(r => r.studentRequestId ==  requestId);
            db.Requests.Remove(req);
            db.SaveChanges();
            return RedirectToAction("StudentsRequests");
        }




        public ActionResult Confrim(int requestId, int studentId, int subjectId)
        {
            if (LoginSingelton.Type != LoginType.admin_login)
            {
                return HttpNotFound("Not Admin Login");
            }
            var req = db.Requests.First(r => r.studentRequestId == requestId);
            var student = db.Students.First(s => s.StudentId == studentId);
            var subject = db.Lecturers.SelectMany(l => l.Subjects).First(s => s.SubjectId == subjectId);
            student.Subjects.Add(subject);
            db.Requests.Remove(req);
            db.SaveChanges();
            return RedirectToAction("StudentsRequests");

        }



        public ActionResult Document(int lecturerId)
        {
            if (LoginSingelton.Type != LoginType.admin_login)
            {
                return HttpNotFound("Not Admin Login");
            }
            var lecturer = db.Lecturers.First(l => l.LecturerId == lecturerId);
            var text = $"{ lecturer.Account.FirstName } became lecturer in our university in { DateTime.Now}";
            var fileName = $"{lecturer.Account.FirstName}_{lecturer.LecturerId}";
            return DownloadData(text, fileName, "txt", "text");
        }

        private FileResult DownloadData(string data, string fName, string fExtension, string cType) 
        {
            string downloadFileName = $"{fName}.{fExtension}";
            string contentType = $"{cType}/{fExtension}";

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(data);
            writer.Flush();
            stream.Position = 0;

            return File(stream, contentType, downloadFileName);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
