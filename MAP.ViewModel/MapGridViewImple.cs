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
    public class MapGridViewImple : IMapGridViewImple
    {
        General _General = new General();
        public MapGridViewImple(int iFeatureId)
        {
            FeatureId = iFeatureId;
        }

        public int FeatureId { get; }

        private DataTable GetGridViewCustomizaitonInfo()
        {
            DataTable dt = new DataTable();

            try
            {
                DataSet ds = _General.Get(new ArrayList() { this.FeatureId }, "sp_GetGridCustomizationInfo");
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Products, Method > GetGridData(int ProductCategoryID)", ex);
            }

            return dt;
        }


        public Dictionary<int, string> GetGridViewNames()
        {
            Dictionary<int, string> dicGridViewsList = new Dictionary<int, string>();
            DataTable dt = GetGridViewCustomizaitonInfo();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dicGridViewsList.Add(Convert.ToInt32(dt.Rows[i]["NodeNo"].ToString()), dt.Rows[i]["Name"].ToString());
                } 
            }

            return dicGridViewsList;
        }

        public GridViewCustomization GetGridViewsCustomizationInfo()
        {
            GridViewCustomization gridView = new GridViewCustomization();
            DataTable dt = GetGridViewCustomizaitonInfo();

            if (dt != null && dt.Rows.Count > 0)
            {
                gridView.Name = dt.Rows[0]["Name"].ToString();
                gridView.TotalRows = Convert.ToInt32(dt.Rows[0]["TotalRows"].ToString());
                gridView.Columns = dt.Rows[0]["Columns"].ToString();
                gridView.Height = Convert.ToDouble(dt.Rows[0]["Height"].ToString());
            }
            return gridView;
        }

        #region Fetch ListViews dynamic data
        //public DataTable GetProductsListViewData(string wareHouseId, string FilterCol, string FilterValue)
        //{
        //    DataTable dt = null;

        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        string spName = getSPName();
        //        if (!string.IsNullOrWhiteSpace(spName))
        //        {
        //            ds = _General.Get(new ArrayList() { Convert.ToInt32(wareHouseId), FilterCol, FilterValue }, spName);
        //            dt = ds.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        PLog.Error("Error::Class > Products, Method > GetGridData(int ProductCategoryID)", ex);
        //    }

        //    return dt;
        //}


        //public DataTable GetWareHousesListViewData(string FilterCol, string FilterValue)
        //{
        //    DataTable dt = null;

        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        string spName = getSPName();
        //        if (!string.IsNullOrWhiteSpace(spName))
        //        {
        //            ds = _General.Get(new ArrayList() { FilterCol, FilterValue }, spName);
        //            dt = ds.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        PLog.Error("Error::Class > Products, Method > GetGridData(int ProductCategoryID)", ex);
        //    }

        //    return dt;
        //}


        //private string getSPName()
        //{
        //    if (FeatureId == Convert.ToInt32(EnumListViews.Products))
        //        return "sp_GetProductsAutoCompleteListData";
        //    else if (FeatureId == Convert.ToInt32(EnumListViews.WareHouses))
        //        return "sp_GetWarehouseAutoCompleteListData";

        //    return "";
        //}

        #endregion

        //public long SaveListViewCustomizationInfo(ListViewCustomization model)
        //{
        //    long flg = 0;
        //    ArrayList al = new ArrayList();
        //    al.Add(model.FeatureId);
        //    al.Add(model.TotalRows);
        //    al.Add(model.Options);
        //    al.Add(0);
        //    //al.Add(Convert.ToInt32(LookUps.GetSessionObject("UserID")));

        //    _General.Set(al, "sp_UpdateListViewCustomizationInfo", out flg);

        //    return flg;
        //}

        //public string GetListViewOptions()
        //{
        //    string options = string.Empty;
        //    try
        //    {
        //        DataTable dt = GetListViewCustomizaitonInfo();
        //        if (dt != null && dt.Rows.Count > 0)
        //            options = dt.Rows[0]["options"].ToString();
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return options;
        //}
    }
}
