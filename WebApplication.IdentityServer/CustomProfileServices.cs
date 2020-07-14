using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.DataModels;
using WebApplication.DataAccess.SQL.Providers;

namespace WebApplication.IdentityServer
{

    public class CustomProfileServices : IProfileService
    {
        private readonly IUserProvider _userProvider;

        public CustomProfileServices (IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            
            if(context.RequestedClaimTypes.Any())
            {
                var user = _userProvider.GetUserMail(context.Subject.GetSubjectId());
                if (user != null)
                {               
                    context.AddRequestedClaims(
                        new List<Claim> {
                            new Claim("email", user.Email),
                            new Claim("nome", user.Nome),
                            new Claim("ruolo", user.Ruolo.ToString()),
                            new Claim("immagine",user.Img),
                            new Claim("info",user.Info)
                    });
                }
               
            }
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var user = _userProvider.GetUserMail(context.Subject.GetSubjectId());
            context.IsActive = user != null;

            return Task.CompletedTask;
        }
    }
}
