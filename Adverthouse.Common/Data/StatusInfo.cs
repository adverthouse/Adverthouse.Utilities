using System;
using Microsoft.AspNetCore.Http;

namespace Adverthouse.Common.Data
{
    public class StatusInfo<TData,TId>
    {
        public int StatusCode { get; set; } = StatusCodes.Status400BadRequest;
        public string StatusMessage { get; set; } = String.Empty;
        public TId ID { get; set; }
        public TData Data { get; set; }
        public StatusInfo(){ }
        
        public StatusInfo(int statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
