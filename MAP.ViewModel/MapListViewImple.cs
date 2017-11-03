using MAP.Inventory.Common;
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
                DataSet ds = _General.Get(new ArrayList() { this.FeatureId }, "sp_GetListViewsCustomizationInfo");
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
                       _ActionMethodName: dt.Rows[i]["ActionMethodName"].ToString(),
                       _ControllerName: dt.Rows[i]["ControllerName"].ToString(),
                       _FeatureId: Convert.ToInt32(dt.Rows[i]["NodeNo"].ToString()),
                       _Name: dt.Rows[i]["Name"].ToString(),
                       _Options: dt.Rows[i]["Options"].ToString(),
                       _TotalRows: Convert.ToInt32(dt.Rows[i]["TotalRows"].ToString())
                        );

                    // create an instance for easyui combogrid options 
                    //MapListView objMapListView = Newtonsoft.Json.JsonConvert.DeserializeObject<MapListView>(objListViewCustomization.Options);
                    //objListViewCustomization.SetMapListViewOptions(objMapListView);

                    listViews.Add(objListViewCustomization);
                }
            }
            return listViews;
        }

        #region Fetch ListViews dynamic data
        public DataTable GetProductsListViewData(string wareHouseId, string FilterCol, string FilterValue)
        {
            DataTable dt = null;

            try
            {
                DataSet ds = new DataSet();
                string spName = getSPName();
                if (!string.IsNullOrWhiteSpace(spName))
                {
                    ds = _General.Get(new ArrayList() { Convert.ToInt32(wareHouseId), FilterCol, FilterValue }, spName);
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method > GetGridData(int ProductCategoryID)", ex);
            }

            return dt;
        }


        public DataTable GetWareHousesListViewData(string FilterCol, string FilterValue)
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

        public DataTable GetCustomerListViewData(int Type, string FilterCol, string FilterValue)
        {
            DataTable dt = null;

            try
            {
                DataSet ds = new DataSet();
                string spName = getSPName();
                if (!string.IsNullOrWhiteSpace(spName))
                {
                    ds = _General.Get(new ArrayList() { Type, FilterCol, FilterValue }, spName);
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
            if (FeatureId == Convert.ToInt32(EnumListViews.Products))
                return "sp_GetProductsAutoCompleteListData";
            else if (FeatureId == Convert.ToInt32(EnumListViews.WareHouses))
                return "sp_GetWarehouseAutoCompleteListData";
            else if (FeatureId == Convert.ToInt32(EnumListViews.Customers) || FeatureId == Convert.ToInt32(EnumListViews.Vendors))
                return "sp_GetCustomerAutoCompleteListData";

            return "";
        }

        #endregion

        public long SaveListViewCustomizationInfo(ListViewCustomization model)
        {
            long flg = 0;
            ArrayList al = new ArrayList();
            al.Add(model.FeatureId);
            al.Add(model.TotalRows);
            al.Add(model.Options);
            al.Add(0);
            //al.Add(Convert.ToInt32(LookUps.GetSessionObject("UserID")));

            _General.Set(al, "sp_UpdateListViewCustomizationInfo", out flg);

            return flg;
        }

        public string GetListViewOptions()
        {
            string options = string.Empty;
            try
            {
                DataTable dt = GetListViewCustomizaitonInfo();
                if (dt != null && dt.Rows.Count > 0)
                    options = dt.Rows[0]["options"].ToString();
            }
            catch (Exception ex)
            {

            }

            return options;
        }
    }
}
