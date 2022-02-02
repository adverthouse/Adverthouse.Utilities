
using Adverthouse.Common.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Adverthouse.Common.Data
{
    public class DapperSql:IDapperSql
    {
        private string _connectionString = "";
        private int _defaultConnectionTimeOut;
        public SqlConnection SqlConnection() => new SqlConnection(GetConnectionString());

        public DapperSql(string connectionString, int connectionTimeOut = 300)
        {
            _connectionString = connectionString;
            _defaultConnectionTimeOut = connectionTimeOut;
        }

        public string GetConnectionString() {
            return _connectionString;
        }
        public IEnumerable<T> Query<T>(string sqlCommand, int? commandTimeout)
        {
            using (IDbConnection db2 = SqlConnection())
            { 
                return db2.Query<T>(sqlCommand,commandTimeout: commandTimeout ?? _defaultConnectionTimeOut);
            }
        }
        public IEnumerable<T> Query<T>(string sqlCommand, object param = null, int? commandTimeout = null)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return db2.Query<T>(sqlCommand,param, commandTimeout: commandTimeout ?? _defaultConnectionTimeOut);
            }
        }
        public int Execute(string sqlCommand, int? commandTimeout)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return db2.Execute(sqlCommand, commandTimeout: commandTimeout ?? _defaultConnectionTimeOut);
            }
        }
        public int Execute(string sqlCommand, object param = null, int? commandTimeout =null)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return db2.Execute(sqlCommand, param, commandTimeout: commandTimeout ?? _defaultConnectionTimeOut);
            }
        }
        public T ExecuteScalar<T>(string sqlCommand, int? commandTimeout)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return db2.ExecuteScalar<T>(sqlCommand, commandTimeout: commandTimeout ?? _defaultConnectionTimeOut);
            }
        }

        public T ExecuteScalar<T>(string sqlCommand, object param = null, int? commandTimeout = null)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return db2.ExecuteScalar<T>(sqlCommand, param, commandTimeout: commandTimeout ?? _defaultConnectionTimeOut);
            }
        }
        public void Dispose() {}

        
    }
}
