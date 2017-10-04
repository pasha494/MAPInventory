using MAP.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MAP.Inventory.Models
{
    public static class LookUps
    {
        public static DataTable GetProductCategies()
        {
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();
                ds = DAL.GetDataSet(" Select * from ProductCategory");
                if (ds != null && ds.Tables.Count > 0)
                    dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > LookUps, Method > GetProductCategies()", ex);
            }

            return dt;
        }

        public static DataTable GetWareHouses()
        {
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();
                ds = DAL.GetDataSet(" Select * from Warehouses");
                if (ds != null && ds.Tables.Count > 0)
                    dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > LookUps, Method > GetWareHouses()", ex);
            }
            return dt;
        }

        public static DataTable GetLookUps(int id)
        {
            DataTable dt = new DataTable();

            try
            {
                DataSet ds = new DataSet();
                ds = DAL.GetDataSet(" select NodeNo,Name from Lookups where LookUpMapId=" + id);
                if (ds != null && ds.Tables.Count > 0)
                    dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > LookUps, Method > GetLookUps(int id)", ex);
            }

            return dt;
        }

        public static string GetDocName(int DocType)
        {
            string str = "";
            try
            {
                DataSet ds = new DataSet();
                ds = DAL.GetDataSet("sp_GetDocName", new List<string>() { "DocType" }, new ArrayList() { DocType });

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    str = ds.Tables[0].Rows[0][0].ToString();
                }


            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > LookUps, Method > GetDocName(int DocType)", ex);
            }
            return str;
        }

        public static DataSet GetDocumentAlerts(bool ReOrdQty, bool BlckQty)
        {
            DataSet ds = new DataSet();
            try
            {
                List<string> list = new List<string>();
                list.Add("@ReOrdQty");
                list.Add("@BlckQty");

                ArrayList al = new ArrayList();
                al.Add(ReOrdQty);
                al.Add(BlckQty);

                ds = DAL.GetDataSet("sp_GetAlertDocs", list, al);

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > LookUps, Method > GetDocumentAlerts()", ex);
            }

            return ds;
        }

        public static object GetSessionObject(string Key)
        {
            SessionManager objSession = null;
            if (HttpContext.Current.Session["SessionManager"] != null)
            {
                objSession = (SessionManager)HttpContext.Current.Session["SessionManager"];

                switch (Key)
                {
                    case "UserID":
                        return objSession.UserID;
                    case "UserName":
                        return objSession.UserName;
                    case "Email":
                        return objSession.Email;
                    case "IsAdmin":
                        return objSession.IsAdmin;
                    case "RoleID":
                        return objSession.RoleID;
                    default:
                        return "";
                }
            }
            return "";
        }

    }
}