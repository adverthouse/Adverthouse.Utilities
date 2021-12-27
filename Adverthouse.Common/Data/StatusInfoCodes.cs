using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adverthouse.Common.Data
{ 
    public enum StatusInfoCodes
    {
        Error = 5000,
        Ok = 2000,
        Authenticated = 1000,
        MistakePassword = 1001,
        UserNotFound = 1002,
        IncorrectPassword = 1003,
        InvalidApiKey = 1004,
        ApiKeyExpired = 1005,
        NotFound = 1006,
        NotAcceptedTerms = 2000,
        UploadError = 3000,
        CaptchaError = 3001,
        AlreadyExists = 20001
    }
}
