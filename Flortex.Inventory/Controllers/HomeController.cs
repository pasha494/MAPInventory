using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using MAP.Inventory.Models;
using Newtonsoft.Json;
using MAP.Inventory.Logging;
using MAP.Inventory.ModelImple;
using MAP.Inventory.Model;
using MAP.Inventory.Common;

namespace MAP.Inventory.Controllers
{

    public class HomeController : Controller
    { 

        [SessionExpireAttribute]
        public ActionResult UnAuthorizedAction()
        {
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "This is the Login Page";
            PLog.Info("BEGIN::Controller > Home, Method > Login");
            PLog.Info("END::Controller > Home, Method >Login");
            return View();
        }

        [SessionExpireAttribute]
        public ActionResult SignOUt()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }


        string CreateLoginCookies(int IsCreate, string email, string password)
        {
            string Pass = "";
            if (IsCreate == 1)
            {
                var cookieRemeberMe = new HttpCookie("RemeberMe", IsCreate.ToString());
                HttpContext.Response.Cookies.Add(cookieRemeberMe);
                HttpContext.Response.Cookies["RemeberMe"].Expires = DateTime.Now.AddDays(30);

                var cookieUserName = new HttpCookie("UserName", email);
                HttpContext.Response.Cookies.Add(cookieUserName);
                HttpContext.Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);

                var cookiePassword = new HttpCookie("Password", Crypto.Encrypt(password, true));
                HttpContext.Response.Cookies.Add(cookiePassword);
                HttpContext.Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
            }
            else
            {
                HttpContext.Response.Cookies["RemeberMe"].Expires = DateTime.Now.AddDays(-1);
                HttpContext.Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                HttpContext.Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
            }
            return Pass;
        }


        public ActionResult LoginSubmit(string email, string password, int RememberMe)
        {
            PLog.Info("BEGIN::Controller > Home, Method > LoginSubmit(string email, string password)");
            int Flg = 0;

            string _Password = CreateLoginCookies(RememberMe, email, password);
            try
            {
                // str = Convert.ToInt32(str).ToString();
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    LoginImple obj = new LoginImple();
                    Flg = obj.CheckLogin(email, password);
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > LoginSubmit", ex);
            }
            PLog.Info("END::Controller > Home, Method > LoginSubmit");
            return Content(Flg.ToString());
        }

        [SessionExpire(Page="Landing")]
        public ActionResult Index()
        {
            return View();
        }

        

        [HttpPost]
        public ActionResult Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public ActionResult SetPrefCookies(string Type, string Status)
        {
            if (!string.IsNullOrEmpty(Type) && Type == "1")
            {
                if (Request.Cookies["ShowReOrdQty"] == null)
                {
                    var userCookie = new HttpCookie("ShowReOrdQty", Status);
                    HttpContext.Response.Cookies.Add(userCookie);
                    HttpContext.Response.Cookies["ShowReOrdQty"].Expires = DateTime.Now.AddDays(30);
                }
                else if (Request.Cookies["ShowReOrdQty"] != null)
                {
                    var userCookie = new HttpCookie("ShowReOrdQty") { Value = Status };
                    HttpContext.Response.Cookies.Set(userCookie);
                }
            }
            else if (!string.IsNullOrEmpty(Type) && Type == "2")
            {
                if (Request.Cookies["ShowBlckQty"] == null)
                {
                    var userCookie = new HttpCookie("ShowBlckQty", Status);
                    HttpContext.Response.Cookies.Add(userCookie);
                    HttpContext.Response.Cookies["ShowBlckQty"].Expires = DateTime.Now.AddDays(30);
                }
                else if (Request.Cookies["ShowBlckQty"] != null)
                {
                    var userCookie = new HttpCookie("ShowBlckQty") { Value = Status };
                    HttpContext.Response.Cookies.Set(userCookie);
                }
            }


            return Content("1");
        }


    }

}
