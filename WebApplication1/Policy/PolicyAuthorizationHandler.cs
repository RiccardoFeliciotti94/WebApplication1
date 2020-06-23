using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication1.Models.Utentes;

namespace WebApplication1.Policy
{
    public class PolicyAuthorizationHandler : AuthorizationHandler<PolicyRequirement>
    {
       
        private IHttpContextAccessor _httpContextAccessor;

        public PolicyAuthorizationHandler( IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            var user = new UtenteModel(_httpContextAccessor.HttpContext);
           
            if(user.Ruolo == requirement.Ruolo)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
