using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Adverthouse.Core.Authorize
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class MemberAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        public string DenyPage { get; set; }

        private readonly string _someFilterParameter;
        public MemberAuthorize()
        {
        }
        public MemberAuthorize(string someFilterParameter)
        {
            _someFilterParameter = someFilterParameter;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                return;
            }

            if (!user.IsAuthLatestVersion())
            {
                context.Result = new RedirectResult(AuthUtility.LogoutEnforceUrl);
                return;
            }
        }
    }
}
