using MAP.Inventory.Common;
using MAP.Inventory.DAL;
using MAP.Inventory.Interface;
using MAP.Inventory.Logging;
using MAP.Inventory.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.ModelImple
{
    public class OpeningStockImple : IDocument
    {
        LookUps _LookUps = new LookUps();
        General _General = new General();

        public int DocID { get; set; }
        public string DocName { get; set; }
        public string DocDate { get; set; }
        public string WareHouseId { get; set; }
        public string GridData { get; set; }
        public string WareHouseName { get; set; }

        public string WareHouseOptions { get; set; }
        public string ProductsOptions { get; set; }

        public GridViewCustomization GridView { get; set; }


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

        public OpeningStockImple()
        {
            WareHouseName = "";
            GridView = new GridViewCustomization();
        }

        public void init()
        {
            DocName = _LookUps.GetDocName((int)EnumGridView.OpeningStock);// 1 stands for opening stocks.  
            LoadScreenControls();

        }

        void LoadScreenControls()
        {
            GetWareHouseListViewOptions();
            GetProductsListViewOptions();
            GetDocumentsGridViewOptions();
        }

        public void GetWareHouseListViewOptions()
        {
            MapListViewImple _wareHouseListView = new MapListViewImple((int)EnumListViews.WareHouses);
            this.WareHouseOptions = _wareHouseListView.GetListViewOptions();
        }

        public void GetProductsListViewOptions()
        {
            MapListViewImple _productsListView = new MapListViewImple((int)EnumListViews.Products);
            this.ProductsOptions = _productsListView.GetListViewOptions();
        }

        public void GetDocumentsGridViewOptions()
        {
            MapGridViewImple _productsListView = new MapGridViewImple((int)EnumGridView.OpeningStock);
            this.GridView = _productsListView.GetGridViewsCustomizationInfo(DocID);
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
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        this.GridData = JsonConvert.SerializeObject(ds.Tables[1]);
                    }
                }
                LoadScreenControls();
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OpeningStock, Method > EditDocument(int DocID)", ex);
            }
        }

        public DataSet GetGirdData(int DocID)
        {
            DataSet ds = new DataSet();
            try
            {

                ds = _General.Get(new ArrayList() { DocID }, "sp_GetOpeningStocksData", 0);

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OpeningStock, Method > GetGirdData(int DocID)", ex);
            }
            return ds;
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
                al.Add(this.GridData);
                al.Add(Convert.ToInt32(_LookUps.GetSessionObject("UserID")));
                DocName = _General.Set(al, "sp_InsertUpdateOpeningStock", out ret, 0);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OpeningStock, Method > SaveDocument(out int ret)", ex);
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

                _General.Set(al, "sp_DeleteOpeningStock", out flg, 0);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OpeningStock, Method > DeleteDocument(int DocID)", ex);
            }

            return flg;
        }

    }
}
