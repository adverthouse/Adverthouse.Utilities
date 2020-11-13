using Adverthouse.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Principal;

namespace Adverthouse.Core.Authorize
{
    public class ManagerAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        public string DenyPage { get; private set; }
        public string Section { get; private set; }
        public PermissionType Permission { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            IPrincipal User = context.HttpContext.User;
            if (!User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            else
            {
                if (!User.IsAllowed(Section, Permission) && !(Section == "dashboard" && Permission == PermissionType.BaseAccess))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
        }

        public ManagerAuthorize(PermissionType permissionType, string section)
        {
            Permission = permissionType;
            Section = section;
        }

        public ManagerAuthorize(PermissionType permissionType, string section, string denyPage)
        {
            DenyPage = denyPage;
            Permission = permissionType;
            Section = section;
        }
    }
}
