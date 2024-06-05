
using Adverthouse.Common.Interfaces;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Adverthouse.Common.Data
{
    public class DapperSql:IDapperSql
    {
        private string _connectionString = "";
        private int _defaultConnectionTimeOut;
        private string _connectionType = "sql_server";
        public IDbConnection SqlConnection() =>
             _connectionType == "sql_server" ?  
             new SqlConnection(GetConnectionString()) :
             new SqliteConnection(GetConnectionString());

        public DapperSql(string connectionString, int connectionTimeOut = 300,bool isSqlServer = true)
        {
            if (!isSqlServer) _connectionType = "sqlite";
            _connectionString = connectionString;
            _defaultConnectionTimeOut = connectionTimeOut;
        }

        public string GetConnectionString()  => _connectionString; 

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
        public async Task<IEnumerable<T>> QueryAsync<T>(string sqlCommand, int? commandTimeout)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return await db2.QueryAsync<T>(sqlCommand, commandTimeout: commandTimeout ?? _defaultConnectionTimeOut);
            }
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string sqlCommand, object param = null, int? commandTimeout = null)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return await db2.QueryAsync<T>(sqlCommand, param, commandTimeout: commandTimeout ?? _defaultConnectionTimeOut);
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

        public async Task<int> ExecuteAsync(string sqlCommand, int? commandTimeout)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return await db2.ExecuteAsync(sqlCommand, commandTimeout: commandTimeout ?? _defaultConnectionTimeOut);
            }
        }
        public async Task<int> ExecuteAsync(string sqlCommand, object param = null, int? commandTimeout = null)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return await db2.ExecuteAsync(sqlCommand, param, commandTimeout: commandTimeout ?? _defaultConnectionTimeOut);
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
        public async Task<T> ExecuteScalarAsync<T>(string sqlCommand, int? commandTimeout)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return await db2.ExecuteScalarAsync<T>(sqlCommand, commandTimeout: commandTimeout ?? _defaultConnectionTimeOut);
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string sqlCommand, object param = null, int? commandTimeout = null)
        {
            using (IDbConnection db2 = SqlConnection())
            {
                return await db2.ExecuteScalarAsync<T>(sqlCommand, param, commandTimeout: commandTimeout ?? _defaultConnectionTimeOut);
            }
        }
        public void Dispose() {} 
    }
}
