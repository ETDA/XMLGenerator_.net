using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

using XMLGenerator.model;

namespace XMLGenerator.controller
{
    class DatabaseController
    {
        private SqlConnection connection;
        private Dictionary<string, string> databaseParamList;
        private List<DBMapping> dbMappingList;

        public DatabaseController(string databaseConnectionFile)
        {
            dbMappingList = new List<DBMapping>();
            PassDatabaseConfig(databaseConnectionFile);
            //SetDatabaseParamList(databaseParamList);
        }

        public void PassDatabaseConfig(string databaseConnectionFile)
        {
            //StreamReader streamReader = new StreamReader(databaseConnectionFile);
            //Dictionary<string, string> connectionDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(streamReader.ReadToEnd());
            JObject jObject = JObject.Parse(File.ReadAllText(databaseConnectionFile));

            ConnectDatabase(
                jObject["databaseType"].ToString(), 
                jObject["hostName"].ToString(), 
                jObject["port"].ToString(), 
                jObject["databaseName"].ToString(), 
                jObject["userName"].ToString(), 
                jObject["password"].ToString()
                );

            var databaseBindingObj = jObject["parameter_biding"];
            foreach (var bindingList in databaseBindingObj)
            {
                string tableName = bindingList["tableName"].ToString();
                string orderBy = bindingList["orderBy"].ToString();
                string where = bindingList["where"].ToString();


                dbMappingList.Add(new DBMapping()
                {
                    tableName = bindingList["tableName"].ToString(),
                    orderBy = bindingList["orderBy"].ToString(),
                    where = bindingList["where"].ToString()
                }) ;
            }

        }

        private void ConnectDatabase(string databaseType, string hostName, string port, string databaseName, string username, string password)
        {
            if (databaseType.Equals("MSSQL", StringComparison.InvariantCultureIgnoreCase))
            {
                connection = new SqlConnection(@"Server=" + hostName + "," + port + ";Database=" + databaseName + ";User ID=" + username + ";Password =" + password + ";MultipleActiveResultSets=True");
            }
            else
            {
                throw new Exception("Unsupported database type");
            }

            connection.Open();
        }

        public object ReadDatabase(string entity, int offset)
        {
            object result = null;
            //split table and field name
            entity = entity.Replace("@", "");

            String tableName = entity.Split('.')[0];
            String fieldName = entity.Split('.')[1];

            DBMapping databaseBinding = dbMappingList.Find(dbBinding => dbBinding.tableName.Equals(tableName, StringComparison.InvariantCultureIgnoreCase));

            String where = databaseBinding.where;
            String orderBy = databaseBinding.orderBy;


            String sql = "SELECT " + fieldName + " FROM " + tableName;

            if (where != null && !where.Trim().Equals(""))
            {
                sql = sql + " WHERE " + where;
            }
            
            if (orderBy != null && !orderBy.Trim().Equals(""))
            {
                sql = sql + " ORDER BY " + orderBy;
            }

            sql = sql + " OFFSET " + offset + " ROWS FETCH NEXT 1 ROWS ONLY";

            foreach (KeyValuePair<string, string> databaseParam in databaseParamList)
            {
                if (sql.Contains(databaseParam.Key))
                {
                    sql = sql.Replace(databaseParam.Key, databaseParam.Value);
                }
            }

            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            //connection.Open();
            SqlDataReader sqlReader = sqlCommand.ExecuteReader();
            if (sqlReader.Read())
            {
                result = sqlReader[0];
            }
            sqlReader.Close();
            sqlCommand.Dispose();

            //sqlReader = null;
            //sqlCommand = null;

            return result;
        }

        public int checkAll(string entity)
        {
            int result = 0; ;
            //split table and field name
            if (!entity.Contains("@"))
            {
                return 0;
            }

            entity = entity.Replace("@", "");

            String tableName = entity.Split('.')[0];
            String fieldName = entity.Split('.')[1];

            DBMapping databaseBinding = dbMappingList.Find(dbBinding => dbBinding.tableName.Equals(tableName, StringComparison.InvariantCultureIgnoreCase));

            String where = databaseBinding.where;
            String orderBy = databaseBinding.orderBy;


            String sql = "SELECT COUNT(1) AS RESULT FROM " + tableName;

            if (where != null && !where.Trim().Equals(""))
            {
                sql = sql + " WHERE " + where;
            }

            foreach (KeyValuePair<string, string> databaseParam in databaseParamList)
            {
                if (sql.Contains(databaseParam.Key))
                {
                    sql = sql.Replace(databaseParam.Key, databaseParam.Value);
                }
            }

            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            SqlDataReader sqlReader = sqlCommand.ExecuteReader();
            if (sqlReader.Read())
            {
                result = Convert.ToInt32(sqlReader[0]);
            }
            sqlReader.Close();
            sqlCommand.Dispose();
            return result;
        }

        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                connection = null;
            }
        }

        public void setDatabaseParamList(Dictionary<string, string> databaseParamList)
        {
            this.databaseParamList = databaseParamList;
        }

    }
}
