using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flortex.Inventory.Models
{
    public class Login
    {

        public int CheckLogin(string UserEmail, string Password)
        {
            int flg = 0;

            DataSet ds = new DataSet();
            ds=DAL.GetDataSet(" Select * from Users Where Email='" + UserEmail + "'");
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (Password == ds.Tables[0].Rows[0]["Password"].ToString())
                    {
                        flg = 1;
                        CreateUserSession(ds.Tables[0]);
                    }
                    else
                        flg = -2;// Wrong password..

                }
                else
                    flg = -1;//Invalid user id..  
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Login, Method > CheckLogin(string UserEmail, string Password)", ex);
            }

            return flg;
        }

        public void CreateUserSession(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                SessionManager objSession = new SessionManager();
                objSession.UserID = Convert.ToInt32(dt.Rows[0]["id"].ToString());
                objSession.UserName = dt.Rows[0]["USerName"].ToString();
                objSession.Email = dt.Rows[0]["Email"].ToString();
                objSession.IsAdmin = Convert.ToBoolean(dt.Rows[0]["IsAdmin"]);
                if (dt.Rows[0]["RoleID"] != DBNull.Value)
                    objSession.RoleID = Convert.ToInt32(dt.Rows[0]["RoleID"].ToString());
                else
                    objSession.RoleID = 0;
                HttpContext.Current.Session.Add("SessionManager", objSession);
            }
        }

    }

    public class SessionManager
    {
        public int UserID { set; get; }
        public string UserName { set; get; }
        public string Email { set; get; }
        public bool IsAdmin { get; set; }
        public int RoleID { get; set; }
    }


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