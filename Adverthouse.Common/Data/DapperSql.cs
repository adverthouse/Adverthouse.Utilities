
using Adverthouse.Common.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Adverthouse.Common.Data
{
    public class DapperSql:IDapperSql
    {
        private string _connectionString = "";

        public DapperSql(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString() {
            return _connectionString;
        }
        public IEnumerable<T> Query<T>(string sqlCommand)
        {
            using (IDbConnection db2 = SqlConnection())
            { 
                return db2.Query<T>(sqlCommand);
            }
        }
        public IEnumerable<T> Query<T>(string sqlCommand, object param = null)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return db2.Query<T>(sqlCommand,param);
            }
        }
        public int Execute(string sqlCommand)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return db2.Execute(sqlCommand);
            }
        }
        public int Execute(string sqlCommand, object param = null)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return db2.Execute(sqlCommand, param);
            }
        }
        public T ExecuteScalar<T>(string sqlCommand)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return db2.ExecuteScalar<T>(sqlCommand);
            }
        }

        public T ExecuteScalar<T>(string sqlCommand, object param = null)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return db2.ExecuteScalar<T>(sqlCommand, param);
            }
        }
        public void Dispose() {}

        public SqlConnection SqlConnection()
        {
            return new SqlConnection(GetConnectionString());
        }




    }
}
