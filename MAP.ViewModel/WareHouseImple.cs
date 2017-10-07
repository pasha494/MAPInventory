using MAP.Inventory.DAL;
using MAP.Inventory.Interface;
using MAP.Inventory.Logging;
using MAP.Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MAP.Inventory.Models
{
    public class WareHouseImple : IWareHouseImple
    {
        General _General = new General();
        public long SaveWareHouse(WareHouseModel _WareHouseModel)
        {
            long flg = 0;
            try
            { 
                ArrayList al = new ArrayList();
                 

                al.Add(_WareHouseModel.WareHouseID);
                al.Add(_WareHouseModel.Code);
                al.Add(_WareHouseModel.Name);
                al.Add(_WareHouseModel.Status);
                al.Add(_WareHouseModel.efDate);

                _General.Set(al, "sp_InsertUpdateWareHouse", out flg);
                // flg = DAL.ExecuteSP("sp_InsertUpdateWareHouse", Params, al);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > WareHouse, Method >SaveWareHouse()", ex);
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

        public WareHouseModel EditWareHouse(int ID)
        {
            WareHouseModel _WareHouseModel = new WareHouseModel();
            try
            {
                DataTable dt = GetGridData(ID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    _WareHouseModel.WareHouseID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    _WareHouseModel.Code = dt.Rows[0]["Code"].ToString();
                    _WareHouseModel.Name = dt.Rows[0]["Name"].ToString();
                    _WareHouseModel.Status = Convert.ToInt32(dt.Rows[0]["Status"].ToString());
                    if (dt.Rows[0]["eDate"] == DBNull.Value)
                        _WareHouseModel.efDate = ConverDate(DateTime.Now);
                    else
                        _WareHouseModel.efDate = ConverDate(Convert.ToDateTime(dt.Rows[0]["eDate"]));
                }

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > WareHouse, Method > EditWareHouse(int ID)", ex);
            }
            return _WareHouseModel;
        }

        public DataTable GetGridData(int WareHouseID)
        {
            DataTable dt = null;

            try
            {
                DataSet ds = new DataSet();
                ds = _General.Get(new ArrayList() { WareHouseID }, "sp_GetWareHouseData"); 
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > WareHouse, Method > GetGridData(int WareHouseID)", ex);
            }

            return dt;
        }

        public long DeleteWareHouse(int ProductID)
        {
            long flg = 0;

            try
            {
                _General.Set(new ArrayList() { ProductID }, "sp_DeleteWareHouse", out flg);
                

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > WareHouse, Method > DeleteWareHouse(int ProductID)", ex);
            }

            return flg;
        }
    }
}
