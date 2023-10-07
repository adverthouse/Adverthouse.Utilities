using System;
using Microsoft.AspNetCore.Http;

namespace Adverthouse.Common.Data
{
    public class StatusInfo<TData,TId>
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; } = String.Empty;
        public TId ID { get; set; }
        public TData Data { get; set; }

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
