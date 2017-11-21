using MAP.Inventory.Common;
using MAP.Inventory.DAL;
using MAP.Inventory.Interface;
using MAP.Inventory.Logging;
using MAP.Inventory.Model;
using MAP.Inventory.ModelImple;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MAP.Inventory.Web.Models
{
    public class OutwardDocumentImple : IDocument
    {
        LookUps _LookUps = new LookUps();
        General _General = new General();

        public int DocID { get; set; }
        public string DocName { get; set; }
        public string DocDate { get; set; }
        public string WareHouseId { get; set; }
        public string WareHouseName { get; set; }
        public bool IsExpected { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }

        public string EffectiveDate { get; set; }
        public string Comments { get; set; }
        public string GridData { get; set; }

        public string WareHouseOptions { get; set; }
        public string ProductsOptions { get; set; }
        public string CustomerOptions { get; set; }

        public GridViewCustomization GridView { get; set; }

        public OutwardDocumentImple()
        {
            Comments = string.Empty;
            WareHouseName = string.Empty;
            CustomerName = string.Empty;
            GridData = string.Empty;
        }

        public void init()
        {
            DocName = _LookUps.GetDocName((int)EnumGridView.OutwardStock);// 3 stands for inward documents.
            LoadScreenControls();
        }

        void LoadScreenControls()
        {
            GetWareHouseListViewOptions();
            GetVendorListViewOptions();
            GetProductsListViewOptions();
            GetDocumentsGridViewOptions();
        }

        public void GetWareHouseListViewOptions()
        {
            MapListViewImple _wareHouseListView = new MapListViewImple((int)EnumListViews.WareHouses);
            this.WareHouseOptions = _wareHouseListView.GetListViewOptions();
        }

        public void GetVendorListViewOptions()
        {
            MapListViewImple _vendorListView = new MapListViewImple((int)EnumListViews.Customers);
            this.CustomerOptions = _vendorListView.GetListViewOptions();
        }

        public void GetProductsListViewOptions()
        {
            MapListViewImple _productsListView = new MapListViewImple((int)EnumListViews.Products);
            this.ProductsOptions = _productsListView.GetListViewOptions();
        }

        public void GetDocumentsGridViewOptions()
        {
            MapGridViewImple _productsListView = new MapGridViewImple((int)EnumGridView.OutwardStock);
            this.GridView = _productsListView.GetGridViewsCustomizationInfo(DocID);
        }

        string ConverDate(DateTime date)
        {
            string str = "";

            if (date != null)
            {
                str = date.Day + "-" + date.Month + "-" + date.Year;
            }
            else
            {
                str = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
            }

            return str;
        }

        public void EditDocument(int DocID)
        {
            DataSet ds = GetGirdData(DocID);
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        this.DocID = DocID;
                        this.DocName = ds.Tables[0].Rows[0]["DocName"].ToString();
                        this.DocDate = ConverDate(Convert.ToDateTime(ds.Tables[0].Rows[0]["DocDate"].ToString()));
                        this.WareHouseId = ds.Tables[0].Rows[0]["WareHouseId"].ToString();
                        this.WareHouseName = ds.Tables[0].Rows[0]["WareHouseName"].ToString();
                        this.IsExpected = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsExpected"]);

                        if (this.IsExpected)
                        {
                            this.EffectiveDate = ConverDate(Convert.ToDateTime(ds.Tables[0].Rows[0]["efDate"].ToString()));
                        }
                        else
                            this.EffectiveDate = ConverDate(DateTime.Now);

                        this.CustomerId = ds.Tables[0].Rows[0]["CustomerId"].ToString();
                        this.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                        this.Comments = ds.Tables[0].Rows[0]["Comments"].ToString();
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        this.GridData = JsonConvert.SerializeObject(ds.Tables[1]);
                    }
                    LoadScreenControls();
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OutwardDocument, Method > EditDocument(int DocID)", ex);
            }
        }

        public DataSet GetGirdData(int DocID)
        {
            DataSet ds = new DataSet();
            try
            { 
                ds = _General.Get(new ArrayList() { DocID }, "sp_GetOutwardDocData", 0);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OutwardDocument, Method > GetGirdData(int DocID)", ex);
            }
            return ds; throw new NotImplementedException();
        }

        public string SaveDocument(out long ret)
        {
            ret = 0; string DocName = "";

            try
            { 
                ArrayList al = new ArrayList(); 
                al.Add(this.DocID);
                al.Add(this.DocName);
                al.Add(Convert.ToDateTime(this.DocDate));
                al.Add(this.WareHouseId);
                al.Add(this.IsExpected);
                al.Add(this.CustomerId);
                al.Add(this.IsExpected ? this.EffectiveDate : null);
                al.Add(this.GridData);
                al.Add(this.Comments);
                al.Add(Convert.ToInt32(_LookUps.GetSessionObject("UserID")));
                DocName = _General.Set(al, "sp_InsertUpdateOutwardDoc", out ret, 0); 
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OutwardDocument, Method > SaveDocument(out int ret)", ex);
            }

            return DocName;
        }

        public long DeleteDocument(int DocID)
        {
            long flg = 0;
            try
            { 
                ArrayList al = new ArrayList(); 
                al.Add(DocID);
                al.Add(Convert.ToInt32(_LookUps.GetSessionObject("UserID")));

                _General.Set(al, "sp_DeleteOutwardStock", out flg, 0); 
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OutwardDocument, Method > DeleteDocument(int DocID)", ex);   
            }

            return flg;
        }
    }
}