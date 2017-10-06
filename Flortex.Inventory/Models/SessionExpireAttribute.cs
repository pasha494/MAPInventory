using MAP.Inventory.Logging;
using MAP.Inventory.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAP.Inventory.Models
{ 
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public string Page { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
           
            // check  sessions here
            if ((HttpContext.Current == null || HttpContext.Current.Session == null) || HttpContext.Current.Session["SessionManager"] == null)
            {
                filterContext.Result = new RedirectResult("~/Home/Login");
                return;
            }
            else if(HttpContext.Current.Session != null &&  HttpContext.Current.Session["SessionManager"] !=null )
            {
                
                SessionManager objSession = (SessionManager)HttpContext.Current.Session["SessionManager"];
                if (objSession.RoleID == 2)//Sales man
                {
                    bool flg = false;
                    string Controller=((filterContext.ActionDescriptor).ControllerDescriptor).ControllerName;
                    string ActionName = (filterContext.ActionDescriptor).ActionName;

                    if (Controller.ToLower() == "home" && (ActionName.ToLower() == "index") || ActionName.ToLower() == "signout"
                        || ActionName.ToLower() == "changepasswordview" || ActionName.ToLower() == "unauthorizedaction"
                        || ActionName.ToLower() == "changepassword"
                        )
                    {
                        flg = true;
                    }
                    else if (Controller.ToLower() == "gridstock" && (ActionName.ToLower() == "getstockreport"
                        || ActionName.ToLower() == "productsbyname" || ActionName.ToLower() == "getproductcategory"
                        || ActionName.ToLower() == "productsbycode"
                        || ActionName.ToLower() == "getquickstockreport"
                        || ActionName.ToLower() == "getreordqtyhistory"
                        || ActionName.ToLower() == "getconsolidatestockreport" 
                        ))
                    {
                        flg = true;
                    }

                    if (!flg)
                    {
                        filterContext.Result = new RedirectResult("~/Home/UnAuthorizedAction");
                        return;
                    }

                }
                else if (objSession.RoleID == 3)
                {
                    bool flg = true;
                    string Controller = ((filterContext.ActionDescriptor).ControllerDescriptor).ControllerName;
                    string ActionName = (filterContext.ActionDescriptor).ActionName;

                    if (Controller.ToLower() == "home" && (ActionName.ToLower() == "usercreation" || ActionName.ToLower() == "users"
                        || ActionName.ToLower() == "saveuser" || ActionName.ToLower() == "deleteuser" || ActionName.ToLower() == "updateuser")) 
                    {
                        flg = false;
                    }
                    if (!flg)
                    {
                        filterContext.Result = new RedirectResult("~/Home/UnAuthorizedAction");
                        return;
                    }
                }

            }


            base.OnActionExecuting(filterContext);
        }
    }
}