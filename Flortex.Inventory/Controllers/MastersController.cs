using MAP.Inventory.Logging;
using MAP.Inventory.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAPInventory.Controllers
{
    [SessionExpire]
    public class MastersController : Controller
    {

        #region Product Masters
                
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
         

        public ActionResult ProductsCategory()
        {
            PLog.Info("BEGIN::Controller > Home, Method > ProductsCategory");
            PLog.Info("END::Controller > Home, Method >ProductsCategory");
            return View(new ProductsCategory());
        }
         

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
         

        public ActionResult Products()
        {
            PLog.Info("BEGIN::Controller > Home, Method > ChangePasswordView");
            PLog.Info("END::Controller > Home, Method >ChangePasswordView");
            return View(new Products());
        }
         

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
        
        public ActionResult Warehouse()
        {
            PLog.Info("BEGIN::Controller > Home, Method >Warehouse() ");
            WareHouse objModel = new WareHouse();
            PLog.Info("END::Controller > Home, Method > Warehouse()");
            return View(objModel);
        }
     
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

    }
}
