using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Flortex.Inventory.Models
{
    public class OutwardDocument : IDocument
    {
        public int DocID { get; set; }
        public string DocName { get; set; }
        public string DocDate { get; set; }
        public string WareHouseId { get; set; }
        public string WareHouseName { get; set; }
        public bool IsExpected { get; set; }
        public string Customer { get; set; }
        public string EffectiveDate { get; set; }
        public string Comments { get; set; }
        public string GridData { get; set; }

        public OutwardDocument()
        {

        }

        public void init()
        {
            DocName = LookUps.GetDocName(3);// 3 stands for Outward documents.  
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

                        this.Customer = ds.Tables[0].Rows[0]["Customer"].ToString();
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
                PLog.Error("Error::Class > OutwardDocument, Method > EditDocument(int DocID)", ex);
            }
        }

        public DataSet GetGirdData(int DocID)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = DAL.GetDataSet("sp_GetOutwardDocData", new List<string>() { "@DocID" }, new ArrayList() { DocID });
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OutwardDocument, Method > GetGirdData(int DocID)", ex);
            }
            return ds; throw new NotImplementedException();
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
                objNames.Add("@IsExpected");
                objNames.Add("@Customer");
                objNames.Add("@EffectiveDate");
                objNames.Add("@Data");
                objNames.Add("@Comments");
                objNames.Add("@CreatedBy");

                al.Add(this.DocID);
                al.Add(this.DocName);
                al.Add(Convert.ToDateTime(this.DocDate));
                al.Add(this.WareHouseId);
                al.Add(this.IsExpected);
                al.Add(this.Customer);
                al.Add(this.IsExpected ? this.EffectiveDate : null);
                al.Add(this.GridData);
                al.Add(this.Comments);
                al.Add(Convert.ToInt32(LookUps.GetSessionObject("UserID")));
                DocName = DAL.ExecuteSP("sp_InsertUpdateOutwardDoc", objNames, al, out ret);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OutwardDocument, Method > SaveDocument(out int ret)", ex);
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

                flg = DAL.ExecuteSP("sp_DeleteOutwardStock", objNames, al);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OutwardDocument, Method > DeleteDocument(int DocID)", ex);   
            }

            return flg;
        }
    }
}