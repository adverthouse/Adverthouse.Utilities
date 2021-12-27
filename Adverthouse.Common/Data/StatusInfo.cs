using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adverthouse.Common.Data
{
    public class StatusInfo
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public int ID { get; set; }
        public object Data { get; set; }

        public StatusInfo()
        {
            StatusCode = (Int32)StatusInfoCodes.Error;
        }

        public StatusInfo(StatusInfoCodes statusCode)
        {
            StatusCode = (Int32)statusCode;
        }
    }
}
