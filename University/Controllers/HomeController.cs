using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;
using University.Models;
using System.Text.Json;
using static University.Models.Utils;
using System.IO;

namespace University.Controllers
{
    public class HomeController : Controller
    {
     
        private FileLoader _fileLoader ;
        private const string cookieName = "LoginSingelton";
        private CookieHelper _cookieHelper;
        public HomeController()
        {
            _cookieHelper = CookieHelper.TryToCreateCH(cookieName);
            _fileLoader = DIfileLoader;
        }

        //https://filesamples.com/samples/document/txt/sample3.txt
        public ActionResult Index()
        {
            var text = Encoding.Default.GetString(_fileLoader.DownloadData());
            if (_cookieHelper != null && !CookieHelper.IsCookieValueEmpty(_cookieHelper.Cookie))
            {
               var loginSingl = JsonSerializer.Deserialize<(Account, LoginType)>(_cookieHelper.Cookie, TupleOption);
               setLogin(loginSingl.Item1, loginSingl.Item2);
            }
            return View(text as object);
        }


 


    }
}