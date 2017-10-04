using MAP.Logging;
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
    public class ProductsCategory
    {

        public int ProductCategoryID { get; set; }
        public string Name { get; set; }    

        public ProductsCategory()
        {
            ProductCategoryID = 0; 
        }

        public int SaveProductCategory()
        {
            int flg = 0;
            try
            {
                List<string> Params = new List<string>();
                ArrayList al = new ArrayList();

                Params.Add("@NodeNo");
                Params.Add("@Name"); 

                al.Add(ProductCategoryID);
                al.Add(Name); 

                flg = DAL.ExecuteSP("sp_InsertUpdateProductsCategory", Params, al);

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > ProductsCategory, Method >SaveProductCategory()", ex);
            }
            return flg;
        }

        public void EditProdcutCategory(int ID)
        {

            try
            {
                DataTable dt = GetGridData(ID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.ProductCategoryID = Convert.ToInt32(dt.Rows[0]["id"].ToString()); 
                    this.Name = dt.Rows[0]["Name"].ToString(); 
                }

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method >EditProdcut(int ID)", ex);
            }
        }

        public DataTable GetGridData(int ProductCategoryID)
        {
            DataTable dt = null;

            try
            {
                DataSet ds = new DataSet();
                ds = DAL.GetDataSet("sp_GetProductsCategoryData", new List<string>() { "ProductCategoryID" }, new ArrayList() { ProductCategoryID });
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method > GetGridData(int ProductCategoryID)", ex);
            }

            return dt;
        }

        public int DeleteProductsCategory(int ProductCategoryID)
        {
            int flg = 0;
            
            try
            {

                flg = DAL.ExecuteSP("sp_DeleteProductsCategory", new List<string>() { "@NodeNo" }, new ArrayList() { ProductCategoryID });
               
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method > DeleteProductsCategory(int DocID)", ex);
            } 

            return flg;
        }

    }
}
