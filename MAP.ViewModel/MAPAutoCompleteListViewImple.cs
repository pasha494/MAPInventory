using MAP.Inventory.DAL;
using MAP.Inventory.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.ModelImple
{
    public class MAPAutoCompleteListViewImple
    {


        General _General = new General();

        public DataTable GetProductsListViewData(string FilterCol,string FilterValue)
        {
            DataTable dt = null;

            try
            {
                DataSet ds = new DataSet();
                ds = _General.Get(new ArrayList() { FilterCol, FilterValue }, "sp_GetProductsAutoCompleteListData");
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method > GetGridData(int ProductCategoryID)", ex);
            }

            return dt;
        }

    }
}
