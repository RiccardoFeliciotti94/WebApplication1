using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.DataModels;
using WebApplication.DataAccess.SQL.Providers;

namespace WebApplication.IdentityServer
{
    public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<Utente> _userManager;
        private readonly IUserProvider _userProvider;
        private readonly ISystemClock _clock;
        public CustomResourceOwnerPasswordValidator(IUserProvider userProvider, ISystemClock clock, UserManager<Utente> userManager) 
        {
            _userProvider = userProvider;
            _userManager = userManager;
            _clock = clock;
        }
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {

            var user = _userProvider.GetUserMail(context.UserName);
            var pass = _userManager.PasswordHasher.VerifyHashedPassword(user, user.Password, context.Password).GetDisplayName();
           
            if (user != null && pass=="Success")
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
