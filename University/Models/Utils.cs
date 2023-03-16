using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace University.Models
{

    static public class Utils
    {

        public static bool isAdmin(Account acc)
        {
            return (acc.Email == "Admin" && acc.Password == "Admin");
        }

        public static Lecturer lecturerFirst(IEnumerable<Lecturer> lecturers, int lecturerId)
        {
            return lecturers.First(l => l.LecturerId == lecturerId);
        }

        public static void setLogin(Account accountLogin, LoginType accountType)
        {
            LoginSingelton.SetLogin(accountLogin);
            LoginSingelton.Type = accountType;
        }

        public static bool isDayOfTheWeek(string word)
            => word == "Monday" ||
            word == "Tuesday" ||
            word == "Wensday" ||
            word == "Thursday" ||
            word == "Friday" ||
            word == "Saturday" ||
            word == "Sunday";



        public class StudentRequest
        {
            public int studentRequestId { get; set; }
            public int studentId { get; set; }
            public int subjectId { get; set; }
        }

        public class StudentsAndSubjects
        {
            public int id { get; set; }
            public Subject subject { get; set;}
            public Student student { get; set;}
        }


        public static bool isFreeTime(Subject subject, IEnumerable<Subject> subjects)
        {

            foreach (var s in subjects)
            {

                if ((subject.DayOfTheWeek == s.DayOfTheWeek) && ((subject.From > s.From && subject.From < s.To) ||
                    (subject.To > s.From && subject.To < s.To) ||
                    (subject.From <= s.From && subject.To >= s.To)))
                {
                    return false;
                }
            }
            return true;
        }

        public abstract class FileLoader
        {
            public FileLoader(string address) =>
                p_address = address;

            public virtual string p_address { get; private set; }
            public abstract byte[] Data { get; protected set; }
            public abstract byte[] DownloadData();

        }

        public class HttpFileLoader
            : FileLoader
        {
            public HttpFileLoader(string address) 
                : base(address)
            {
                Data = null;
            }

            private byte[] _data { get; set; }
            public override byte[] Data { get => _data; protected set => _data = value; } 

            public override byte[] DownloadData()
            {
                using (WebClient client = new WebClient())
                {
                    Data =  client.DownloadData(p_address);
                }
                return Data;
            }

        }


        public class XmlHelper
        {
            public  string _path { get; private set; }
            protected virtual XmlDocument _xmlDoc { get; set; }
            protected virtual XmlNodeList _xmlRoot { get; set; }
            public string Tag { get => _xmlRoot.Item(0).InnerText; set  => _xmlRoot.Item(0).InnerText = value;}
            public XmlHelper(string path, string tag)
            {
           
                _path = path;
                _xmlDoc = DocumentInit();
                _xmlRoot = GetRoot(tag);
            }

           

            private XmlDocument DocumentInit()
            {
                var doc = new XmlDocument();
                doc.Load(_path);
                return doc;
            }
            private XmlNodeList GetRoot(string tag) =>
                _xmlDoc.GetElementsByTagName(tag);

            ~XmlHelper()
            {
                _xmlDoc.Save(_path);
            }
            

        }

        public class CookieHelper
        {
            public string CookieName { get; private set; }
            private  HttpResponse _response { get; set; }
            private HttpRequest _request { get;  set; }

            public CookieHelper(string cookieName)
            {
                _response = HttpContext.Current.Response;
                _request = HttpContext.Current.Request;
                CookieName = cookieName;
                if (!isExists(cookieName))
                {
                    throw new NullReferenceException("Cookie does not exist");
                }
            }

            public CookieHelper( string cookieName, DateTime cookieExpires)
            {
                _response = HttpContext.Current.Response;
                _request = HttpContext.Current.Request;
                CookieName = cookieName;
                if (!isExists(cookieName))
                {
                    initCookie(cookieName, cookieExpires);
                }
            }



            private void initCookie(string cookieName, DateTime cookieExpires)
            {
                var cookie = new HttpCookie(cookieName);
                cookie.Expires = cookieExpires;
                _response.Cookies.Add(cookie);
            }

            private bool isExists(string cookieName)
             => (_request.Cookies[cookieName] != null );
            


            public string Cookie { get => _request.Cookies[CookieName].Value; set => _response.Cookies[CookieName].Value =  value; }
            public static CookieHelper TryToCreateCH(string cookieName)
            {
                try
                {
                    return new CookieHelper(cookieName);
                }
                catch
                {
                    return null;
                }
            }


            public static bool IsCookieValueEmpty(string cookieValue)
            {
                return cookieValue == ""
                    || cookieValue == "{}"
                    || string.IsNullOrWhiteSpace(cookieValue);
            }









        }

    
        public static FileLoader DIfileLoader { get; set; }
        public static XmlHelper Helper { get; set; }

        private static  JsonSerializerOptions _tupleOption = new JsonSerializerOptions
        {
            IncludeFields = true,
        };

        public static  JsonSerializerOptions TupleOption { get => _tupleOption; }


    }
}