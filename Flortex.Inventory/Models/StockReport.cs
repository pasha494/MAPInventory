using MAP.Inventory.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MAP.Inventory.Web.Models
{
    public class StockReport
    {

        public DataSet ds { get; set; }

        public DataTable dt { get; set; }

        public StockReport()
        {

        }

        List<int> listProducts = new List<int>();

        public void GetStockReportData()
        {
            try
            {
                this.ds = DAL.GetDataSet("Sp_GetStockReportData", new List<string>(), new System.Collections.ArrayList());
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > StockReport, Method > GetStockReportData()", ex);
            }

        }

        public void GenerateReportData()
        {
            try
            {

                if (ds != null && ds.Tables.Count > 0)
                {
                    CreateDataTable();

                    //-----------Insert data in the 

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        int ProductID = Convert.ToInt32(ds.Tables[1].Rows[i]["ProductID"]);
                        if (!listProducts.Contains(ProductID))
                        {
                            DataRow dr = dt.NewRow();
                            dr["ProductID"] = ProductID;
                            dr["ProductName"] = ds.Tables[1].Rows[i]["ProductName"].ToString();
                            dr["ProductCode"] = ds.Tables[1].Rows[i]["ProductCode"].ToString();
                            dr["CategoryID"] = Convert.ToInt32(ds.Tables[1].Rows[i]["CategoryID"]);
                            dr["CategoryName"] = ds.Tables[1].Rows[i]["CategoryName"].ToString();
                            dr["Spec"] = ds.Tables[1].Rows[i]["Spec"].ToString();

                            double TotalQty = 0;
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                double TotalQtyLoc = 0;
                                int LocID = Convert.ToInt32(ds.Tables[0].Rows[j]["ID"].ToString());
                                //-----------Actual Quantity-----------//
                                DataRow drAct = ds.Tables[1].Select("ProductID=" + ProductID + " and LocationID=" + LocID).FirstOrDefault();
                                double ActQty = 0;

                                if (drAct != null && drAct["Quantity"] != null)
                                    ActQty = Convert.ToDouble(drAct["Quantity"].ToString());

                                dr["Act_Qty_" + LocID] =Convert.ToDouble(ActQty.ToString("0.00"));

                                TotalQty += ActQty;
                                TotalQtyLoc = ActQty;

                                //-----------Blocked Quantity-----------//
                                DataRow[] drBlck1 = ds.Tables[2].Select("ProductID=" + ProductID + " and WarehouseID=" + LocID);
                                if (drBlck1 != null && drBlck1.Length > 0)
                                {
                                    double Qty = 0;
                                    foreach (DataRow drBlck in drBlck1)
                                    {
                                        Qty += Convert.ToDouble(drBlck["Quantity"].ToString());
                                    }
                                    TotalQty -= Qty;
                                    TotalQtyLoc -= Qty;
                                    if (drBlck1.Length > 1)
                                        dr["Blck_Qty_Exp_" + LocID] = 1;
                                    else
                                        dr["Blck_Qty_Exp_" + LocID] = 0;

                                    dr["Blck_Qty_" + LocID] = Convert.ToDouble(Qty.ToString("0.00")); //Qty;
                                }
                                else
                                {
                                    dr["Blck_Qty_" + LocID] = 0;
                                    dr["Blck_Qty_Exp_" + LocID] = 0;
                                }

                                dr["Total_Qty_" + LocID] = Convert.ToDouble(TotalQtyLoc.ToString("0.00")); 

                                //-----------ReOrder Quantity-----------//
                                DataRow[] drReOrd1 = ds.Tables[3].Select("ProductID=" + ProductID + " and WarehouseID=" + LocID);
                                if (drReOrd1 != null && drReOrd1.Length > 0)
                                {
                                    double Qty = 0;
                                    string ExpDates = "";
                                    foreach (DataRow drReOrd in drReOrd1)
                                    {
                                        Qty += Convert.ToDouble(drReOrd["Quantity"].ToString());

                                        if (drReOrd["efDate"] != null && !string.IsNullOrEmpty(drReOrd["efDate"].ToString()))
                                            ExpDates += Convert.ToDateTime(drReOrd["efDate"].ToString()).ToString("dd/MM/yyyy");
                                    }
                                    dr["ReOrd_Qty_" + LocID] = Convert.ToDouble(Qty.ToString("0.00")); //Qty;
                                    dr["ReOrd_ExpDt_" + LocID] = ExpDates.Length > 10 ? ExpDates.Substring(0, 10) + "...," : ExpDates;

                                }
                                else
                                {
                                    dr["ReOrd_Qty_" + LocID] = 0;
                                    dr["ReOrd_ExpDt_" + LocID] = "";
                                }
                            }

                            dr["Total_Qty"] = Convert.ToDouble(TotalQty.ToString("0.00"));//TotalQty;
                            listProducts.Add(ProductID);
                            this.dt.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > StockReport, Method >GenerateReportData()", ex);
            }
        }

        private void CreateDataTable()
        {
            try
            {
                dt = new DataTable();
                dt.Columns.Add("ProductID", typeof(Int32));
                dt.Columns.Add("ProductName", typeof(String));
                dt.Columns.Add("ProductCode", typeof(String));
                dt.Columns.Add("CategoryID", typeof(Int32));
                dt.Columns.Add("CategoryName", typeof(String));
                dt.Columns.Add("Spec", typeof(String));
                dt.Columns.Add("Total_Qty", typeof(Double));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dt.Columns.Add("Act_Qty_" + ds.Tables[0].Rows[i]["id"].ToString(), typeof(Double)); 
                    dt.Columns.Add("Blck_Qty_" + ds.Tables[0].Rows[i]["id"].ToString(), typeof(Double));

                    dt.Columns.Add("Total_Qty_" + ds.Tables[0].Rows[i]["id"].ToString(), typeof(Double));

                    dt.Columns.Add("Blck_Qty_Exp_" + ds.Tables[0].Rows[i]["id"].ToString(), typeof(Int32)); 
                    dt.Columns.Add("ReOrd_Qty_" + ds.Tables[0].Rows[i]["id"].ToString(), typeof(Double));
                    dt.Columns.Add("ReOrd_ExpDt_" + ds.Tables[0].Rows[i]["id"].ToString(), typeof(String));
                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > StockReport, Method >CreateDataTable()", ex);
            }
        }

        public DataSet GetReOrdQtyHistory(int ProductID, int WarehouseID)
        {
            DataSet ds = new DataSet();
            try
            {
                List<string> list = new List<string>();
                ArrayList al = new ArrayList();

                list.Add("@ProductID");
                list.Add("@WarehouseID");
                al.Add(ProductID);
                al.Add(WarehouseID);

                ds = DAL.GetDataSet("sp_GetReorderHistory", list, al);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > StockReport, Method >GetReOrdQtyHistory(int ProductID, int WarehouseID)", ex);
            }
            return ds;
        }

        public DataSet GetBlockedQtyHistory(int ProductID, int WarehouseID)
        {
            DataSet ds = new DataSet();
            try
            {
                List<string> list = new List<string>();
                ArrayList al = new ArrayList();

                list.Add("@ProductID");
                list.Add("@WarehouseID");
                al.Add(ProductID);
                al.Add(WarehouseID);

                ds = DAL.GetDataSet("sp_GetBlockedQtyHistory", list, al);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > StockReport, Method >GetBlockedQtyHistory(int ProductID, int WarehouseID)", ex);
            }
            return ds;
        }

    }
}