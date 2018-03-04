using MAP.Inventory.DAL;
using MAP.Inventory.Interface;
using MAP.Inventory.Logging;
using MAP.Inventory.Model;
using System;
using System.Collections;
using System.Data;

namespace MAP.Inventory.ModelImple
{
    public class ProductsImple : IProductsImple
    {
        readonly General _general = new General();

        public long SaveProducts(ProductsModel productsModel)
        {
            long flg = 0;
            try
            {
                var al = new ArrayList
                {
                    productsModel.ProductID,
                    productsModel.Code,
                    productsModel.Name,
                    productsModel.ProductCategory,
                    productsModel.Status,
                    productsModel.efDate,
                    productsModel.Spec,
                    productsModel.Price
                };



                _general.Set(al, "sp_InsertUpdateProducts",out flg);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > OutwardDocument, Method >SaveProducts()", ex);
            }
            return flg;
        }

        string ConverDate(DateTime date)
        {
            string str;

            if (date != DateTime.MinValue || date!=DateTime.MaxValue)
            {
                str = date.Day + "-" + date.Month + "-" + date.Year;
            }
            else
            {
                str = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
            }

            return str;
        }

        public ProductsModel EditProdcut(int id)
        {
            var objProductsModel = new ProductsModel();
            try
            {
                DataTable dt = GetGridData(id);

                 if (dt != null && dt.Rows.Count > 0)
                {
                    objProductsModel.ProductID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    objProductsModel.Code = dt.Rows[0]["Code"].ToString();
                    objProductsModel.Name = dt.Rows[0]["Name"].ToString();
                    objProductsModel.ProductCategory = Convert.ToInt32(dt.Rows[0]["ProductCategoryID"].ToString());
                    objProductsModel.Status = Convert.ToInt32(dt.Rows[0]["Status"].ToString());
                    objProductsModel.efDate = ConverDate(dt.Rows[0]["eDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dt.Rows[0]["eDate"]));

                    objProductsModel.Spec = dt.Rows[0]["Spec"].ToString();

                    objProductsModel.Price = Convert.ToDecimal(dt.Rows[0]["Price"].ToString());
                }

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method >EditProdcut(int ID)", ex);
            }
            return objProductsModel;
        }

        public DataTable GetGridData(int productId)
        {
            DataTable dt = null;

            try
            {
                var ds = _general.Get(new ArrayList() { productId }, "sp_GetProductsData");
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method > GetGridData(int ProductID)", ex);
            }

            return dt;
        }

        public long DeleteProduct(int productId)
        {
            long flg = 0;

            try
            {
                 _general.Set(new ArrayList() { productId }, "sp_DeleteProducts", out flg);
               // flg = DAL.ExecuteSP("sp_DeleteProducts", new List<string>() { "@NodeNo" }, new ArrayList() { ProductID });

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method > DeleteDocument(int DocID)", ex);
            }

            return flg;
        }
    }
}
