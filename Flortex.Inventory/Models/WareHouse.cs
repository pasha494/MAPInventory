using MAP.Inventory.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace MAP.Inventory.Models
{
    public class WareHouse
    {

        public int WareHouseID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; } 
        public int Status { get; set; }
        public string efDate { get; set; }

        public WareHouse()
        {
            WareHouseID = 0; 
            Status = 0;
        }

        public int SaveWareHouse()
        {
            int flg = 0;
            try
            {
                List<string> Params = new List<string>();
                ArrayList al = new ArrayList();

                Params.Add("@NodeNo");
                Params.Add("@Code");
                Params.Add("@Name");
                Params.Add("@Status");
                Params.Add("@eDate");

                al.Add(WareHouseID);
                al.Add(Code);
                al.Add(Name);
                al.Add(Status);
                al.Add(efDate);

                flg = DAL.ExecuteSP("sp_InsertUpdateWareHouse", Params, al);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > WareHouse, Method >SaveWareHouse()", ex);
            }

            return flg;
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

        public void EditWareHouse(int ID)
        { 
            try
            {
                DataTable dt = GetGridData(ID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.WareHouseID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    this.Code = dt.Rows[0]["Code"].ToString();
                    this.Name = dt.Rows[0]["Name"].ToString(); 
                    this.Status = Convert.ToInt32(dt.Rows[0]["Status"].ToString());
                    if (dt.Rows[0]["eDate"] == DBNull.Value)
                        this.efDate = ConverDate(DateTime.Now);
                    else
                        this.efDate = ConverDate(Convert.ToDateTime(dt.Rows[0]["eDate"]));
                }

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > WareHouse, Method > EditWareHouse(int ID)", ex);
            }
        }

        public DataTable GetGridData(int WareHouseID)
        {
            DataTable dt = null;

            try
            {
                DataSet ds = new DataSet();
                ds = DAL.GetDataSet("sp_GetWareHouseData", new List<string>() { "@WareHouseID" }, new ArrayList() { WareHouseID });
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > WareHouse, Method > GetGridData(int WareHouseID)", ex);
            }

            return dt;
        }

        public int DeleteWareHouse(int ProductID)
        {
            int flg = 0;

            try
            {

                flg = DAL.ExecuteSP("sp_DeleteWareHouse ", new List<string>() { "@NodeNo" }, new ArrayList() { ProductID });

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > WareHouse, Method > DeleteWareHouse(int ProductID)", ex);
            }

            return flg;
        }
    }
}
