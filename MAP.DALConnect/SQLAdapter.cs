using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;

namespace DALConnect
{
    public class SQLAdapter
    {
        private static string DBName = "FlortexProd";
        private static ReaderWriterLock m_ProcedureCacheLock = new ReaderWriterLock();
        private static Hashtable m_ProcedureCache = new Hashtable();
        private static Dictionary<int, string> ConnectionCache = new Dictionary<int, string>();

        private static SQLAdapter.DbUtilProcedure Procedure(SqlConnection connection, string procedureName, SqlTransaction oTransaction)
        {
            SQLAdapter.DbUtilProcedure dbUtilProcedure = (SQLAdapter.DbUtilProcedure)null;
            if (!SQLAdapter.m_ProcedureCache.Contains((object)procedureName))
            {
                SqlDataReader sqlDataReader = (SqlDataReader)null;
                try
                {
                    SQLAdapter.m_ProcedureCacheLock.AcquireWriterLock(-1);
                    if (!SQLAdapter.m_ProcedureCache.Contains((object)procedureName))
                    {
                        dbUtilProcedure = new SQLAdapter.DbUtilProcedure();
                        SqlCommand sqlCommand = new SqlCommand("spUTL_GetProcedureMetaData", connection);
                        sqlCommand.Parameters.AddWithValue("@objname", (object)procedureName);
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Transaction = oTransaction;
                        sqlDataReader = sqlCommand.ExecuteReader();
                        dbUtilProcedure.ProcedureName = procedureName;
                        while (sqlDataReader.Read())
                            dbUtilProcedure.ProcedureParameters.Add((object)new SQLAdapter.DbUtilParameter()
                            {
                                ParameterName = sqlDataReader.GetString(0),
                                ParameterLength = sqlDataReader.GetInt32(2),
                                ParameterType = SQLAdapter.DbUtilParameter.StringToType(sqlDataReader.GetString(1)),
                                ParameterDirection = SQLAdapter.DbUtilParameter.IntToDirection(sqlDataReader.GetInt32(5))
                            });
                        dbUtilProcedure.ProcedureParameters.Add((object)new SQLAdapter.DbUtilParameter()
                        {
                            ParameterName = "@RETURN_VALUE",
                            ParameterLength = 4,
                            ParameterType = SqlDbType.Int,
                            ParameterDirection = ParameterDirection.ReturnValue
                        });
                        SQLAdapter.m_ProcedureCache[(object)procedureName] = (object)dbUtilProcedure;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (sqlDataReader != null)
                        sqlDataReader.Close();
                    SQLAdapter.m_ProcedureCacheLock.ReleaseWriterLock();
                }
            }
            try
            {
                SQLAdapter.m_ProcedureCacheLock.AcquireReaderLock(-1);
                dbUtilProcedure = (SQLAdapter.DbUtilProcedure)SQLAdapter.m_ProcedureCache[(object)procedureName];
            }
            catch (Exception ex)
            {
            }
            finally
            {
                SQLAdapter.m_ProcedureCacheLock.ReleaseReaderLock();
            }
            return dbUtilProcedure;
        }

        public static SQLDBResult Execute(string procedureName, ArrayList procedureParameters, string strConnection)
        {
            SQLDBResult sqldbResult = (SQLDBResult)null;
            SqlConnection connection = (SqlConnection)null;
            try
            {
                connection = new SqlConnection(strConnection);
                connection.Open();
                SqlCommand selectCommand = SQLAdapter.Procedure(connection, procedureName, (SqlTransaction)null).Command(connection, procedureParameters);
                selectCommand.CommandTimeout = 0;
                sqldbResult = new SQLDBResult();
                new SqlDataAdapter(selectCommand).Fill(sqldbResult.Contents);
                foreach (SqlParameter sqlParameter in (DbParameterCollection)selectCommand.Parameters)
                {
                    switch (sqlParameter.Direction)
                    {
                        case ParameterDirection.Output:
                        case ParameterDirection.InputOutput:
                        case ParameterDirection.ReturnValue:
                            sqldbResult.Parameters[(object)sqlParameter.ParameterName] = sqlParameter.Value;
                            continue;
                        default:
                            continue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SQLAdapter.DbUtilException(ex.Message, ex);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return sqldbResult;
        }

        public static int Execute(string Query, string strConnection)
        {
            SqlConnection sqlConnection = new SqlConnection(strConnection);
            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                sqlCommand.CommandTimeout = 0;
                sqlCommand.CommandText = Query;
                return sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new SQLAdapter.DbUtilException(ex.Message, ex);
            }
            finally
            {
                if (sqlConnection != null && sqlConnection.State == ConnectionState.Open)
                    sqlConnection.Close();
            }
        }

        public static string GetConnection()
        {
            string str;
            if (ConfigurationManager.AppSettings["DBAuthentication"] == "SQLServer")
                str = "Data Source=" + ConfigurationManager.AppSettings["DBServer"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["DBUserID"] + ";Password=" + ConfigurationManager.AppSettings["DBPassword"];
            else
                str = "Data Source=" + ConfigurationManager.AppSettings["DBServer"] + ";Trusted_Connection=Yes;";
            return str;
        }

        public static string GetConnection(int CompanyIndex)
        {
            string str = CompanyIndex == 0 ? "" : CompanyIndex.ToString();
            if (!SQLAdapter.ConnectionCache.ContainsKey(CompanyIndex))
            {
                if (ConfigurationManager.AppSettings["DBAuthentication"] == "SQLServer")
                    SQLAdapter.ConnectionCache.Add(CompanyIndex, "Data Source=" + ConfigurationManager.AppSettings["DBServer"] + ";Initial Catalog=" + DBName + str + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["DBUserID"] + ";Password=" + ConfigurationManager.AppSettings["DBPassword"]);
                else
                    SQLAdapter.ConnectionCache.Add(CompanyIndex, "Data Source=" + ConfigurationManager.AppSettings["DBServer"] + ";Initial Catalog=" + DBName + str + ";Trusted_Connection=Yes;");
            }
            return SQLAdapter.ConnectionCache[CompanyIndex];
        }

        public class DbUtilException : Exception
        {
            private int m_nErrorCode = -1;

            public int ErrorCode
            {
                get
                {
                    return this.m_nErrorCode;
                }
                set
                {
                    this.m_nErrorCode = value;
                }
            }

            public DbUtilException()
            {
            }

            public DbUtilException(string message)
                : base(message)
            {
            }

            public DbUtilException(string message, int errCode)
                : base(message)
            {
                this.m_nErrorCode = errCode;
            }

            public DbUtilException(string message, Exception inner)
                : base(message, inner)
            {
            }

            public DbUtilException(string message, Exception inner, int errCode)
                : base(message, inner)
            {
                this.m_nErrorCode = errCode;
            }
        }

        private class DbUtilProcedure
        {
            public ArrayList ProcedureParameters = new ArrayList();
            public string ProcedureName;

            public SqlCommand Command(SqlConnection connection, ArrayList values)
            {
                SqlCommand sqlCommand = new SqlCommand(this.ProcedureName, connection);
                if (values != null)
                {
                    int num = 0;
                    foreach (SQLAdapter.DbUtilParameter dbUtilParameter in this.ProcedureParameters)
                    {
                        SqlParameter sqlParameter = sqlCommand.Parameters.Add(dbUtilParameter.ParameterName, dbUtilParameter.ParameterType, dbUtilParameter.ParameterLength);
                        sqlParameter.Direction = dbUtilParameter.ParameterDirection;
                        if (dbUtilParameter.ParameterDirection == ParameterDirection.Input && values.Count > num)
                            sqlParameter.Value = values[num++];
                    }
                }
                sqlCommand.CommandType = CommandType.StoredProcedure;
                return sqlCommand;
            }
        }

        private class DbUtilParameter
        {
            private static SQLAdapter.DbUtilParameter.TypeMap[] Types = new SQLAdapter.DbUtilParameter.TypeMap[24]
      {
        new SQLAdapter.DbUtilParameter.TypeMap("bit", SqlDbType.Bit),
        new SQLAdapter.DbUtilParameter.TypeMap("char", SqlDbType.Char),
        new SQLAdapter.DbUtilParameter.TypeMap("bigint", SqlDbType.BigInt),
        new SQLAdapter.DbUtilParameter.TypeMap("binary", SqlDbType.Binary),
        new SQLAdapter.DbUtilParameter.TypeMap("datetime", SqlDbType.DateTime),
        new SQLAdapter.DbUtilParameter.TypeMap("decimal", SqlDbType.Decimal),
        new SQLAdapter.DbUtilParameter.TypeMap("float", SqlDbType.Float),
        new SQLAdapter.DbUtilParameter.TypeMap("image", SqlDbType.Image),
        new SQLAdapter.DbUtilParameter.TypeMap("int", SqlDbType.Int),
        new SQLAdapter.DbUtilParameter.TypeMap("money", SqlDbType.Money),
        new SQLAdapter.DbUtilParameter.TypeMap("nchar", SqlDbType.NChar),
        new SQLAdapter.DbUtilParameter.TypeMap("numeric", SqlDbType.Int),
        new SQLAdapter.DbUtilParameter.TypeMap("nvarchar", SqlDbType.NVarChar),
        new SQLAdapter.DbUtilParameter.TypeMap("real", SqlDbType.Real),
        new SQLAdapter.DbUtilParameter.TypeMap("smalldatetime", SqlDbType.SmallDateTime),
        new SQLAdapter.DbUtilParameter.TypeMap("smallint", SqlDbType.SmallInt),
        new SQLAdapter.DbUtilParameter.TypeMap("smallmoney", SqlDbType.SmallMoney),
        new SQLAdapter.DbUtilParameter.TypeMap("timestamp", SqlDbType.Timestamp),
        new SQLAdapter.DbUtilParameter.TypeMap("tinyint", SqlDbType.TinyInt),
        new SQLAdapter.DbUtilParameter.TypeMap("uniqueidentifier", SqlDbType.UniqueIdentifier),
        new SQLAdapter.DbUtilParameter.TypeMap("varbinary", SqlDbType.VarBinary),
        new SQLAdapter.DbUtilParameter.TypeMap("varchar", SqlDbType.VarChar),
        new SQLAdapter.DbUtilParameter.TypeMap("variant", SqlDbType.Variant),
        new SQLAdapter.DbUtilParameter.TypeMap("ntext", SqlDbType.Text)
      };
            public string ParameterName;
            public SqlDbType ParameterType;
            public int ParameterLength;
            public ParameterDirection ParameterDirection;

            public static SqlDbType StringToType(string typeName)
            {
                SqlDbType sqlDbType = SqlDbType.Variant;
                foreach (SQLAdapter.DbUtilParameter.TypeMap typeMap in SQLAdapter.DbUtilParameter.Types)
                {
                    if (typeMap.Name.ToLower() == typeName)
                    {
                        sqlDbType = typeMap.Type;
                        break;
                    }
                }
                return sqlDbType;
            }

            public static ParameterDirection IntToDirection(int direction)
            {
                ParameterDirection parameterDirection = ParameterDirection.InputOutput;
                switch (direction)
                {
                    case 0:
                        parameterDirection = ParameterDirection.Input;
                        break;
                    case 1:
                        parameterDirection = ParameterDirection.Output;
                        break;
                }
                return parameterDirection;
            }

            private class TypeMap
            {
                public string Name;
                public SqlDbType Type;

                public TypeMap(string name, SqlDbType type)
                {
                    this.Name = name;
                    this.Type = type;
                }
            }
        }

        public class DbUtilReaderResult
        {
            public ArrayList HashList = new ArrayList();
            public Hashtable Parameters = new Hashtable();
            public object Return;
        }
    }
}

