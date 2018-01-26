using MAP.Inventory.DAL;
using MAP.Inventory.Interface;
using MAP.Inventory.Logging;
using MAP.Inventory.Model;
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.ModelImple
{
    public class ProductPriceMapImple : IProductsImple
    {
        General _General = new General();

        public long SaveProducts(ProductsModel _ProductsModel)
        {
            long flg = 0;
            try
            { 
                ArrayList al = new ArrayList();
                 

                al.Add(_ProductsModel.ProductID);
                al.Add(_ProductsModel.Code);
                al.Add(_ProductsModel.Name);
                al.Add(_ProductsModel.ProductCategory);
                al.Add(_ProductsModel.Status);
                al.Add(_ProductsModel.efDate);
                al.Add(_ProductsModel.Spec);

                _General.Set(al, "sp_InsertUpdateProducts",out flg);
                ///flg = DAL.ExecuteSP("sp_InsertUpdateProducts", Params, al);

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

        public ProductsModel EditProdcut(int ID)
        {
            ProductsModel objProductsModel = new ProductsModel();
            try
            {
                DataTable dt = GetGridData(ID);

                 if (dt != null && dt.Rows.Count > 0)
                {
                    objProductsModel.ProductID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    objProductsModel.Code = dt.Rows[0]["Code"].ToString();
                    objProductsModel.Name = dt.Rows[0]["Name"].ToString();
                    objProductsModel.ProductCategory = Convert.ToInt32(dt.Rows[0]["ProductCategoryID"].ToString());
                    objProductsModel.Status = Convert.ToInt32(dt.Rows[0]["Status"].ToString());
                    if (dt.Rows[0]["eDate"] == DBNull.Value)
                        objProductsModel.efDate = ConverDate(DateTime.Now);
                    else
                        objProductsModel.efDate = ConverDate(Convert.ToDateTime(dt.Rows[0]["eDate"]));

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

        public DataTable GetGridData(int ProductID)
        {
            DataTable dt = null;

            try
            {
                DataSet ds = new DataSet();
                ds = _General.Get(new ArrayList() { ProductID }, "sp_GetProductsData");
               // ds = DAL.GetDataSet("sp_GetProductsData", new List<string>() { "ProductID" }, new ArrayList() { ProductID });
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method > GetGridData(int ProductID)", ex);
            }

            return dt;
        }

        public long DeleteProduct(int ProductID)
        {
            long flg = 0;

            try
            {
                 _General.Set(new ArrayList() { ProductID }, "sp_DeleteProducts", out flg);
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
