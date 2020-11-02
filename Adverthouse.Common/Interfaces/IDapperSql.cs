using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Adverthouse.Common.Interfaces
{
    public interface IDapperSql : IDisposable
    {
        string GetConnectionString();
        IEnumerable<T> Query<T>(string sqlCommand);
        IEnumerable<T> Query<T>(string sqlCommand,object param = null);
        int Execute(string sqlCommand);
        int Execute(string sqlCommand, object param = null);
        T ExecuteScalar<T>(string sqlCommand);
        T ExecuteScalar<T>(string sqlCommand, object param = null);
        SqlConnection SqlConnection();
    }
}
