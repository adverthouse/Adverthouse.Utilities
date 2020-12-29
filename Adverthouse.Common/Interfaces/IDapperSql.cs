using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

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
    }
}
