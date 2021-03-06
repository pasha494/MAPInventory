﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using MAP.Inventory.Web.Models;
using Newtonsoft.Json;
using MAP.Inventory.Logging;
using MAP.Inventory.ModelImple;
using MAP.Inventory.Model;
using MAP.Inventory.Common;

namespace MAP.Inventory.Web.Controllers
{

    public class HomeController : Controller
    {

        //        select top 7 OS.DocNo, SUM(OM.Quantity) Quantity ,SUM(OM.TotalPrice)  TotalPrice
        //        from OpeningStockMaster OS
        //Inner join OpeningMasterData OM on OS.DocNo=OM.DocNo
        //group by OS.DocNo
        //order by OS.DocNo desc





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


        void CreateLoginCookies(int IsCreate, string email, string password)
        {
           
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
        }


        public ActionResult LoginSubmit(string email, string password, int RememberMe)
        {
            PLog.Info("BEGIN::Controller > Home, Method > LoginSubmit(string email, string password)");
            int Flg = 0;

           
            try
            {
                // str = Convert.ToInt32(str).ToString();
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    LoginImple obj = new LoginImple();
                    Flg = obj.CheckLogin(email, password);
                    if (Flg > 0)
                        CreateLoginCookies(RememberMe, email, password);
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > LoginSubmit", ex);
            }
            PLog.Info("END::Controller > Home, Method > LoginSubmit");
            return Content(Flg.ToString());
        }


        [SessionExpire(FeatureKey = "HomeLanding", RequestType = 1)]
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


        public ActionResult UnAuthorizedFeature()
        {

            return Content("-493");
        }

        public ActionResult HealthCheck()
        {

            return PartialView(new HealthCheckImple());
        }


    }

}
