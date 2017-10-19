using MAP.Inventory.Common;
using MAP.Inventory.Logging;
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

    //OpeningStock  
    public class OpeningStock : IDocument
    {

        public int DocID { get; set; }
        public string DocName { get; set; }
        public string DocDate { get; set; }
        public string WareHouseId { get; set; }
        public string GridData { get; set; }
        public string WareHouseName { get; set; }

        public string WareHouseOptions { get; set; }


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

        public OpeningStock()
        {
            WareHouseName = "";
        }

        public void init()
        {
            DocName = LookUps.GetDocName(1);// 1 stands for opening stocks.  
            GetWareHouseListViewOptions();

        }

        public void GetWareHouseListViewOptions()
        {
            MapListViewImple _wareHouseListView = new MapListViewImple((int)EnumListViews.WareHouses);
            this.WareHouseOptions = _wareHouseListView.GetListViewOptions();
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
                GetWareHouseListViewOptions();
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

                ds = DAL.GetDataSet("sp_GetOpeningStocksData", new List<string>() { "@DocID" }, new ArrayList() { DocID });

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OpeningStock, Method > GetGirdData(int DocID)", ex);
            }
            return ds;
        }

        public string SaveDocument(out int ret)
        {
            ret = 0; string DocName = "";

            try
            {
                List<string> objNames = new List<string>();
                ArrayList al = new ArrayList();
                objNames.Add("@DocID");
                objNames.Add("@DocName");
                objNames.Add("@DocDate");
                objNames.Add("@WarehouseID");
                objNames.Add("@Data");
                objNames.Add("@CreatedBy");

                al.Add(this.DocID);
                al.Add(this.DocName);
                al.Add(Convert.ToDateTime(this.DocDate));
                al.Add(this.WareHouseId);
                al.Add(this.GridData);
                al.Add(Convert.ToInt32(LookUps.GetSessionObject("UserID")));
                DocName = DAL.ExecuteSP("sp_InsertUpdateOpeningStock", objNames, al, out ret);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OpeningStock, Method > SaveDocument(out int ret)", ex);
            }

            return DocName;
        }

        public int DeleteDocument(int DocID)
        {
            int flg = 0;
            try
            {
                List<string> objNames = new List<string>();
                ArrayList al = new ArrayList();
                objNames.Add("@DocID");
                objNames.Add("@CreatedBy");

                al.Add(DocID);
                al.Add(Convert.ToInt32(LookUps.GetSessionObject("UserID")));

                flg = DAL.ExecuteSP("sp_DeleteOpeningStock", objNames, al);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OpeningStock, Method > DeleteDocument(int DocID)", ex);
            }

            return flg;
        }
    }
}