using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using T.Model;
using System.Web.Routing;

namespace T.Model
{
    [AttributeUsage(AttributeTargets.All)]
    public class ValidateUserAttribute : AuthorizeAttribute
    {
        public string AllowedRoles { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            clsSignin objSignIn = new clsSignin();
            if (HttpContext.Current.Session["UserInfo"] != null && isAuthorized)
            {
                string userRole = ((clsSignin)(HttpContext.Current.Session["UserInfo"])).Role;
                if (Authenticate(userRole))
                {
                    return true;
                }
            }
            return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Home", action = "DashBoard" }));
            }
        }
        protected bool Authenticate(string struserRole)
        {
            List<string> lstRoles = SplitRoles(AllowedRoles);
            if (lstRoles.Count > 0)
            {
                if (lstRoles.Contains(struserRole))
                    return true;
            }
            return false;
        }
        public List<string> SplitRoles(string strRoles)
        {
            List<string> spltRoles = new List<string>();
            if (!string.IsNullOrEmpty(strRoles))
                spltRoles = strRoles.Split(',').ToList();
            return spltRoles;
        }
    }
}
