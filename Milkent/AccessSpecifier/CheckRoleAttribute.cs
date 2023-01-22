using System;
using System.Web.Mvc;
using System.Web.Security;
namespace Milkent.AccessSpecifier
{

    public class CheckRoleAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public string AllowedRoles { get; set; }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (System.Web.HttpContext.Current.Session!=null&& System.Web.HttpContext.Current.Session["Role"] != null&& AllowedRoles!=null)
                {
                    
                    string userRole = System.Web.HttpContext.Current.Session["Role"].ToString();
                    bool userAuthorized = false;

                    userAuthorized = false;

                    if (userRole == AllowedRoles)
                    {
                        userAuthorized = true;
                    }
                    if (userAuthorized == false)
                    {
                        filterContext.HttpContext.Response.Redirect("/Home/ErrorPage", true);
                    }
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect(FormsAuthentication.LoginUrl + String.Format("?ReturnUrl={0}", filterContext.HttpContext.Request.Url.AbsolutePath), true);
                }
            }
            else
            {
                filterContext.HttpContext.Response.Redirect(FormsAuthentication.LoginUrl + String.Format("?ReturnUrl={0}", filterContext.HttpContext.Request.Url.AbsolutePath), true);
            }


        }
    }

}




//I implemented the following ActionFilterAttribute and it works to handle both authentication and roles.I am storing roles in my own DB tables like this:

//User
//UserRole (contains UserID and RoleID foreign keys)
//Role
//public class CheckRoleAttribute : ActionFilterAttribute
//{
//    public string[] AllowedRoles { get; set; }


//    public override void OnActionExecuting(ActionExecutingContext filterContext)
//    {
//        string userName = filterContext.HttpContext.User.Identity.Name;

//        if (filterContext.HttpContext.User.Identity.IsAuthenticated)
//        {
//            if (AllowedRoles.Count() > 0)
//            {
//                IUserRepository userRepository = new UserRepository();
//                User user = userRepository.GetUser(userName);
//                bool userAuthorized = false;
//                foreach (Role userRole in user.Roles)
//                {
//                    userAuthorized = false;
//                    foreach (string allowedRole in AllowedRoles)
//                    {
//                        if (userRole.Name == allowedRole)
//                        {
//                            userAuthorized = true;
//                            break;
//                        }
//                    }
//                }
//                if (userAuthorized == false)
//                {
//                    filterContext.HttpContext.Response.Redirect("/Account/AccessViolation", true);
//                }
//            }
//            else
//            {
//                filterContext.HttpContext.Response.Redirect("/Account/AccessViolation", true);
//            }
//        }
//        else
//        {
//            filterContext.HttpContext.Response.Redirect(FormsAuthentication.LoginUrl + String.Format("?ReturnUrl={0}", filterContext.HttpContext.Request.Url.AbsolutePath), true);
//        }


//    }
//    I call this like this...

//    [CheckRole(AllowedRoles = new string[] { "admin" })]
//    public ActionResult Delete(int id)
//    {
//        //delete logic here
//    }