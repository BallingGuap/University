using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Models;
using static University.Models.Utils;
using System.Net;

namespace University.Controllers
{
    public class LoginController : Controller
    {
        private UniversityContext db = new UniversityContext();
        public ActionResult Login()
        {
            return View();
        }


       

        private void sendCookie(string cookieName, DateTime cookieExpires, string cookieValue)
        {
            var cookieHelper = new CookieHelper(cookieName, cookieExpires);
            cookieHelper.Cookie = cookieValue;
        }
            
 



        [HttpPost]
        public ActionResult Login([Bind(Include = "Email,Password")] Account acc)
        {

            const string cookieName = "LoginSingelton";
            DateTime cookieExpires = DateTime.Now.AddMinutes(2);

            if (Utils.isAdmin(acc))
            {

                setLogin(acc, LoginType.admin_login);
                sendCookie(cookieName, DateTime.Now.AddMinutes(2), 
                           JsonSerializer.Serialize((LoginSingelton.Login, LoginSingelton.Type), TupleOption));
                return Redirect("../.");
            }
            var student = db.Students.FirstOrDefault(s => s.Account.Email == acc.Email && s.Account.Password == acc.Password);
            if (student != default(Student))
            {
                var fullAcc = db.Students.First(s => s.Account.Email == acc.Email).Account;
                setLogin(fullAcc, LoginType.student_login);
                sendCookie(cookieName, DateTime.Now.AddMinutes(2),
                           JsonSerializer.Serialize((LoginSingelton.Login, LoginSingelton.Type), TupleOption));

                return Redirect("../.");
            }
            var lecturer = db.Lecturers.FirstOrDefault(s => s.Account.Email == acc.Email && s.Account.Password == acc.Password);
            if (lecturer != default(Lecturer))
            {
                var fullAcc = db.Lecturers.First(l => l.Account.Email == acc.Email).Account;
                setLogin(fullAcc, LoginType.lecturer_login);
                sendCookie(cookieName, DateTime.Now.AddMinutes(2),
                           JsonSerializer.Serialize((LoginSingelton.Login, LoginSingelton.Type), TupleOption));

                return Redirect("../.");
            }
            return HttpNotFound("Incorrect Email or Password");

        }


        public ActionResult Logout()
        {
            setLogin(new Account(), LoginType.unregistered_login);
            sendCookie("LoginSingelton", DateTime.Now.AddMinutes(2), "{}");
            return Redirect("../.");
        }


    }
}