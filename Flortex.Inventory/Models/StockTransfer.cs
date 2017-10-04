using MAP.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MAP.Inventory.Models
{

    //StockTransfer  
    public class StockTransfer : IDocument
    {

        public int DocID { get; set; }
        public string DocName { get; set; }
        public string DocDate { get; set; }
        public string FromWareHouseId { get; set; }
        public string FromWareHouseName { get; set; }
        public string ToWareHouseId { get; set; }
        public string ToWareHouseName { get; set; }
        public string Comments { get; set; }
        public string GridData { get; set; }
        
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

        public StockTransfer()
        {

        }

        public void init()
        {
            DocName = LookUps.GetDocName(4);// 1 stands for stock transfer.  
        }
        
        public void EditDocument(int DocID)
        {
            try
            {
                DataSet ds = GetGirdData(DocID);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        this.DocID = DocID;
                        this.DocName = ds.Tables[0].Rows[0]["DocName"].ToString();
                        this.DocDate = ConverDate(Convert.ToDateTime(ds.Tables[0].Rows[0]["DocDate"].ToString()));
                        this.FromWareHouseId = ds.Tables[0].Rows[0]["FromWareHouseId"].ToString();
                        this.FromWareHouseName = ds.Tables[0].Rows[0]["FromWareHouseName"].ToString();
                        this.ToWareHouseId = ds.Tables[0].Rows[0]["ToWareHouseId"].ToString();
                        this.ToWareHouseName = ds.Tables[0].Rows[0]["ToWareHouseName"].ToString();
                        this.Comments = ds.Tables[0].Rows[0]["Comments"].ToString();
                    }

                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        this.GridData = JsonConvert.SerializeObject(ds.Tables[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > StockTransfer, Method >EditDocument(int DocID)", ex);
            }
        }

        public DataSet GetGirdData(int DocID)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = DAL.GetDataSet("sp_GetStockTransferData", new List<string>() { "@DocID" }, new ArrayList() { DocID });
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > StockTransfer, Method >GetGirdData(int DocID)", ex);
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
                objNames.Add("@ToWareHouseID");
                objNames.Add("@FromWareHouseID");
                objNames.Add("@Comments");
                objNames.Add("@Data");
                objNames.Add("@CreatedBy");

                al.Add(this.DocID);
                al.Add(this.DocName);
                al.Add(Convert.ToDateTime(this.DocDate));
                al.Add(this.ToWareHouseId);
                al.Add(this.FromWareHouseId);
                al.Add(this.Comments);
                al.Add(this.GridData);
                al.Add(Convert.ToInt32(LookUps.GetSessionObject("UserID")));

                DocName = DAL.ExecuteSP("sp_InsertUpdateStockTransfer", objNames, al, out ret);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > StockTransfer, Method >SaveDocument(out int ret)", ex);
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

                flg = DAL.ExecuteSP("sp_DeleteStockTransfer", objNames, al);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > StockTransfer, Method > DeleteDocument(int DocID)", ex);
            }

            return flg;
        }
    }
}