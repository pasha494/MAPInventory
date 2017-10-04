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
using MAP.Logging;

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
                    Login obj = new Login();
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

        [SessionExpire]
        public ActionResult Index()
        {
            return View();
        }

        #region Users
        [SessionExpire]
        public ActionResult Users()
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

        [SessionExpire]
        public string LoadUsers()
        {
            PLog.Info("BEGIN::Controller > Home, Method > LoadUsers()");
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {

                Users objUsers = new Users();

                DataTable dt = objUsers.GetGridData(0); //0 will gets all the users data 


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

        [SessionExpire]
        public ActionResult UserCreation()
        {
            PLog.Info("BEGIN::Controller > Home, Method > UserCreation()");
            Users obj = null;
            try
            {
                obj = new Users();
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > UserCreation()", ex);

            }
            PLog.Info("END::Controller > Home, Method > UserCreation()");
            return View(obj);
        }

        [SessionExpire]
        public ActionResult SaveUser(string Data)
        {
            PLog.Info("BEGIN::Controller > Home, Method > SaveUser(string Data)");
            int flg = 0;
            try
            {
                Users obj = JsonConvert.DeserializeObject<Users>(Data);

                flg = obj.SaveUsers();
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > SaveUser(string Data)", ex);

            }
            PLog.Info("END::Controller > Home, Method > SaveUser(string Data)");
            return Content(flg.ToString());
        }

        [SessionExpire]
        public int DeleteUser(string ID)
        {
            PLog.Info("BEGIN::Controller > Home, Method > DeleteUser(string ID)");
            int ret = 0;
            if (!string.IsNullOrEmpty(ID))
            {
                try
                {
                    Users obj = new Users();
                    ret = obj.DeleteUser(Convert.ToInt32(ID));
                }
                catch (Exception ex)
                {
                    PLog.Error("Error::Controller > Home, Method > DeleteUser(string ID)", ex);

                }
            }
            PLog.Info("END::Controller > Home, Method > DeleteUser(string ID)");
            return ret;
        }

        [SessionExpire]
        public ActionResult UpDateUser(string ID)
        {
            PLog.Info("BEGIN::Controller > Home, Method > UpDateUser(string ID)");
            Users objModel = new Users();

            try
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    objModel.EditUser(Convert.ToInt32(ID));
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > UpDateUser(string ID)", ex);

            }
            PLog.Info("END::Controller > Home, Method > UpDateUser(string ID)");
            return View("UserCreation", objModel);
        }

        [SessionExpire]
        public ActionResult ChangePasswordView()
        {
            PLog.Info("BEGIN::Controller > Home, Method > ChangePasswordView");
            PLog.Info("END::Controller > Home, Method >ChangePasswordView");
            return View();
        }


        [SessionExpire]
        public ActionResult ChangePassword(string CurrentPassword, string NewPassword)
        {
            PLog.Info("BEGIN::Controller > Home, Method > ChangePasswordView(string CurrentPassword, string NewPassword)");
            int ret = 0;
            if (!string.IsNullOrEmpty(CurrentPassword) && !string.IsNullOrEmpty(NewPassword))
            {
                try
                {
                    Users obj = new Users();
                    ret = obj.ChangePassword(CurrentPassword, NewPassword);
                }
                catch (Exception ex)
                {
                    PLog.Error("Error::Controller > Home, Method > ChangePasswordView(string CurrentPassword, string NewPassword)", ex);

                }
            }
            PLog.Info("END::Controller > Home, Method >ChangePasswordView(string CurrentPassword, string NewPassword)");
            return Content(ret.ToString());
        }
        #endregion

        #region Product Masters


        [SessionExpire]
        public ActionResult ProductsCategoryList()
        {
            PLog.Info("BEGIN::Controller > Home, Method > ProductCategoryList()");
            string str = ""; 
            try
            {
                str = LoadProductsCategory();
                ViewBag.Data = str;
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > ProductCategoryList()", ex);
            }
            PLog.Info("END::Controller > Home, Method > ProductCategoryList()");
            return View();
        }

        //LoadProducts
        [SessionExpire]
        public string LoadProductsCategory()
        {
            PLog.Info("BEGIN::Controller > Home, Method > LoadProductsCategory()");

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            try
            {

                ProductsCategory oProducts = new ProductsCategory();

                DataTable dt = oProducts.GetGridData(0); //0 will gets all the products data


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
                PLog.Error("Error::Controller > Home, Method > LoadProductsCategory()", ex);
            }
            PLog.Info("END::Controller > Home, Method > LoadProductsCategory()");
            return serializer.Serialize(rows);
        }

        [SessionExpire]
        public ActionResult ProductsCategory()
        {
            PLog.Info("BEGIN::Controller > Home, Method > ProductsCategory");
            PLog.Info("END::Controller > Home, Method >ProductsCategory");
            return View(new ProductsCategory());
        }

        [SessionExpire]
        public ActionResult SaveProductsCategory(string Data)
        {
            PLog.Info("BEGIN::Controller > Home, Method > SaveProductsCategory(string Data)");
            int flg = 0;
            try
            {
                ProductsCategory obj = JsonConvert.DeserializeObject<ProductsCategory>(Data); 
                flg = obj.SaveProductCategory();
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > SaveProductsCategory(string Data)", ex);
            }
            PLog.Info("END::Controller > Home, Method > SaveProductsCategory(string Data)");
            return Content(flg.ToString());
        }

        [SessionExpire]
        public ActionResult UpdateProductsCategory(string ID)
        {
            PLog.Info("BEGIN::Controller > Home, Method > UpdateProductsCategory(string ID");
            ProductsCategory objModel = new ProductsCategory();
            try
            {

                if (!string.IsNullOrEmpty(ID))
                {
                    objModel.EditProdcutCategory(Convert.ToInt32(ID));
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > UpdateProductsCategory(string ID)", ex);
            }
            PLog.Info("END::Controller > Home, Method > UpdateProductsCategory(string ID)");
            return View("ProductsCategory", objModel);
        }

        [SessionExpire]
        public int DeleteProductsCategory(string ID)
        {
            PLog.Info("BEGIN::Controller > Home, Method > DeleteProductsCategory(string ID)");
            int ret = 0;
            if (!string.IsNullOrEmpty(ID))
            {
                try
                {
                    ProductsCategory obj = new ProductsCategory();
                    ret = obj.DeleteProductsCategory(Convert.ToInt32(ID));
                }
                catch (Exception ex)
                {
                    PLog.Error("Error::Controller > Home, Method > DeleteProductsCategory(string ID)", ex);
                }
            }
            PLog.Info("END::Controller > Home, Method > DeleteProductsCategory(string ID)");
            return ret;
        }



        [SessionExpire]
        public ActionResult ProductsList()
        {
            PLog.Info("BEGIN::Controller > Home, Method > ProductsList()");
            string str = "";

            try
            {
                str = LoadProducts();
                ViewBag.Data = str;
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > ProductsList()", ex);
            }
            PLog.Info("END::Controller > Home, Method > ProductsList()");
            return View();
        }

        //LoadProducts
        [SessionExpire]
        public string LoadProducts()
        {
            PLog.Info("BEGIN::Controller > Home, Method > LoadProducts()");

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            try
            {

                Products oProducts = new Products();

                DataTable dt = oProducts.GetGridData(0); //0 will gets all the products data


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
                PLog.Error("Error::Controller > Home, Method > LoadProducts()", ex);
            }
            PLog.Info("END::Controller > Home, Method > LoadProducts()");
            return serializer.Serialize(rows);
        }

        [SessionExpire]
        public ActionResult Products()
        {
            PLog.Info("BEGIN::Controller > Home, Method > ChangePasswordView");
            PLog.Info("END::Controller > Home, Method >ChangePasswordView");
            return View(new Products());
        }

        [SessionExpire]
        public ActionResult SaveProduct(string Data)
        {
            PLog.Info("BEGIN::Controller > Home, Method > SaveProduct(string Data)");
            int flg = 0;
            try
            {
                Products obj = JsonConvert.DeserializeObject<Products>(Data);
                string[] date = obj.efDate.Split('-');
                obj.efDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0])).ToString();
                flg = obj.SaveProducts();
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > SaveProduct(string Data)", ex);
            }
            PLog.Info("END::Controller > Home, Method > SaveProduct(string Data)");
            return Content(flg.ToString());
        }

        [SessionExpire]
        public ActionResult UpdateProduct(string ID)
        {
            PLog.Info("BEGIN::Controller > Home, Method > UpdateProduct(string ID");
            Products objModel = new Products();
            try
            {

                if (!string.IsNullOrEmpty(ID))
                {
                    objModel.EditProdcut(Convert.ToInt32(ID));
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > UpdateProduct(string ID)", ex);
            }
            PLog.Info("END::Controller > Home, Method > UpdateProduct(string ID)");
            return View("Products", objModel);
        }

        [SessionExpire]
        public int DeleteProduct(string ID)
        {
            PLog.Info("BEGIN::Controller > Home, Method > DeleteProduct(string ID)");
            int ret = 0;
            if (!string.IsNullOrEmpty(ID))
            {
                try
                {
                    Products obj = new Products();
                    ret = obj.DeleteProduct(Convert.ToInt32(ID));
                }
                catch (Exception ex)
                {
                    PLog.Error("Error::Controller > Home, Method > DeleteProduct(string ID)", ex);

                }
            }
            PLog.Info("END::Controller > Home, Method > DeleteProduct(string ID)");
            return ret;
        }


        #endregion


        #region Warehouse Master
        [SessionExpire]
        public ActionResult Warehouses()
        {
            PLog.Info("BEGIN::Controller > Home, Method >Warehouses ");
            string str = "";
            try
            {
                str = LoadWarehouses();
                ViewBag.Data = str;

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > Warehouses", ex);
            }
            PLog.Info("END::Controller > Home, Method > Warehouses");

            return View();

        }
        [SessionExpire]
        public string LoadWarehouses()
        {
            PLog.Info("BEGIN::Controller > Home, Method >LoadWarehouses ");
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            try
            {
                WareHouse oWareHouse = new WareHouse();
                DataTable dt = oWareHouse.GetGridData(0); //0 will gets all the WareHouse data  

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
                PLog.Error("Error::Controller > Home, Method > LoadWarehouses()", ex);
            }
            PLog.Info("END::Controller > Home, Method > LoadWarehouses");
            return serializer.Serialize(rows);
        }
        [SessionExpire]
        public ActionResult Warehouse()
        {
            PLog.Info("BEGIN::Controller > Home, Method >Warehouse() ");
            WareHouse objModel = new WareHouse();
            PLog.Info("END::Controller > Home, Method > Warehouse()");
            return View(objModel);
        }
        [SessionExpire]
        public ActionResult SaveWarehouse(string Data)
        {
            PLog.Info("BEGIN::Controller > Home, Method > SaveWarehouse(string Data) ");
            int flg = 0;
            try
            {
                WareHouse obj = JsonConvert.DeserializeObject<WareHouse>(Data);

                string[] date = obj.efDate.Split('-');

                obj.efDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0])).ToString();
                flg = obj.SaveWareHouse();
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > SaveWarehouse(string Data)", ex);
            }
            PLog.Info("END::Controller > Home, Method > SaveWarehouse(string Data)");
            return Content(flg.ToString());
        }
        [SessionExpire]
        public ActionResult UpDateWarehouse(string ID)
        {
            PLog.Info("BEGIN::Controller > Home, Method >UpDateWarehouse(string ID) ");
            WareHouse objModel = new WareHouse();
            try
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    objModel.EditWareHouse(Convert.ToInt32(ID));
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > UpDateWarehouse(string ID) ", ex);
            }
            PLog.Info("END::Controller > Home, Method > UpDateWarehouse(string ID) ");
            return View("Warehouse", objModel);
        }
        [SessionExpire]
        public int DeleteWarehouse(string ID)
        {
            PLog.Info("BEGIN::Controller > Home, Method >DeleteWarehouse(string ID) ");
            int ret = 0;
            if (!string.IsNullOrEmpty(ID))
            {
                try
                {
                    WareHouse obj = new WareHouse();
                    ret = obj.DeleteWareHouse(Convert.ToInt32(ID));
                }
                catch (Exception ex)
                {
                    PLog.Error("Error::Controller > Home, Method > DeleteWarehouse(string ID) ", ex);

                }
            }
            PLog.Info("END::Controller > Home, Method > DeleteWarehouse(string ID) ");
            return ret;
        }


        #endregion
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
