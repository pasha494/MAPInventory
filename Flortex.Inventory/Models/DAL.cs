using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;

namespace Flortex.Inventory.Models
{


    public class DAL
    {
        public static string strCon = ConfigurationManager.ConnectionStrings["flortexConnection"].ToString();

        public static DataSet GetDataSet(string strQuery)
        {
            SqlConnection oCon = new SqlConnection(strCon);
            SqlDataAdapter da = new SqlDataAdapter(strQuery, oCon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public static DataSet GetDataSet(string SPName, List<string> names, ArrayList values)
        {
            SqlConnection oCon = new SqlConnection(strCon);
            SqlCommand oCmd = GetCommand(SPName, names, values);
            oCmd.Connection = oCon;
            SqlDataAdapter da = new SqlDataAdapter(oCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public static DataRow GetDataRow(string strQuery)
        {
            SqlConnection oCon = new SqlConnection(strCon);
            SqlDataAdapter da = new SqlDataAdapter(strQuery, oCon);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0];
            else
                return null;
        }

        public static int ExecuteSP(string SPName, List<string> names, ArrayList values)
        {
            int iResult = 0;
            try
            {
                SqlConnection oCon = new SqlConnection(strCon);
                SqlCommand oCmd = GetCommand(SPName, names, values);
                oCmd.Connection = oCon;
                oCon.Open();
                oCmd.ExecuteNonQuery();
                // Return value as parameter
                try
                {
                    iResult = Convert.ToInt32(oCmd.Parameters["returnVal"].Value);
                }
                catch { }

                oCon.Close();
            }
            catch
            {
            }
            return iResult;
        }

        public static string ExecuteSP(string SPName, List<string> names, ArrayList values, out int Result)
        {
            string DocName = "";
            DataSet ds=new DataSet();
            Result = 0;

            try
            {
                SqlConnection oCon = new SqlConnection(strCon);
                SqlCommand oCmd = GetCommand(SPName, names, values);
                oCmd.Connection = oCon;
                SqlDataAdapter da = new SqlDataAdapter(oCmd);

                da.Fill(ds);
                try
                {
                    Result = Convert.ToInt32(oCmd.Parameters["returnVal"].Value);
                }
                catch
                {
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DocName = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                 
            }
            return DocName;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        static SqlCommand GetCommand(string SPName, List<string> names, ArrayList values)
        {
            SqlCommand oCmd = new SqlCommand(SPName);
            oCmd.CommandType = CommandType.StoredProcedure;
            SqlParameter param;
            for (int i = 0; i < values.Count; i++)
            {
                param = new SqlParameter();
                param.ParameterName = names[i];
                param.Value = values[i];
                if (values[i] is int)
                    param.SqlDbType = SqlDbType.Int;
                else if (values[i] is long)
                    param.SqlDbType = SqlDbType.BigInt;
                else if (values[i] is DateTime)
                    param.SqlDbType = SqlDbType.DateTime;
                else if (values[i] is bool)
                    param.SqlDbType = SqlDbType.Bit;
                else
                    param.SqlDbType = SqlDbType.NVarChar;
                oCmd.Parameters.Add(param);
            }
            SqlParameter returnValue = new SqlParameter("returnVal", SqlDbType.Int);
            returnValue.Direction = ParameterDirection.ReturnValue;
            oCmd.Parameters.Add(returnValue);
            return oCmd;
        }
    }
}