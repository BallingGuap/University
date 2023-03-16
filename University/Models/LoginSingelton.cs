using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace University.Models
{
    public enum LoginType
    {
        unregistered_login = 0,
        admin_login = 2,
        student_login = 4,
        lecturer_login = 8
    }

    public static class LoginSingelton
    {
        private static LoginType _type { get; set; }
        public static LoginType Type { get => _type; set => _type = value; }
        private static Account _login { get; set; }
        public static  Account Login { get => _login ?? throw new NullReferenceException(); 
                                      private set => _login = value; }
        static LoginSingelton() => _login = null;
        public static void SetLogin(Account login) =>
            Login = login;
    }
}