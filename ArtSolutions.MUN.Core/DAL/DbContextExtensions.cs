using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace ArtSolutions.MUN.Core
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Execute stored procedure with single table value parameter.
        /// </summary>
        /// <typeparam name="T">Type of object to store.</typeparam>
        /// <param name="context">DbContext instance.</param>
        /// <param name="data">Data to store</param>
        /// <param name="procedureName">Procedure name</param>
        /// <param name="paramName">Parameter name</param>
        /// <param name="typeName">User table type name</param>
        public static int ExecuteTableValueProcedure<T>(this DbContext context, IEnumerable<T> data, string procedureName, string paramName, string typeName)
        {
            //// convert source data to DataTable
            DataTable table = data.ToDataTable();

            //// create parameter
            SqlParameter parameter = new SqlParameter(paramName, table);
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.TypeName = typeName;

            //// execute sp sql
            string sql = String.Format("EXEC {0} {1};", procedureName, paramName);

            //// execute sql
            return context.Database.ExecuteSqlCommand(sql, parameter);
        }

        public static int ExecuteSqlProcedure(this DbContext context, SqlParameter[] parameters, string procedureName)
        {
            var inputparamNames = parameters.Where(x => x.Direction == ParameterDirection.Input).Select(x => x.ParameterName).ToArray();
            var outputparamNames = parameters.Where(x => x.Direction == ParameterDirection.Output).Select(x => x.ParameterName + " output").ToArray();
            if (outputparamNames.Count() > 0)
                inputparamNames = inputparamNames.Union(outputparamNames).ToArray();
            string strParams = string.Join(",", inputparamNames);
            //// execute sp sql
            string sql = String.Format("EXEC {0} {1};", procedureName, strParams);

            //// execute sql
            int aa = context.Database.ExecuteSqlCommand(sql, parameters);
            return aa;
        }
        public static int ExecuteSqlProcedureWithOutputParam(this DbContext context, SqlParameter[] parameters, string procedureName)
        {
            var paramNames = parameters.Select(x => x.ParameterName).ToArray();
            string strParams = string.Join(",", paramNames);
            //// create sql command
            string sql = String.Format("EXEC @ReturnValue = {0} {1};", procedureName, strParams);

            //// execute sql
            int result = context.Database.ExecuteSqlCommand(sql, parameters);
            return result;
        }
        /// <summary>
        /// Get DataSet From Stored Procedure
        /// </summary>
        /// <param name="context">Database Connection</param>
        /// <param name="parameters">Sql Parameter</param>
        /// <param name="procedureName">Stored Procedure Name</param>
        /// <param name="addSchemaInfo">Bool param use to get column schema details like Size.</param>
        /// <returns>Return DataSet</returns>
        public static DataSet ExecuteSqlProcedureDataSet(this DbContext context, SqlParameter[] parameters, string procedureName, bool addSchemaInfo = false)
        {
            DataSet ds = new DataSet();
            context.Database.Connection.Open();
            try
            {
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = procedureName;
                cmd.CommandTimeout = 30000;
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                DbProviderFactory factory = DbProviderFactories.GetFactory(context.Database.Connection);

                using (var adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    if (addSchemaInfo)
                        adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    adapter.Fill(ds);
                }
            }
            finally
            {
                context.Database.Connection.Close();
            }
            return ds;
        }
    }
}
