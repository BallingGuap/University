using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using University.Models;
using System.Data.Entity.Migrations;
using System.IO;

namespace University.Controllers
{
    public class StudentAdminController : Controller
    {
        private UniversityContext db = new UniversityContext();

        // GET: Student
        public async Task<ActionResult> Index()
        {
            if (LoginSingelton.Type != LoginType.admin_login)
            {
                return HttpNotFound("Not Admin Login");
            }
            return View(await db.Students.ToListAsync());
        }

        // GET: Student/Details/5
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
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            if (LoginSingelton.Type != LoginType.admin_login)
            {
                return HttpNotFound("Not Admin Login");
            }
            ViewBag.Lecturers = db.Lecturers;
            return View();
        }

        // POST: Student/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "StudentId,Account")] Student student, int[] selectedLecturers)
        {
            student.StudentId = await db.Students.MaxAsync(s => s.StudentId);
            student.Account.AccountId =
                await db.Lecturers.MaxAsync(l => l.Account.AccountId) > await db.Students.MaxAsync(s => s.Account.AccountId)
                                             ? await db.Lecturers.MaxAsync(l => l.Account.AccountId)
                                             : await db.Students.MaxAsync(s => s.Account.AccountId);
            student.Account.CreatedDate = DateTime.Now;

            foreach (var lec in db.Lecturers)
            {
                if (selectedLecturers.Contains(lec.LecturerId))
                {
                    student.Lecturers.Add(lec);
                }
            }
                db.Students.Add(student);
                 
                await db.SaveChangesAsync();
                return RedirectToAction("Index");

        }

        // GET: Student/Edit/5
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
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.Lecturers = db.Lecturers;
            return View(student);
        }

        //var str = "";
        //foreach(var lec in student.Lecturers)
        //{
        //    str += lec.Account.FirstName;
        //    str += "\n";
        //}
        //return str;
        // POST: Student/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        public /*async /*Task<ActionResult>*/ ActionResult Edit([Bind(Include = "StudentId,Account")] Student student, int[] selectedLecturers)
        {

             student.Lecturers.Clear();

            if (selectedLecturers != null)
            {
                foreach (var lec in db.Lecturers)
                {
                    if (selectedLecturers.Contains(lec.LecturerId))
                    {
                        student.Lecturers.Add(lec);
                    }
                }
            }


            //db.Entry(student).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");


        }

        // GET: Student/Delete/5
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
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Student student = await db.Students.FindAsync(id);
            db.Students.Remove(student);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public ActionResult Document(int studentId)
        {
            if (LoginSingelton.Type != LoginType.admin_login)
            {
                return HttpNotFound("Not Lecturer Login");
            }
            var student = db.Students.First(s => s.StudentId == studentId);
            var text = $"{student.Account.FirstName} became student in our university in {DateTime.Now}";
            var fileName = $"{student.Account.FirstName}_{student.StudentId}";
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
