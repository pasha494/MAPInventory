using MAP.Inventory.Common.Controls;
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
    public class MapListViewImple : IMapListViewImple
    {
        General _General = new General();
        public MapListViewImple(int iFeatureId)
        {
            FeatureId = iFeatureId;
        }

        public int FeatureId { get; }

        private DataTable GetListViewCustomizaitonInfo()
        {
            DataTable dt = new DataTable();

            try
            {
                DataSet ds = _General.Get(new ArrayList(), "sp_GetListViewsCustomizationInfo");
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method > GetGridData(int ProductCategoryID)", ex);
            }

            return dt;
        }

        public List<ListViewCustomization> GetListViewsCustomizationInfo()
        {
            List<ListViewCustomization> listViews = new List<ListViewCustomization>();

            DataTable dt = GetListViewCustomizaitonInfo();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // create an instance for the listview customization class
                    ListViewCustomization objListViewCustomization = new ListViewCustomization(
                       _AcitonMethodName: dt.Rows[i]["ActionMethodName"].ToString(),
                       _ControllerName: dt.Rows[i]["ControllerName"].ToString(),
                       _FeatureId: Convert.ToInt32(dt.Rows[i]["NodeNo"].ToString()),
                       _Name: dt.Rows[i]["Name"].ToString(),
                       _Options: dt.Rows[i]["Options"].ToString(),
                       _TotalRows: Convert.ToInt32(dt.Rows[i]["TotalRows"].ToString())
                        );

                    // create an instance for easyui combogrid options 
                    //MapListView objMapListView = Newtonsoft.Json.JsonConvert.DeserializeObject<MapListView>(objListViewCustomization.Options);
                    //objListViewCustomization.SetMapListViewOptions(objMapListView);

                    listViews.Add( objListViewCustomization);
                }
            }
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
