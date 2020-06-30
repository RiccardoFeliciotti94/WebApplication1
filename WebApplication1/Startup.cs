using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication.DataAccess.SQL;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication1.Api.Middleware;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Policy;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Api.Services;
using WebApplication1.Hubs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApplication1
{
    public class Startup
    {
        private readonly IConfiguration _Configuration;

        public Startup(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession();

            services.AddHttpClient();
            services.AddSignalR();
            services.AddDbContext<ApplicationDbContext>(options =>
                         options.UseSqlServer(_Configuration.GetConnectionString("SqlLocal")));
            services.AddTransient<IUserProvider, UserProvider>();
            services.AddTransient<IMsgProvider, MsgProvider>();
            services.AddSingleton<IApiCallService, ApiCallService>();
            
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    options.SaveTokens = true;

                    options.SignedOutCallbackPath = "/Home/Index";

                    options.Scope.Add("api1.get");
                    options.Scope.Add("profile");
                    options.Scope.Add("openid");

                    //claims handle
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.ClaimActions.DeleteClaim("sid");
                    options.ClaimActions.DeleteClaim("idp");
                    options.ClaimActions.MapJsonKey("email", "email");
                    options.ClaimActions.MapJsonKey("nome", "nome");
                    options.ClaimActions.MapJsonKey("ruolo", "ruolo");

                }).AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api1";

                });
            //
            services.AddSingleton<IJwtService, JwtService>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.Requirements.Add(new PolicyRequirement("Admin")));
            });
            services.AddSingleton<IAuthorizationHandler, PolicyAuthorizationHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                app.UseExceptionHandler("/Error");
            }
            //app.UseStatusCodePagesWithRedirects("/Error/{0}");
            //app.UseExceptionHandler("/Error");
            app.UseSession();
       
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                 ).RequireAuthorization();
                endpoints.MapHub<LikeHub>("/likeHub");
            });
            
        }

        
    }
}
