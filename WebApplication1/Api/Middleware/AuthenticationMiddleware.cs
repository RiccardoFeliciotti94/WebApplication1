using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;

namespace WebApplication1.Api.Middleware
{
    public static class AuthenticationMiddleware
    {
        public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var secret = config.GetSection("JwtConfig").GetSection("secret").Value;

            var key = Encoding.ASCII.GetBytes(secret);
            /* services.AddAuthentication(x =>
             {
                 x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             })
             .AddJwtBearer(x =>
             {
                 x.TokenValidationParameters = new TokenValidationParameters
                 {
                     IssuerSigningKey = new SymmetricSecurityKey(key),
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidIssuer = "localhost",
                     ValidAudience = "localhost"
                 };
             });*/
            
            services.AddAuthentication("Bearer")
             .AddIdentityServerAuthentication(options =>
             {
                 options.Authority = "http://localhost:5000";
                 options.RequireHttpsMetadata = false;
                 options.ApiName = "api1";
                 
             });
           /* services.AddAuthentication("Bearer")
         .AddJwtBearer("Bearer", options =>
         {
             options.Authority = "http://localhost:5000";
             options.RequireHttpsMetadata = false;

             options.Audience = "api1";
             
         });*/

            return services;
        }
    }
}
