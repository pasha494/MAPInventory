using MAP.Inventory.Interface;
using MAP.Inventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;
using MAP.Inventory.DAL;
using MAP.Inventory.Logging;

namespace MAP.Inventory.ModelImple
{
    public class CustomerImple : ICustomerImple
    {
        General _General = new General();

        public long Delete(int CustomerID)
        {

            long flg = 0;

            try
            {
                _General.Set(new ArrayList() { CustomerID }, "sp_DeleteCustomer", out flg);
                // flg = DAL.ExecuteSP("sp_DeleteProducts", new List<string>() { "@NodeNo" }, new ArrayList() { ProductID });

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method > DeleteDocument(int DocID)", ex);
            }

            return flg;
        }

        public CustomerModel Edit(int ID)
        {
            CustomerModel objCustomerModel = new CustomerModel();
            try
            {
                DataTable dt = GetGridData(ID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    objCustomerModel.CustomerId = Convert.ToInt32(dt.Rows[0]["NODENO"].ToString());
                    objCustomerModel.IsCustomer =Convert.ToBoolean( dt.Rows[0]["IsCustomer"].ToString());
                    objCustomerModel.FirstName = dt.Rows[0]["FirstName"].ToString();
                    objCustomerModel.LastName = dt.Rows[0]["LastName"].ToString();
                    objCustomerModel.EmailId = dt.Rows[0]["EmailId"].ToString();
                    objCustomerModel.MobileNo =Convert.ToInt32( dt.Rows[0]["Mobile"].ToString());

                    objCustomerModel.PhoneNo = dt.Rows[0]["Phone"].ToString();
                    objCustomerModel.PAN = dt.Rows[0]["PAN"].ToString();
                    objCustomerModel.GSTIN = dt.Rows[0]["GSTIN"].ToString();
                    objCustomerModel.AdharNumber = dt.Rows[0]["AdharNumber"].ToString();

                    objCustomerModel.Address = dt.Rows[0]["Address"].ToString();
                    objCustomerModel.City = dt.Rows[0]["City"].ToString();
                    objCustomerModel.PinCode = Convert.ToInt32(dt.Rows[0]["PinCode"].ToString());
                    objCustomerModel.Notes = dt.Rows[0]["Notes"].ToString();

                    objCustomerModel.Extra1 = dt.Rows[0]["Extra1"].ToString();
                    objCustomerModel.Extra2 = dt.Rows[0]["Extra2"].ToString();
                    objCustomerModel.Extra3 = dt.Rows[0]["Extra3"].ToString();
                    objCustomerModel.Extra4 = dt.Rows[0]["Extra4"].ToString();
                    objCustomerModel.Extra5 = dt.Rows[0]["Extra5"].ToString();
                }

            }
            catch (Exception ex)
            {

            }
            return objCustomerModel;
        }

        public DataTable GetGridData(int CustomerID)
        {

            DataTable dt = null;

            try
            {
                DataSet ds = new DataSet();
                ds = _General.Get(new ArrayList() { CustomerID }, "sp_GetCustomerDetails");
                // ds = DAL.GetDataSet("sp_GetProductsData", new List<string>() { "ProductID" }, new ArrayList() { ProductID });
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > CustomerImple, Method > GetGridData(int ProductID)", ex);
            }

            return dt;
        }

        public long Save(CustomerModel _CustomerModel)
        {
            long flg = 0;
            try
            {
                ArrayList al = new ArrayList();
                al.Add(_CustomerModel.CustomerId);
                al.Add(_CustomerModel.IsCustomer);
                al.Add(_CustomerModel.FirstName);
                al.Add(_CustomerModel.LastName);
                al.Add(_CustomerModel.EmailId);
                al.Add(_CustomerModel.MobileNo);
                al.Add(_CustomerModel.PhoneNo);
                al.Add(_CustomerModel.PAN);
                al.Add(_CustomerModel.GSTIN);
                al.Add(_CustomerModel.AdharNumber);
                al.Add(_CustomerModel.Address);
                al.Add(_CustomerModel.City);
                al.Add(_CustomerModel.PinCode);
                al.Add(_CustomerModel.Notes);
                al.Add(_CustomerModel.Extra1);
                al.Add(_CustomerModel.Extra2);
                al.Add(_CustomerModel.Extra3);
                al.Add(_CustomerModel.Extra4);
                al.Add(_CustomerModel.Extra5);
                al.Add(0);

                _General.Set(al, "sp_InsertUpdateCustomer", out flg);


            }
            catch (Exception ex)
            {

            }
            return flg;
        }
    }
}
