using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.Providers;

namespace WebApplication.IdentityServer
{
    public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserProvider _userProvider;
        private readonly ISystemClock _clock;
        public CustomResourceOwnerPasswordValidator(IUserProvider userProvider, ISystemClock clock) 
        {
            _userProvider = userProvider;
            _clock = clock;
        }
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = _userProvider.GetUtenteWithMailandPassword(context.UserName,context.Password);
            //var user = _userProvider.GetUserMail(context.UserName);
            if(user != null)
            {
                 context.Result = new GrantValidationResult(
                     user.Email ?? throw new ArgumentException("Subject ID not set", nameof(user.Email)),
                     OidcConstants.AuthenticationMethods.Password, 
                     _clock.UtcNow.UtcDateTime
                );
            }
           
            return Task.CompletedTask;
        }
    }
}
