using System;

namespace Adverthouse.Common.Data.RocksDB
{
    public class RocksDBResponse<T>
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public int ID { get; set; }
        public T Data { get; set; }

        public RocksDBResponse()
        {
            StatusCode = (Int32)RocksDBStatusCodes.Error;
        }

        public void SetStatusCode(RocksDBStatusCodes statusCode) {
            StatusCode = (Int32)statusCode;
        }

        public RocksDBResponse(RocksDBStatusCodes statusCode)
        {
            StatusCode = (Int32)statusCode;
        }
    }
}
