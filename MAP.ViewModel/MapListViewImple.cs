using MAP.Inventory.Common.Controls;
using MAP.Inventory.DAL;
using MAP.Inventory.Interface;
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
    public class MapListViewImple : IMapListViewImple
    {
        General _General = new General();
        public MapListViewImple(int iFeatureId)
        {
            FeatureId = iFeatureId;
        }


        public int FeatureId { get; }

        private DataTable GetListViewOptions()
        {
            DataTable dt = new DataTable();
            return dt;
        }


        public Dictionary<int, MapListView> GetListViewsCustomizationInfo()
        {
            Dictionary<int, MapListView> listViews = new Dictionary<int, MapListView>();
             

            return listViews;
        }

        #region Fetch ListViews dynamic data
        public DataTable GetProductsListViewData(string FilterCol, string FilterValue)
        {
            DataTable dt = null;

            try
            {
                DataSet ds = new DataSet();
                string spName = getSPName();
                if (!string.IsNullOrWhiteSpace(spName))
                {
                    ds = _General.Get(new ArrayList() { FilterCol, FilterValue }, spName);
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method > GetGridData(int ProductCategoryID)", ex);
            }

            return dt;
        }


        private string getSPName()
        {
            if (FeatureId == 1)
                return "sp_GetProductsAutoCompleteListData";

            return "";
        }

        #endregion

    }
}
