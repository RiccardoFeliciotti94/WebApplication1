using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.Providers;

namespace WebApplication1.Policy
{
    public class PolicyRequirement : IAuthorizationRequirement
    {
        public string Ruolo { get; }

        public PolicyRequirement(string role)
        {
            Ruolo = role;
        }
    }
}
