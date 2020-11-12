using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Core.Security
{
    public static class SecurityUtility
    {
        public static string Encrypt(string key, string data, string applicationName = "App")
        {
            var dataProtectionProvider = DataProtectionProvider.Create(applicationName);
            var protector = dataProtectionProvider.CreateProtector(key);
            return protector.Protect(data);
        }

        public static string Decrypt(string key, string data, string applicationName = "App")
        {
            var dataProtectionProvider = DataProtectionProvider.Create(applicationName);
            var protector = dataProtectionProvider.CreateProtector(key);
            return protector.Unprotect(data);
        }
    }
}
