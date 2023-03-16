using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.Models;

namespace University.Controllers
{
    public class AdminPanelController : Controller
    {
        
        public ActionResult MainPageAddressEdit()
        {
            if (LoginSingelton.Type != LoginType.admin_login)
            {
                return HttpNotFound("Not Admin Login");
            }
            return View();
        }

        [HttpPut]
        public ActionResult MainPageAddressEdit(string Address)
        {
            Utils.Helper.Tag = Address;
            Utils.DIfileLoader = new Utils.HttpFileLoader(Utils.Helper.Tag);
            return Redirect("../.");
        }

    }
}