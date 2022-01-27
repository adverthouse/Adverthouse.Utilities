using System;

namespace Adverthouse.Common.Data.RocksDB
{
    public class RocksDBResponse
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public bool IsFromCache { get; set; } = false;
        public string Data { get; set; }

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
