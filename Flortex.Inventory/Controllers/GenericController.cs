﻿using MAP.Inventory.Logging;
using MAP.Inventory.ModelImple;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAP.Inventory.Controllers
{
    public class GenericController : Controller
    {
        //
        // GET: /Generic/

        public ActionResult Index()
        {
            return View();
        }


        public string GetProductsListView(string filterCol, string q)
        {
            MAPAutoCompleteListViewImple _listView = new MAPAutoCompleteListViewImple();

            PLog.Info("BEGIN::Controller > GridStock, Method > WarehouseData");
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> row;

            try
            {
                DataTable dt = new DataTable();
                dt = _listView.GetProductsListViewData(filterCol, q); 

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
                PLog.Error("Error::Controller >GridStock, Method >WarehouseData", ex);
                throw;
            }
            PLog.Info("END::Controller > GridStock, Method > WarehouseData");
            return serializer.Serialize(rows);


        }

    }
}
