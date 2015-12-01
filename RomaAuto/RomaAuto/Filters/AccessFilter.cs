using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using RomaAuto.Models;
using System.Net;
using RomaAuto.Helpers;

namespace RomaAuto.Filters
{
    public class LoginFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RomaDBEntities _db = new RomaDBEntities();
            if (!LoginHelper.IsLoggedIn())
            {
                filterContext.Result = new RedirectToRouteResult(
                       new RouteValueDictionary{{ "controller", "Account" }, { "action", "Login" } });
            }
            else
            {
                MainUser user = (MainUser)LoginHelper.CurrentUser();
                var userFromDb = _db.Operators.FirstOrDefault(item => item.OperatorID == user.Id && item.Name == user.Name && item.CategoryID == user.Category);
                if (userFromDb == null)
                {
                    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }
            
            base.OnActionExecuting(filterContext);
        }
    }

    public class AccessFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RomaDBEntities _db = new RomaDBEntities();
            MainUser user = LoginHelper.CurrentUser();

            if (!LoginHelper.IsLoggedIn() || _db.Operators.FirstOrDefault(item => item.CategoryID == user.Category) == null || user.Category < 3)
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}