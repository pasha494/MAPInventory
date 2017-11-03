using MAP.Inventory.Common;
using MAP.Inventory.Logging;
using MAP.Inventory.ModelImple;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAP.Inventory.Web.Controllers
{
    public class GenericController : Controller
    {
        //
        // GET: /Generic/

        public ActionResult Index()
        {
            return View();
        }

        public string GetProductsListView(string wareHouseId, string searchField, string q)
        {
            // FeatureId for products view autocomplete list view data is "1"
            MapListViewImple _listView = new MapListViewImple(Convert.ToInt32(EnumListViews.Products));

            PLog.Info("BEGIN::Controller > GridStock, Method > WarehouseData");
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> row;

            try
            {
                DataTable dt = new DataTable();
                dt = _listView.GetProductsListViewData(wareHouseId, searchField, q);
                if (dt != null)
                {
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
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method >WarehouseData", ex);
                throw;
            }
            PLog.Info("END::Controller > GridStock, Method > WarehouseData");
            return serializer.Serialize(rows);


        }


        public string GetWareHousesListView(string searchField, string q)
        {
            // FeatureId for products view autocomplete list view data is "1"
            MapListViewImple _listView = new MapListViewImple(Convert.ToInt32(EnumListViews.WareHouses));

            PLog.Info("BEGIN::Controller > GridStock, Method > WarehouseData");
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> row;

            try
            {
                DataTable dt = new DataTable();
                dt = _listView.GetWareHousesListViewData(searchField, q);
                if (dt != null)
                {
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
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method >WarehouseData", ex);
                throw;
            }
            PLog.Info("END::Controller > GridStock, Method > WarehouseData");
            return serializer.Serialize(rows);
        }


        public string GetCustomerListtView(string searchField, string q)
        {
            // FeatureId for products view autocomplete list view data is "1"
            MapListViewImple _listView = new MapListViewImple(Convert.ToInt32(EnumListViews.Customers));

            PLog.Info("BEGIN::Controller > GridStock, Method > WarehouseData");
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> row;

            try
            {
                DataTable dt = new DataTable();
                dt = _listView.GetCustomerListViewData(1, searchField, q);
                if (dt != null)
                {
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
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method >WarehouseData", ex);
                throw;
            }
            PLog.Info("END::Controller > GridStock, Method > WarehouseData");
            return serializer.Serialize(rows);
        }

        public string GetVendorListtView(string searchField, string q)
        {
            // FeatureId for products view autocomplete list view data is "1"
            MapListViewImple _listView = new MapListViewImple(Convert.ToInt32(EnumListViews.Vendors));

            PLog.Info("BEGIN::Controller > GridStock, Method > WarehouseData");
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> row;

            try
            {
                DataTable dt = new DataTable();
                dt = _listView.GetCustomerListViewData(0, searchField, q);
                if (dt != null)
                {
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
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method >WarehouseData", ex);
                throw;
            }
            PLog.Info("END::Controller > GridStock, Method > WarehouseData");
            return serializer.Serialize(rows);
        }
         

    }
}
