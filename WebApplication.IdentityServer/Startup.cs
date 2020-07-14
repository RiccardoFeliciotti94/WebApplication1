using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication.DataAccess.SQL;
using WebApplication.DataAccess.SQL.DataModels;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication.IdentityServer.Provider;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebApplication.IdentityServer
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
            services.AddDbContext<ApplicationDbContext>(options =>
                         options.UseSqlServer(_Configuration.GetConnectionString("SqlLocal")));

            services.AddScoped<IProfileService, CustomProfileServices>();
            services.AddScoped<IResourceOwnerPasswordValidator, CustomResourceOwnerPasswordValidator>();
            services.AddTransient<IUserProvider, UserProvider>();

            services.AddLocalApiAuthentication();


            services.AddIdentity<Utente, Ruolo>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
            .AddDefaultTokenProviders();

            services.AddTransient<IUserStore<Utente>, UserStore>();
            services.AddTransient<IRoleStore<Ruolo>, RoleStore>();

            services.AddAuthorization();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Account/Login";
                config.LogoutPath = "/Account/Logout";
            });

            services.AddIdentityServer(options => { 
              options.Discovery.CustomEntries.Add("local_api", "~/localapi");
            options.Events.RaiseSuccessEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseErrorEvents = true; }
              )
                .AddAspNetIdentity<Utente>()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddProfileService<CustomProfileServices>()
                .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>()
                 .AddDeveloperSigningCredential();

            services.AddControllersWithViews();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls12;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseIdentityServer();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
