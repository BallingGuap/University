using University.Models;
using System.Web.Mvc;

namespace University.Controllers
{
    public class ExtensionController : Controller
    {
        // GET: Extension
        public ActionResult PersonalArea()
        {
            if(LoginSingelton.Type == LoginType.unregistered_login)
            {
                HttpNotFound("You are not registred yet");
            }

            return View();
        }
    }
}