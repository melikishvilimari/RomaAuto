using RomaAuto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RomaAuto.Helpers
{
    public class LoginHelper
    {
        public static void LogOff()
        {
            HttpContext.Current.Session["user"] = null;
        }

        public static MainUser CurrentUser()
        {
            return (MainUser)HttpContext.Current.Session["user"];
        }

        public static bool IsLoggedIn()
        {
            return (MainUser)HttpContext.Current.Session["user"] != null;
        }

        public static void CreateUser(MainUser user)
        {
            HttpContext.Current.Session["user"] = user;
        }
    }
}