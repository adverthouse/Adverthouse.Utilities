using System;
using Microsoft.AspNetCore.Http;

namespace Adverthouse.Common.Data
{
    public class StatusInfo<T>
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public long ID { get; set; }
        public T Data { get; set; }

        public StatusInfo()
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }

        public StatusInfo(int statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
