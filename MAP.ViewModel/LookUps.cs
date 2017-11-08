using MAP.Inventory.DAL;
using MAP.Inventory.Logging;
using MAP.Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MAP.Inventory.ModelImple
{
    public class LookUps
    {
        General _General = new General();
        public DataTable GetProductCategies()
        {
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = new DataSet();
               // ds = DAL.GetDataSet(" Select * from ProductCategory");
                if (ds != null && ds.Tables.Count > 0)
                    dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > LookUps, Method > GetProductCategies()", ex);
            }

            return dt;
        }

        //public static DataTable GetWareHouses()
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        ds = DAL.GetDataSet(" Select * from Warehouses");
        //        if (ds != null && ds.Tables.Count > 0)
        //            dt = ds.Tables[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        PLog.Error("Error::Class > LookUps, Method > GetWareHouses()", ex);
        //    }
        //    return dt;
        //}

        public DataTable GetLookUps(int id)
        {
            DataTable dt = new DataTable();

            try
            {
                DataSet ds = new DataSet();
                ds = _General.Get(new ArrayList() { id }, "sp_GetLookUpData", 0);
                if (ds != null && ds.Tables.Count > 0)
                    dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > LookUps, Method > GetLookUps(int id)", ex);
            }

            return dt;
        }

        public string GetDocName(int DocType)
        {
            string str = "";
            try
            {
                DataSet ds = new DataSet();
                ds = _General.Get(  new ArrayList() { DocType },"sp_GetDocName",0);

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

        public DataSet GetDocumentAlerts(bool ReOrdQty, bool BlckQty)
        {
            DataSet ds = new DataSet();
            try
            { 
                ArrayList al = new ArrayList();
                al.Add(ReOrdQty);
                al.Add(BlckQty);

                ds = _General.Get(al, "sp_GetAlertDocs", 0); 

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > LookUps, Method > GetDocumentAlerts()", ex);
            }

            return ds;
        }

        public object GetSessionObject(string Key)
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