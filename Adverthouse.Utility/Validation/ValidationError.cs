using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Utility.Validation
{
    public class ValidationError
    {
        public string ErrorMessage { get; set; }
        public string ErrorField { get; set; }
        public ValidationError(string err, string field)
        {
            ErrorMessage = err;
            ErrorField = field;
        }
    }
}
