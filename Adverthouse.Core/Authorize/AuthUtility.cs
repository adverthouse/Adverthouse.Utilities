using Adverthouse.Core.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Adverthouse.Core.Authorize
{
    public static class AuthUtility
    {
        public const string emailSchema = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
        public static int CurrentVersion = 1;
        private static ConcurrentDictionary<int, List<PermissionHelper>> permissionsByRoleId = new();

        public static void UpsertPermissionsByRoleId(int roleId,List<PermissionHelper> permissions){
            permissionsByRoleId.AddOrUpdate(roleId,permissions,(key, oldValue) => permissions);
        }
 
        public static int RoleId(this IPrincipal user) 
                    => user.GetClaimByType<int>("RoleId"); 
        public static bool IsAuthLatestVersion(this IPrincipal user)
        {
            return CurrentVersion == user.GetClaimByType<int>("Version");
        }

        public static string GetClaimByType(this IPrincipal user, string type)
        {
            return GetClaimByType<string>(user, type);
        }

        public static T GetClaimByType<T>(this IPrincipal user, string type)
        {
            var identity = user.Identity as ClaimsIdentity;
            var claim = identity.Claims.Where(a => a.Type == type).FirstOrDefault();
            return (T)Convert.ChangeType(claim?.Value, typeof(T));
        }

        public static bool IsAllowed(this IPrincipal user, string section)
        {
            var permissions =  permissionsByRoleId.GetValueOrDefault(user.RoleId());
            if (permissions == null) return false;

            var perm = from per in permissions
                       where per.TypeOfPermission == PermissionType.BaseAccess && per.Section == section
                       select per;
            var isAllowed = false;
            if (perm.Count<PermissionHelper>() > 0)
            {
                isAllowed = true;
            }
            return isAllowed;
        }

        public static bool IsAllowed(this IPrincipal user, string section, PermissionType permissionType)
        {
            var permissions =  permissionsByRoleId.GetValueOrDefault(user.RoleId());
            if (permissions == null) return false;

            var perm = from per in permissions
                       where per.TypeOfPermission == permissionType && per.Section == section
                       select per;
            var isAllowed = false;

            if (perm.Count<PermissionHelper>() > 0)
            {
                isAllowed = true;
            }
            return isAllowed;
        }
    }
}
