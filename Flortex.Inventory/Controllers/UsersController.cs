using MAP.Inventory.Logging;
using MAP.Inventory.Model;
using MAP.Inventory.ModelImple;
using MAP.Inventory.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace MAP.Inventory.Web.Controllers
{
    [SessionExpire]
    public class UsersController : Controller
    { 
        public ActionResult Index()
        {
            PLog.Info("BEGIN::Controller > Home, Method > Users()");
            string str = "";
            try
            {
                str = LoadUsers();
                ViewBag.Data = str;
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > Users()", ex);

            }
            PLog.Info("END::Controller > Home, Method > Users()");
            return View();
        }

        
        public string LoadUsers()
        {
            PLog.Info("BEGIN::Controller > Home, Method > LoadUsers()");
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {
                UsersImple objUsersImple = new UsersImple();

                DataTable dt = objUsersImple.GetGridData(0); //0 will gets all the users data 


                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > LoadUsers()", ex);

            }
            PLog.Info("END::Controller > Home, Method > LoadUsers()");
            return serializer.Serialize(rows);
        }

       
        public ActionResult UserCreation()
        {
            PLog.Info("BEGIN::Controller > Home, Method > UserCreation()");
            UsersModel obj = null;
            try
            {
                obj = new UsersModel();
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > UserCreation()", ex);

            }
            PLog.Info("END::Controller > Home, Method > UserCreation()");
            return View(obj);
        }

        [SessionExpire(FeatureKey = "SaveUsers", RequestType = 2)]
        public ActionResult SaveUser(string Data)
        {
            PLog.Info("BEGIN::Controller > Home, Method > SaveUser(string Data)");
            long flg = 0;
            try
            {
                UsersImple objUsersImple = new UsersImple();
                UsersModel objUser = JsonConvert.DeserializeObject<UsersModel>(Data);

                flg = objUsersImple.SaveUsers(objUser);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > SaveUser(string Data)", ex);

            }
            PLog.Info("END::Controller > Home, Method > SaveUser(string Data)");
            return Content(flg.ToString());
        }

       
        public long DeleteUser(string ID)
        {
            PLog.Info("BEGIN::Controller > Home, Method > DeleteUser(string ID)");
            long ret = 0;
            if (!string.IsNullOrEmpty(ID))
            {
                try
                {
                    UsersImple objUsersImple = new UsersImple();
                    ret = objUsersImple.DeleteUser(Convert.ToInt32(ID));
                }
                catch (Exception ex)
                {
                    PLog.Error("Error::Controller > Home, Method > DeleteUser(string ID)", ex);

                }
            }
            PLog.Info("END::Controller > Home, Method > DeleteUser(string ID)");
            return ret;
        }

       
        public ActionResult UpDateUser(string ID)
        {
            PLog.Info("BEGIN::Controller > Home, Method > UpDateUser(string ID)");
            UsersImple objUsersImple = new UsersImple();
            UsersModel objModel = null;

            try
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    objModel = objUsersImple.EditUser(Convert.ToInt32(ID));
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > UpDateUser(string ID)", ex);

            }
            PLog.Info("END::Controller > Home, Method > UpDateUser(string ID)");
            return View("UserCreation", objModel);
        }

        
        public ActionResult ChangePasswordView()
        {
            PLog.Info("BEGIN::Controller > Home, Method > ChangePasswordView");
            PLog.Info("END::Controller > Home, Method >ChangePasswordView");
            return View();
        }


        
        public ActionResult ChangePassword(string CurrentPassword, string NewPassword)
        {
            PLog.Info("BEGIN::Controller > Home, Method > ChangePasswordView(string CurrentPassword, string NewPassword)");
            long ret = 0;
            if (!string.IsNullOrEmpty(CurrentPassword) && !string.IsNullOrEmpty(NewPassword))
            {
                try
                {
                    UsersImple objUsersImple = new UsersImple();
                    ModelImple.LookUps _LookUps = new ModelImple.LookUps();
                    ret = objUsersImple.ChangePassword(CurrentPassword, NewPassword, Convert.ToInt32(_LookUps.GetSessionObject("UserID")));
                }
                catch (Exception ex)
                {
                    PLog.Error("Error::Controller > Home, Method > ChangePasswordView(string CurrentPassword, string NewPassword)", ex);

                }
            }
            PLog.Info("END::Controller > Home, Method >ChangePasswordView(string CurrentPassword, string NewPassword)");
            return Content(ret.ToString());
        } 

    }
}
