using MAP.Inventory.Logging;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MAP.Inventory.DAL
{

    public class General
    {
        private static string DataBase = "SQL";

        public General()
        {
            //if (General.DataBase != null && !General.DataBase.Equals(""))
            //    return;
            //General.DataBase = ConfigurationManager.AppSettings["DBTYPE"];
        }

        public DataSet Get(ArrayList param, string SPName, int CompanyIndex = 0)
        {
            DataSet dataSet = new DataSet();
            long ReturnValue = 0L;
            try
            {
                if (General.DataBase.Equals("SQL"))
                {
                    SQLDBResult dbResult = SQLAdapter.Execute(SPName, param, SQLAdapter.GetConnection(CompanyIndex));
                    dataSet = dbResult.Contents;
                    ReturnValue = Convert.ToInt64(dbResult.Parameters[(object)"@RETURN_VALUE"]);
                    //if (SPName == "sp_GetUserDetails")
                    //    throw new Exception();
                    this.LogException(param, SPName, CompanyIndex, dbResult, "", ReturnValue);
                }
            }
            catch (Exception ex)
            {
                this.LogException(param, SPName, CompanyIndex, (SQLDBResult)null, ex, ReturnValue);
            }
            return dataSet;
        }

        public string Get(ArrayList param, string SPName, out string OutputParameterName, int iFlg, int CompanyIndex = 0)
        {
            OutputParameterName = "";
            long ReturnValue = 0;
            try
            {
                if (General.DataBase.Equals("SQL"))
                {
                    SQLDBResult dbResult = SQLAdapter.Execute(SPName, param, SQLAdapter.GetConnection(CompanyIndex));
                    if (dbResult.Parameters.ContainsKey((object)"@CodeGenerated"))
                        OutputParameterName = dbResult.Parameters[(object)"@CodeGenerated"].ToString();
                    ReturnValue = Convert.ToInt64(dbResult.Parameters[(object)"@RETURN_VALUE"]);
                    this.LogException(param, SPName, CompanyIndex, dbResult, "", ReturnValue);
                    return OutputParameterName;
                }
            }
            catch (Exception ex)
            {
                this.LogException(param, SPName, CompanyIndex, (SQLDBResult)null, ex.Message, ReturnValue);
            }
            return "";
        }

        public DataSet Get(ArrayList param, string SPName, out long ReturnValue, int CompanyIndex = 0)
        {
            DataSet dataSet = new DataSet();
            ReturnValue = 0L;
            try
            {
                if (General.DataBase.Equals("SQL"))
                {
                    SQLDBResult dbResult = SQLAdapter.Execute(SPName, param, SQLAdapter.GetConnection(CompanyIndex));
                    ReturnValue = Convert.ToInt64(dbResult.Parameters[(object)"@RETURN_VALUE"]);
                    dataSet = dbResult.Contents;
                    this.LogException(param, SPName, CompanyIndex, dbResult, "", ReturnValue);
                }
            }
            catch (Exception ex)
            {
                this.LogException(param, SPName, CompanyIndex, (SQLDBResult)null, ex.Message, ReturnValue);
            }
            return dataSet;
        }

        private string GetParamString(ArrayList param)
        {
            string str = "";
            for (int index = 0; index < param.Count; ++index)
            {
                if (param[index] != null)
                {
                    if (param[index].GetType() == typeof(string))
                        str = str + "\n " + index.ToString() + ",'" + param[index].ToString().Replace("'", "''") + "'";
                    else
                        str = str + "\n " + index.ToString() + "," + param[index].ToString();
                }
                else
                    str = str + "\n " + index.ToString() + ",NULL";
            }
            return str;
        }

        public string Set(ArrayList param, string SPName, out long ReturnValue, int CompanyIndex = 0)
        {
            ReturnValue = 0L;
            try
            {
                if (General.DataBase.Equals("SQL"))
                {
                    SQLDBResult dbResult = SQLAdapter.Execute(SPName, param, SQLAdapter.GetConnection(CompanyIndex));
                    ReturnValue = Convert.ToInt64(dbResult.Parameters[(object)"@RETURN_VALUE"]);
                    this.LogException(param, SPName, CompanyIndex, dbResult, "", ReturnValue);
                    if (dbResult.Contents != null)
                    {
                        if (dbResult.Contents.Tables.Count > 0)
                        {
                            if (dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Rows.Count > 0)
                            {
                                if (dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Columns.Contains("ErrorMessage"))
                                    return dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Rows[0]["ErrorMessage"].ToString();
                                else if (dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Columns.Contains("DOCNAME"))
                                    return dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Rows[0]["DOCNAME"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.LogException(param, SPName, CompanyIndex, (SQLDBResult)null, ex.Message, ReturnValue);
            }
            return string.Empty;
        }

        public DataSet SetWithDataSet(ArrayList param, string SPName, out long ReturnValue, int CompanyIndex = 0)
        {
            ReturnValue = 0L;
            try
            {
                if (General.DataBase.Equals("SQL"))
                {
                    SQLDBResult dbResult = SQLAdapter.Execute(SPName, param, SQLAdapter.GetConnection(CompanyIndex));
                    ReturnValue = Convert.ToInt64(dbResult.Parameters[(object)"@RETURN_VALUE"]);
                    this.LogException(param, SPName, CompanyIndex, dbResult, "", ReturnValue);
                    return dbResult.Contents;
                }
            }
            catch (Exception ex)
            {
                this.LogException(param, SPName, CompanyIndex, (SQLDBResult)null, ex.Message, ReturnValue);
            }
            return new DataSet();
        }

        private void LogException(ArrayList param, string SPName, int CompanyIndex, SQLDBResult dbResult, Exception ex, long ReturnValue)
        {
            PLog.Error(this.GetParamString(param) + " : SPName:" + SPName, ex);
        }

        private void LogException(ArrayList param, string SPName, int CompanyIndex, SQLDBResult dbResult, string Message, long ReturnValue)
        {
            PLog.Info(this.GetParamString(param) + " : SPName:" + SPName);
            //PLog.Error(this.GetParamString(param) +""+,);
            //if (dbResult != null)
            //{
            //    if (dbResult.Contents.Tables.Count > 0 && dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Columns.Contains("ServerMessage") && dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Rows.Count > 0)
            //    {
            //        Logger.Instance();
            //        Logger.DebugLog("General Service", ":: Set CompanyIndex:" + CompanyIndex.ToString() + " SpName:" + SPName, "paramerters" + this.GetParamString(param) + " message=" + dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Rows[0]["ServerMessage"].ToString());
            //    }
            //    if (dbResult.Contents.Tables.Count > 0 && dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Columns.Contains("ErrorMessage") && dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Rows.Count > 0)
            //    {
            //        Logger.Instance();
            //        Logger.DebugLog("General Service", ":: Set CompanyIndex:" + CompanyIndex.ToString() + " SpName:" + SPName, "paramerters" + this.GetParamString(param) + " message=" + dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Rows[0]["ErrorMessage"].ToString());
            //    }
            //    else
            //    {
            //        Logger.Instance();
            //        Logger.DebugLog("General Service", ":: Set CompanyIndex:" + CompanyIndex.ToString() + " SpName:" + SPName, "paramerters" + this.GetParamString(param));
            //    }
            //    if (ReturnValue != -999L || dbResult.Contents.Tables.Count <= 0 || dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Rows.Count <= 0)
            //        return;
            //    Logger.Instance();
            //    Logger.InfoLog("OnStartup: Logger Instance Created Successfully");
            //    Logger.InfoLog("OnStartup: Application Starting...");
            //    if (dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Columns.Contains("ServerMessage"))
            //        Logger.ErrorLog("General Service", ":: Set CompanyIndex:" + CompanyIndex.ToString() + " SpName:" + SPName + "Parameters :" + this.GetParamString(param), dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Rows[0]["ServerMessage"].ToString());
            //    else if (dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Columns.Contains("ErrorMessage"))
            //        Logger.ErrorLog("General Service", ":: Set CompanyIndex:" + CompanyIndex.ToString() + " SpName:" + SPName + "Parameters :" + this.GetParamString(param), dbResult.Contents.Tables[dbResult.Contents.Tables.Count - 1].Rows[0]["ErrorMessage"].ToString());
            //    else
            //        Logger.ErrorLog("General Service", ":: Set CompanyIndex:" + CompanyIndex.ToString() + " SpName:" + SPName + "Parameters :" + this.GetParamString(param), "");
            //}
            //else
            //    Logger.ErrorLog("General Service", ":: Set CompanyIndex:" + CompanyIndex.ToString() + " SpName:" + SPName + "Parameters :" + this.GetParamString(param), Message);
        }

        public int ExecuteScript(string Query, int CompanyIndex = 0)
        {
            return SQLAdapter.Execute(Query, SQLAdapter.GetConnection(CompanyIndex));
        }

        public bool IsDatabaseExists()
        {
            try
            {
                SQLAdapter.Execute(" if exists(select name from sys.databases where name='pact2c') RAISERROR('-100',16,1) ", SQLAdapter.GetConnection());
                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public string HealthCheckConnection()
        {
            string flag = "";
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(SQLAdapter.GetConnection());
                connection.Open();
                flag = "Success";
            }
            catch (Exception ex)
            {
                flag = "Error:" + ex.ToString();
                PLog.Error("HealthCheck", ex, 0, "");
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

            }
            return flag;
        }
        //public bool CreateDatabase(string dbName, string FilePath)
        //{
        //    return SQLAdapter.Execute("CREATE DATABASE " + dbName + " ON  PRIMARY ( NAME = 'PACT2C', FILENAME ='" + FilePath + dbName + ".mdf' )" + " LOG ON ( NAME = 'PACT2C_log', FILENAME = '" + FilePath + dbName + "_log.ldf')", SQLAdapter.GetConnection()) > 0;
        //}
    }

}
