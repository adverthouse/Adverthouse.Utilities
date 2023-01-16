using System;
using Microsoft.AspNetCore.Http;

namespace Adverthouse.Common.Data
{
    public class StatusInfo<T>
    {
        public int StatusCode { get; set; } = StatusCodes.Status200OK;
        public string StatusMessage { get; set; }
        public int ID { get; set; }
        public T Data { get; set; }
    }
}
