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
    public class Products
    {

        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ProductCategory { get; set; }
        public int Status { get; set; }
        public string efDate { get; set; }
        public string Spec { get; set; }

        public Products()
        {
            ProductID = 0;
            ProductCategory = 0;
            Status = 0;
        }

        public int SaveProducts()
        {
            int flg = 0;
            try
            {
                List<string> Params = new List<string>();
                ArrayList al = new ArrayList();

                Params.Add("@NodeNo");
                Params.Add("@Code");
                Params.Add("@Name");
                Params.Add("@Category");
                Params.Add("@Status");
                Params.Add("@eDate");
                Params.Add("@Spec");

                al.Add(ProductID);
                al.Add(Code);
                al.Add(Name);
                al.Add(ProductCategory);
                al.Add(Status);
                al.Add(efDate);
                al.Add(Spec);

                flg = DAL.ExecuteSP("sp_InsertUpdateProducts", Params, al);

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OutwardDocument, Method >SaveProducts()", ex);
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

        public void EditProdcut(int ID)
        {

            try
            {
                DataTable dt = GetGridData(ID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.ProductID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    this.Code = dt.Rows[0]["Code"].ToString();
                    this.Name = dt.Rows[0]["Name"].ToString();
                    this.ProductCategory = Convert.ToInt32(dt.Rows[0]["ProductCategoryID"].ToString());
                    this.Status = Convert.ToInt32(dt.Rows[0]["Status"].ToString());
                    if (dt.Rows[0]["eDate"] == DBNull.Value)
                        this.efDate = ConverDate(DateTime.Now);
                    else
                        this.efDate = ConverDate(Convert.ToDateTime(dt.Rows[0]["eDate"]));

                    this.Spec = dt.Rows[0]["Spec"].ToString();
                }

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method >EditProdcut(int ID)", ex);
            }
        }

        public DataTable GetGridData(int ProductID)
        {
            DataTable dt = null;

            try
            {
                DataSet ds = new DataSet();
                ds = DAL.GetDataSet("sp_GetProductsData", new List<string>() { "ProductID" }, new ArrayList() { ProductID });
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method > GetGridData(int ProductID)", ex);
            }

            return dt;
        }

        public int DeleteProduct(int ProductID)
        {
            int flg = 0;
            
            try
            {

                flg = DAL.ExecuteSP("sp_DeleteProducts", new List<string>() { "@NodeNo" }, new ArrayList() { ProductID });
               
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method > DeleteDocument(int DocID)", ex);
            } 

            return flg;
        }

    }
}
