using MAP.Inventory.Interface;
using MAP.Inventory.Logging;
using MAP.Inventory.Model;
using MAP.Inventory.ModelImple;
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

                IProductsCategoryImple oProducts = new ProductsCategoryImple();

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
            return View(new ProductsCategoryModel());
        }
         

        public ActionResult SaveProductsCategory(string Data)
        {
            PLog.Info("BEGIN::Controller > Home, Method > SaveProductsCategory(string Data)");
            long flg = 0;
            try
            {
                ProductsCategoryModel obj = JsonConvert.DeserializeObject<ProductsCategoryModel>(Data);
                IProductsCategoryImple oProducts = new ProductsCategoryImple();
                flg = oProducts.SaveProductCategory(obj);
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
            ProductsCategoryModel objModel = null;
            IProductsCategoryImple oProducts = new ProductsCategoryImple();
            try
            {

                if (!string.IsNullOrEmpty(ID))
                {
                    objModel= oProducts.EditProdcutCategory(Convert.ToInt32(ID));
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > UpdateProductsCategory(string ID)", ex);
            }
            PLog.Info("END::Controller > Home, Method > UpdateProductsCategory(string ID)");
            return View("ProductsCategory", objModel);
        }
         

        public long DeleteProductsCategory(string ID)
        {
            PLog.Info("BEGIN::Controller > Home, Method > DeleteProductsCategory(string ID)");
            long ret = 0;
            if (!string.IsNullOrEmpty(ID))
            {
                try
                {
                    IProductsCategoryImple oProducts = new ProductsCategoryImple();
                    ret = oProducts.DeleteProductsCategory(Convert.ToInt32(ID));
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

                IProductsImple oProductsImple = new ProductsImple();

                DataTable dt = oProductsImple.GetGridData(0); //0 will gets all the products data


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
            return View(new ProductsModel());
        }
         

        public ActionResult SaveProduct(string Data)
        {
            PLog.Info("BEGIN::Controller > Home, Method > SaveProduct(string Data)");
            long flg = 0;
            IProductsImple oProductsImple = new ProductsImple();
            try
            {
                ProductsModel obj = JsonConvert.DeserializeObject<ProductsModel>(Data);
                string[] date = obj.efDate.Split('-');
                obj.efDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0])).ToString();
                flg = oProductsImple.SaveProducts(obj);
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
            IProductsImple oProductsImple = new ProductsImple();
            ProductsModel objModel = null;
            try
            {

                if (!string.IsNullOrEmpty(ID))
                {
                    objModel=oProductsImple.EditProdcut(Convert.ToInt32(ID));
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > UpdateProduct(string ID)", ex);
            }
            PLog.Info("END::Controller > Home, Method > UpdateProduct(string ID)");
            return View("Products", objModel);
        }


        public long DeleteProduct(string ID)
        {
            PLog.Info("BEGIN::Controller > Home, Method > DeleteProduct(string ID)");
            long ret = 0;
            if (!string.IsNullOrEmpty(ID))
            {
                try
                {
                    IProductsImple oProductsImple = new ProductsImple(); 
                    ret = oProductsImple.DeleteProduct(Convert.ToInt32(ID));
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
                IWareHouseImple oWareHouse = new WareHouseImple();
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
            WareHouseModel objModel = new WareHouseModel();
            PLog.Info("END::Controller > Home, Method > Warehouse()");
            return View(objModel);
        }
     
        public ActionResult SaveWarehouse(string Data)
        {
            PLog.Info("BEGIN::Controller > Home, Method > SaveWarehouse(string Data) ");
            long flg = 0;
            try
            {
                WareHouseModel obj = JsonConvert.DeserializeObject<WareHouseModel>(Data);

                IWareHouseImple oWareHouse = new WareHouseImple();

                string[] date = obj.efDate.Split('-');

                obj.efDate = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0])).ToString();
                flg = oWareHouse.SaveWareHouse(obj);
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
            WareHouseModel objModel = null;
            IWareHouseImple oWareHouse = new WareHouseImple();
            try
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    objModel = oWareHouse.EditWareHouse(Convert.ToInt32(ID));
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller > Home, Method > UpDateWarehouse(string ID) ", ex);
            }
            PLog.Info("END::Controller > Home, Method > UpDateWarehouse(string ID) ");
            return View("Warehouse", objModel);
        }
        
        public long DeleteWarehouse(string ID)
        {
            PLog.Info("BEGIN::Controller > Home, Method >DeleteWarehouse(string ID) ");
            long ret = 0;
            if (!string.IsNullOrEmpty(ID))
            {
                try
                {
                    IWareHouseImple oWareHouse = new WareHouseImple();
                    ret = oWareHouse.DeleteWareHouse(Convert.ToInt32(ID));
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
