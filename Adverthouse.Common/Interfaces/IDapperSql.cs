using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Adverthouse.Common.Interfaces
{
    public interface IDapperSql : IDisposable
    {
        string GetConnectionString();
        IEnumerable<T> Query<T>(string sqlCommand, int? commandTimeout);
        IEnumerable<T> Query<T>(string sqlCommand, object param = null, int? commandTimeout = null);
        int Execute(string sqlCommand, int? commandTimeout);
        int Execute(string sqlCommand, object param = null, int? commandTimeout = null);
        T ExecuteScalar<T>(string sqlCommand, int? commandTimeout);
        T ExecuteScalar<T>(string sqlCommand, object param = null, int? commandTimeout = null);
        SqlConnection SqlConnection();
        Task<IEnumerable<T>> QueryAsync<T>(string sqlCommand, int? commandTimeout);
        Task<IEnumerable<T>> QueryAsync<T>(string sqlCommand, object param = null, int? commandTimeout = null);
        Task<int> ExecuteAsync(string sqlCommand, int? commandTimeout);
        Task<int> ExecuteAsync(string sqlCommand, object param = null, int? commandTimeout = null);
        Task<T> ExecuteScalarAsync<T>(string sqlCommand, int? commandTimeout);
        Task<T> ExecuteScalarAsync<T>(string sqlCommand, object param = null, int? commandTimeout = null);
    }
}
