using MAP.Inventory.DAL;
using MAP.Inventory.Interface;
using MAP.Inventory.Logging;
using MAP.Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.ModelImple
{
    public class ProductsCategoryImple : IProductsCategoryImple
    {
        General _General = new General();

        public long SaveProductCategory(ProductsCategoryModel _ProductsCategoryModel)
        {
            long flg = 0;
            try
            { 
                ArrayList al = new ArrayList();
                 
                al.Add(_ProductsCategoryModel.ProductCategoryID);
                al.Add(_ProductsCategoryModel.Name);
                _General.Set(al, "sp_InsertUpdateProductsCategory",out flg);
               // flg = DAL.ExecuteSP("sp_InsertUpdateProductsCategory", Params, al);

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > ProductsCategory, Method >SaveProductCategory()", ex);
            }
            return flg;
        }

        public ProductsCategoryModel EditProdcutCategory(int ID)
        {
            ProductsCategoryModel _ProductsCategoryModel = new ProductsCategoryModel();
            try
            {
                DataTable dt = GetGridData(ID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    _ProductsCategoryModel.ProductCategoryID = Convert.ToInt32(dt.Rows[0]["id"].ToString());
                    _ProductsCategoryModel.Name = dt.Rows[0]["Name"].ToString();
                }

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method >EditProdcut(int ID)", ex);
            }

            return _ProductsCategoryModel;
        }

        public DataTable GetGridData(int ProductCategoryID)
        {
            DataTable dt = null;

            try
            {
                DataSet ds = new DataSet();
                ds = _General.Get(new ArrayList() { ProductCategoryID }, "sp_GetProductsCategoryData"); 
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method > GetGridData(int ProductCategoryID)", ex);
            }

            return dt;
        }

        public long DeleteProductsCategory(int ProductCategoryID)
        {
            long flg = 0;

            try
            {
                _General.Set(new ArrayList() { ProductCategoryID }, "sp_DeleteProductsCategory", out flg); 

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method > DeleteProductsCategory(int DocID)", ex);
            }

            return flg;
        }
    }
}
