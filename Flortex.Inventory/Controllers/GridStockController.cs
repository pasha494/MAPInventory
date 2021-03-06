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
using MAP.Inventory.Interface;

namespace MAP.Inventory.Web.Controllers
{
    [SessionExpire]
    public class GridStockController : Controller
    {
        public ActionResult GetLatestDocName(int Type)
        {
            ModelImple.LookUps _LookUps = new ModelImple.LookUps();
            PLog.Info("BEGIN::Controller > GridStock, Method > GetLatestDocName(int Type)");
            string str = "";
            try
            {
                if (Type > 0)
                    str = _LookUps.GetDocName(Type);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method > GetLatestDocName(int Type)", ex);
            }
            PLog.Info("END::Controller > GridStock, Method > GetLatestDocName(int Type)");
            return Content(str);
        }

        public string WarehouseData()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method > WarehouseData");
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> row;

            try
            { 
                IWareHouseImple oWareHouse = new WareHouseImple();
                DataTable dt = oWareHouse.GetGridData(0); //0 will gets all the WareHouse data   
  
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

        public string ProductsByCode()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method > ProductsByCode");
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
                PLog.Error("Error::Controller >GridStock, Method > ProductsByCode", ex);
            }
            PLog.Info("END::Controller > GridStock, Method > ProductsByCode");
            return serializer.Serialize(rows);
        }

        public string GetProductCategory()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method > GetProductCategory");
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
                PLog.Error("Error::Controller >GridStock, Method > GetProductCategory", ex);
            }
            PLog.Info("END::Controller > GridStock, Method > GetProductCategory");
            return serializer.Serialize(rows);
        }

        public string ProductsByName(string q)
        {
            PLog.Info("BEGIN::Controller > GridStock, Method > ProductsByName");
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
                PLog.Error("Error::Controller >GridStock, Method > ProductsByName", ex);
            }
            PLog.Info("END::Controller > GridStock, Method > ProductsByName");
            return serializer.Serialize(rows);
        }

        public ActionResult GetNextPrevDoc(string _Action, int DocType, int DocNo)
        {
            PLog.Info("BEGIN::Controller > GridStock, Method > GetNextPrevDoc"); 
            CommonImple _CommonImple = new CommonImple();
            if (DocType == 1)//opening stocks
            {
                DataSet ds = _CommonImple.GetNextPrevDocData(DocType);
                int x = GetDocNo(ds, DocNo, _Action);
                if (x == 0)
                    return RedirectToAction("AddOpStock");
                else
                    return RedirectToAction("UpDateOpStock", new { DocID = x });
            }
            else if (DocType == 2)
            {
                DataSet ds = _CommonImple.GetNextPrevDocData(DocType);
                int x = GetDocNo(ds, DocNo, _Action);
                if (x == 0)
                    return RedirectToAction("AddInStock");
                else
                    return RedirectToAction("UpDateInStock", new { DocID = x });
            }
            else if (DocType == 3)
            {
                DataSet ds = _CommonImple.GetNextPrevDocData(DocType);
                int x = GetDocNo(ds, DocNo, _Action);
                if (x == 0)
                    return RedirectToAction("AddOutStock");
                else
                    return RedirectToAction("UpdateOutStock", new { DocID = x });
            }
            else if (DocType == 4)
            {
                DataSet ds = _CommonImple.GetNextPrevDocData(DocType);
                int x = GetDocNo(ds, DocNo, _Action);
                if (x == 0)
                    return RedirectToAction("AddStockTransfer");
                else
                    return RedirectToAction("UpDateStockTransfer", new { DocID = x });
            }

            return Content("");

        }

        int GetDocNo(DataSet ds, int DocNo, string _Action)
        {
            int Ret = 0;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int Count = ds.Tables[0].Rows.Count;
                int Min = Convert.ToInt32(ds.Tables[0].Rows[0]["DocNo"].ToString());
                int Max = Convert.ToInt32(ds.Tables[0].Rows[Count - 1]["DocNo"].ToString());
                if (_Action.ToLower() == "prev")
                {
                    if (DocNo == Min)
                        Ret = Max;
                    else if (DocNo == 0)
                        Ret = Max;
                    else
                    {
                        DataRow dr = ds.Tables[0].Select("DocNo=" + DocNo).FirstOrDefault();
                        int Index = ds.Tables[0].Rows.IndexOf(dr);

                        Ret = Convert.ToInt32(ds.Tables[0].Rows[Index - 1]["DocNo"].ToString());
                    }
                }
                else if (_Action.ToLower() == "next")
                {
                    if (DocNo == Max)
                        Ret = Min;
                    else if (DocNo == 0)
                        Ret = Min;
                    else
                    {
                        DataRow dr = ds.Tables[0].Select("DocNo=" + DocNo).FirstOrDefault();
                        int Index = ds.Tables[0].Rows.IndexOf(dr);

                        Ret = Convert.ToInt32(ds.Tables[0].Rows[Index + 1]["DocNo"].ToString());
                    }
                }
            }
            return Ret;
        }

        public ActionResult GetReOrdQtyHistory(string ProductID, string WarehouseID, string Type)
        {
            PLog.Info("BEGIN::Controller > GridStock, Method > GetReOrdQtyHistory(string ProductID, string WarehouseID)");
            string str = "{}";

            try
            {

                if (!string.IsNullOrEmpty(ProductID) && !string.IsNullOrEmpty(WarehouseID))
                {
                    StockReportImple objModel = new StockReportImple();
                    if (Type.ToLower() == "inward")
                    {

                        DataSet ds = objModel.GetReOrdQtyHistory(Convert.ToInt32(ProductID), Convert.ToInt32(WarehouseID));

                        if (ds != null && ds.Tables.Count > 0)
                        {
                            str = JsonConvert.SerializeObject(ds.Tables[0]);
                        }
                    }
                    else if (Type.ToLower() == "outward")
                    {

                        DataSet ds = objModel.GetBlockedQtyHistory(Convert.ToInt32(ProductID), Convert.ToInt32(WarehouseID));

                        if (ds != null && ds.Tables.Count > 0)
                        {
                            str = JsonConvert.SerializeObject(ds.Tables[0]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method > GetReOrdQtyHistory(string ProductID, string WarehouseID)", ex);
            }
            PLog.Info("END::Controller > GridStock, Method > GetReOrdQtyHistory(string ProductID, string WarehouseID)");
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        [SessionExpire(FeatureKey = "DetailedReport", RequestType = 1)]
        public ActionResult GetStockReport()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >GetStockReport()");
            StockReportImple objModel = new StockReportImple();
            try
            {
                objModel.GetStockReportData();
                objModel.GenerateReportData();
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method > GetStockReport()", ex);
            }
            PLog.Info("END::Controller > GridStock, Method > GetStockReport()");
            return View(objModel);
        }

        [SessionExpire(FeatureKey = "ConsolidatedReport", RequestType = 1)]
        public ActionResult GetConsolidateStockReport()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >GetConsolidateStockReport()");
            StockReportImple objModel = new StockReportImple();
            try
            {
                objModel.GetStockReportData();
                objModel.GenerateReportData();
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method > GetConsolidateStockReport()", ex);
            }
            PLog.Info("END::Controller > GridStock, Method > GetConsolidateStockReport()");
            return View(objModel);
        }

        [SessionExpire(FeatureKey = "QuickReports", RequestType = 1)]
        public ActionResult GetQuickStockReport()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >GetStockReport()");
            StockReportImple objModel = new StockReportImple();
            try
            {
                objModel.GetStockReportData();
                objModel.GenerateReportData();
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method > GetStockReport()", ex);
            }
            PLog.Info("END::Controller > GridStock, Method > GetStockReport()");
            return View(objModel);
        }



        #region Opening Stocks

        /// <summary>
        /// Loat the opening stocks grid landing page screen
        /// </summary>
        /// <returns></returns>
        /// 
        [SessionExpire(FeatureKey = "OpeningStocksLanding", RequestType = 1)]
        public ActionResult OpeningStock()
        {
            return View();
        }

        /// <summary>
        /// Gets the Grid data for the OpeningStock landing page screen
        /// </summary>
        /// <returns></returns>
        /// 
        [SessionExpire(FeatureKey = "OpeningStocksLanding", RequestType = 1)]
        public string LoadOpeningStockDocuments()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >LoadOpeningStockDocuments()");
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            try
            {
                DataSet ds = new DataSet();
                OpeningStockImple objModel = new OpeningStockImple();
                ds = objModel.GetGirdData(0);//0 will get all the active opening masters data 
                DataTable dt = ds.Tables[0];
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
                PLog.Error("Error::Controller >GridStock, Method > LoadOpeningStockDocuments()", ex);
            }
            PLog.Info("END::Controller > GridStock, Method >LoadOpeningStockDocuments()");
            return serializer.Serialize(rows);
        }

        /// <summary>
        /// Opens the Opening masters Data entry screen.
        /// </summary>
        /// <returns></returns>
        /// 
        [SessionExpire(FeatureKey = "OPS_Add", RequestType = 1)]
        public ActionResult AddOpStock()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >AddOpStock()");
            OpeningStockImple objModel = new OpeningStockImple();
            try
            {
                objModel.init();
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method > AddOpStock()", ex);
            }
            PLog.Info("END::Controller > GridStock, Method >AddOpStock()");
            return View(objModel);
        }

        /// <summary>
        /// Below screen loads the Opening Stocks data entry screen in edit mode.
        /// </summary>
        /// <param name="DocID"></param>
        /// <returns></returns>
        /// 
        [SessionExpire(FeatureKey = "OPS_Edit", RequestType = 1)]
        public ActionResult UpDateOpStock(string DocID)
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >UpDateOpStock(string DocID)");
            OpeningStockImple objModel = new OpeningStockImple();

            if (!string.IsNullOrEmpty(DocID))
            {
                try
                {
                    objModel.EditDocument(Convert.ToInt32(DocID));
                }
                catch (Exception ex)
                {
                    PLog.Error("Error::Controller >GridStock, Method > UpDateOpStock(string DocID)", ex);
                }
            }
            PLog.Info("END::Controller > GridStock, Method >UpDateOpStock(string DocID)");
            return View("AddOpStock", objModel);
        }

        /// <summary>
        /// Saves the Opening stocks document..
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [SessionExpire(FeatureKey = "OPS_Save", RequestType = 2)]
        public string SaveDocumentOP(string Data)
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >SaveDocumentOP(string Data):::" + Data);
            string DocName = "";
            try
            {
                OpeningStockImple objModel = JsonConvert.DeserializeObject<OpeningStockImple>(Data);
                long Flg = 0;
                string[] sD = objModel.DocDate.Split('-');
                objModel.DocDate = new DateTime(Convert.ToInt32(sD[2]), Convert.ToInt32(sD[1]), Convert.ToInt32(sD[0])).ToString();
                DocName = objModel.SaveDocument(out Flg);
                if (Flg == -494)
                {
                    DocName = "-494";
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method > SaveDocumentOP(string Data)", ex);
            }
            PLog.Info("END::Controller > GridStock, Method >SaveDocumentOP(string Data):::" + Data);
            return DocName;
        }

        /// <summary>
        /// Deletes the Opening stock document ( just grade out the data)
        /// </summary>
        /// <param name="DocID"></param>
        /// <returns></returns>
        /// 
        [SessionExpire(FeatureKey = "OPS_Delete", RequestType = 2)]
        public ActionResult DeleteOpStock(string DocID)
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >DeleteOpStock(string DocID)");
            long flg = 0;
            if (!string.IsNullOrEmpty(DocID))
            {
                try
                {
                    OpeningStockImple obj = new OpeningStockImple();
                    flg = obj.DeleteDocument(Convert.ToInt32(DocID));
                }
                catch (Exception ex)
                {
                    PLog.Error("Error::Controller >GridStock, Method > DeleteOpStock(string DocID)", ex);
                }
            }
            PLog.Info("END::Controller > GridStock, Method >DeleteOpStock(string DocID)");
            return Content(flg.ToString());
        }


        #endregion

        #region Inward Stock Documents
        [SessionExpire(FeatureKey = "InwardLanding", RequestType = 1)]
        public ActionResult InwardStock()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >InwardStock()");
            PLog.Info("END::Controller > GridStock, Method >InwardStock()");
            return View();
        }

        [SessionExpire(FeatureKey = "InwardLanding", RequestType = 1)]
        public string LoadInwardStockDocuments()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >LoadInwardStockDocuments()");
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            try
            {
                DataSet ds = new DataSet();
                InwardDocumentImple objModel = new InwardDocumentImple();
                ds = objModel.GetGirdData(0);//0 will get all the active inward masters data 
                DataTable dt = ds.Tables[0];

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
                PLog.Error("Error::Controller >GridStock, Method > LoadInwardStockDocuments()", ex);
            }
            PLog.Info("END::Controller > GridStock, Method >LoadInwardStockDocuments()");
            return serializer.Serialize(rows);
        }

        [SessionExpire(FeatureKey = "INW_Add    ", RequestType = 1)]
        public ActionResult AddInStock()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >AddInStock()");
            InwardDocumentImple objModel = new InwardDocumentImple();
            try
            {
                objModel.init();
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method > AddInStock()", ex);
            }
            PLog.Info("END::Controller > GridStock, Method >AddInStock()");
            return View(objModel);
        }

        [SessionExpire(FeatureKey = "INW_Edit", RequestType = 1)]
        public ActionResult UpDateInStock(string DocID)
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >UpDateInStock(string DocID)");
            InwardDocumentImple objModel = new InwardDocumentImple();
            try
            {
                if (!string.IsNullOrEmpty(DocID))
                {
                    objModel.EditDocument(Convert.ToInt32(DocID));
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method > UpDateInStock(string DocID)", ex);
            }
            PLog.Info("END::Controller > GridStock, Method >UpDateInStock(string DocID)");
            return View("AddInStock", objModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        [SessionExpire(FeatureKey = "INW_Save", RequestType = 2)]
        public string SaveDocumentIN(string Data)
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >SaveDocumentIN(string Data):::" + Data);
            string DocName = "";
            try
            {
                InwardDocumentImple objModel = JsonConvert.DeserializeObject<InwardDocumentImple>(Data);
                long Flg = 0;
                string[] sD = objModel.DocDate.Split('-');
                objModel.DocDate = new DateTime(Convert.ToInt32(sD[2]), Convert.ToInt32(sD[1]), Convert.ToInt32(sD[0])).ToString();
                if (!string.IsNullOrEmpty(objModel.EffectiveDate))
                {
                    string[] sD1 = objModel.EffectiveDate.Split('-');
                    objModel.EffectiveDate = new DateTime(Convert.ToInt32(sD1[2]), Convert.ToInt32(sD1[1]), Convert.ToInt32(sD1[0])).ToString();
                }
                DocName = objModel.SaveDocument(out Flg);
                if (Flg == -494)
                {
                    DocName = "-494";
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method > SaveDocumentIN(string Data)", ex);
            }
            PLog.Info("END::Controller > GridStock, Method >SaveDocumentIN(string Data):::" + Data);
            return DocName;
        }

        [SessionExpire(FeatureKey = "INW_Delete", RequestType = 2)]
        public ActionResult DeleteInStock(string DocID)
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >DeleteInStock(string DocID)");
            long flg = 0;
            if (!string.IsNullOrEmpty(DocID))
            {
                try
                {
                    InwardDocumentImple obj = new InwardDocumentImple();
                    flg = obj.DeleteDocument(Convert.ToInt32(DocID));
                }
                catch (Exception ex)
                {
                    PLog.Error("Error::Controller >GridStock, Method > DeleteInStock(string DocID)", ex);
                }
            }
            PLog.Info("END::Controller > GridStock, Method >DeleteInStock(string DocID)");
            return Content(flg.ToString());
        }

        #endregion

        #region OutWard strock Documents

        [SessionExpire(FeatureKey = "OutwardLanding", RequestType = 1)]
        public ActionResult OutwardStock()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >OutwardStock()");
            PLog.Info("END::Controller > GridStock, Method >OutwardStock()");
            return View();
        }

        [SessionExpire(FeatureKey = "OutwardLanding", RequestType = 1)]
        public string LoadOutwardStockDocuments()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >LoadOutwardStockDocuments()");
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            try
            {
                DataSet ds = new DataSet();
                OutwardDocumentImple objModel = new OutwardDocumentImple();
                ds = objModel.GetGirdData(0);//0 will get all the active Outward masters data 
                DataTable dt = ds.Tables[0];
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
                PLog.Error("Error::Controller >GridStock, Method > LoadOutwardStockDocuments()", ex);
            }
            PLog.Info("END::Controller > GridStock, Method >LoadOutwardStockDocuments");
            return serializer.Serialize(rows);
        }

        [SessionExpire(FeatureKey = "OUT_Add", RequestType = 1)]
        public ActionResult AddOutStock()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >AddOutStock()");
            OutwardDocumentImple objModel = new OutwardDocumentImple();
            try
            {
                objModel.init();
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method > AddOutStock()", ex);
            }
            PLog.Info("END::Controller > GridStock, Method >AddOutStock()");
            return View(objModel);
        }

        [SessionExpire(FeatureKey = "OUT_Edit", RequestType = 1)]
        public ActionResult UpdateOutStock(string DocID)
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >UpdateOutStock(string DocID)");
            OutwardDocumentImple objModel = new OutwardDocumentImple();
            if (!string.IsNullOrEmpty(DocID))
            {
                try
                {
                    objModel.EditDocument(Convert.ToInt32(DocID));
                }
                catch (Exception ex)
                {
                    PLog.Error("Error::Controller >GridStock, Method > UpdateOutStock(string DocID)", ex);
                }
            }
            PLog.Info("END::Controller > GridStock, Method >UpdateOutStock(string DocID)");
            return View("AddOutStock", objModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        [SessionExpire(FeatureKey = "OUT_Save", RequestType = 2)]
        public string SaveDocumentOUT(string Data)
        {
            PLog.Info("BEGIN::Controller > GridStock, Method > SaveDocumentOUT(string Data):::" + Data);
            string DocName = "";
            try
            {
                OutwardDocumentImple objModel = JsonConvert.DeserializeObject<OutwardDocumentImple>(Data);
                long Flg = 0;
                string[] sD = objModel.DocDate.Split('-');
                objModel.DocDate = new DateTime(Convert.ToInt32(sD[2]), Convert.ToInt32(sD[1]), Convert.ToInt32(sD[0])).ToString();

                if (!string.IsNullOrEmpty(objModel.EffectiveDate))
                {
                    string[] sD1 = objModel.EffectiveDate.Split('-');
                    objModel.EffectiveDate = new DateTime(Convert.ToInt32(sD1[2]), Convert.ToInt32(sD1[1]), Convert.ToInt32(sD1[0])).ToString();
                }
                DocName = objModel.SaveDocument(out Flg);
                if (Flg == -494)
                {
                    DocName = "-494";
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method > SaveDocumentOUT(string Data)", ex);
            }
            PLog.Info("END::Controller > GridStock, Method > SaveDocumentOUT(string Data):::" + Data);
            return DocName;
        }

        [SessionExpire(FeatureKey = "OUT_Delete", RequestType = 2)]
        public ActionResult DeleteOutStock(string DocID)
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >  DeleteOutStock(string DocID)");
            long flg = 0;
            if (!string.IsNullOrEmpty(DocID))
            {
                try
                {
                    OutwardDocumentImple obj = new OutwardDocumentImple();
                    flg = obj.DeleteDocument(Convert.ToInt32(DocID));
                }
                catch (Exception ex)
                {
                    PLog.Error("Error::Controller >GridStock, Method >  DeleteOutStock(string DocID)", ex);
                }
            }
            PLog.Info("END::Controller > GridStock, Method >  DeleteOutStock(string DocID)");
            return Content(flg.ToString());
        }


        #endregion

        #region Stock Transfer
        [SessionExpire(FeatureKey = "StockTransferLanding", RequestType = 1)]
        public ActionResult StockTransfer()
        {
            return View();
        }

        [SessionExpire(FeatureKey = "StockTransferLanding", RequestType = 1)]
        public string LoadStockTransferDocuments()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method > LoadStockTransferDocuments()");
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            try
            {
                DataSet ds = new DataSet();
                StockTransferImple objModel = new StockTransferImple();
                ds = objModel.GetGirdData(0);//0 will get all the active StockTransfer masters data 
                DataTable dt = ds.Tables[0];

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
                PLog.Error("Error::Controller >GridStock, Method > LoadStockTransferDocuments()", ex);
            }
            PLog.Info("END::Controller > GridStock, Method > LoadStockTransferDocuments()");
            return serializer.Serialize(rows);
        }

        /// <summary>
        /// Opens the StockTransfer Data entry screen.
        /// </summary>
        /// <returns></returns>
        /// 
        [SessionExpire(FeatureKey = "STR_Add", RequestType = 1)]
        public ActionResult AddStockTransfer()
        {
            PLog.Info("BEGIN::Controller > GridStock, Method > AddStockTransfer()");
            StockTransferImple objModel = new StockTransferImple();
            try
            {
                objModel.init();
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method > AddStockTransfer()", ex);
            }
            PLog.Info("END::Controller > GridStock, Method > AddStockTransfer()");
            return View(objModel);
        }

        /// <summary>
        /// Below screen loads the Opening StockTransfer data entry screen in edit mode.
        /// </summary>
        /// <param name="DocID"></param>
        /// <returns></returns>
        /// 
        [SessionExpire(FeatureKey = "STR_Edit", RequestType = 1)]
        public ActionResult UpDateStockTransfer(string DocID)
        {
            PLog.Info("BEGIN::Controller > GridStock, Method > UpDateStockTransfer(string DocID)");
            StockTransferImple objModel = new StockTransferImple();
            try
            {
                if (!string.IsNullOrEmpty(DocID))
                {
                    objModel.EditDocument(Convert.ToInt32(DocID));
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method >UpDateStockTransfer(string DocID)", ex);
            }
            PLog.Info("END::Controller > GridStock, Method > UpDateStockTransfer(string DocID)");
            return View("AddStockTransfer", objModel);
        }

        /// <summary>
        /// Saves the StockTransfer document..
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [SessionExpire(FeatureKey = "STR_Save", RequestType = 2)]
        public string SaveStockTransfer(string Data)
        {
            PLog.Info("BEGIN::Controller > GridStock, Method >SaveStockTransfer(string Data):::" + Data);
            string DocName = "";
            try
            {
                StockTransferImple objModel = JsonConvert.DeserializeObject<StockTransferImple>(Data);
                long Flg = 0;
                string[] sD = objModel.DocDate.Split('-');
                objModel.DocDate = new DateTime(Convert.ToInt32(sD[2]), Convert.ToInt32(sD[1]), Convert.ToInt32(sD[0])).ToString();
                DocName = objModel.SaveDocument(out Flg);
                if (Flg == -494)
                {
                    DocName = "-494";
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Controller >GridStock, Method > SaveStockTransfer(string Data)", ex);
            }
            PLog.Info("END::Controller > GridStock, Method > SaveStockTransfer(string Data):::" + Data);
            return DocName;
        }

        /// <summary>
        /// Deletes the StockTransfer document ( just grade out the data)
        /// </summary>
        /// <param name="DocID"></param>
        /// <returns></returns>
        /// 
        [SessionExpire(FeatureKey = "STR_Delete", RequestType = 2)]
        public ActionResult DeleteStockTransfer(string DocID)
        {
            PLog.Info("BEGIN::Controller > GridStock, Method > DeleteStockTransfer(string DocID)");
            long flg = 0;
            if (!string.IsNullOrEmpty(DocID))
            {
                try
                {
                    StockTransferImple obj = new StockTransferImple();
                    flg = obj.DeleteDocument(Convert.ToInt32(DocID));
                }
                catch (Exception ex)
                {
                    PLog.Error("Error::Controller >GridStock, Method >  DeleteStockTransfer(string DocID)", ex);
                }
            }
            PLog.Info("END::Controller > GridStock, Method > DeleteStockTransfer(string DocID)");
            return Content(flg.ToString());
        }

        #endregion


    }
}
