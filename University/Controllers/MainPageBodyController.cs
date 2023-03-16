using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace University.Controllers
{
    public class MainPageBodyController : Controller
    {
        // GET: MainPageBody
        private string _text = null;
        MainPageBodyController(string text)
        {
            _text = text;
        }

        public ActionResult MainPageBodyPartial()
        {
            return PartialView(_text);
        }
    }
}