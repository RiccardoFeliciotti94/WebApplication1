using System.Net;
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

            services.AddIdentity<Utente, Ruolo>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
            .AddDefaultTokenProviders();

             services.AddTransient <IUserStore<Utente>, UserStore>();
             services.AddTransient<IRoleStore<Ruolo>, RoleStore>();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Account/Login";
            });

            // var assembly = typeof(Startup).Assembly.GetName().Name;

            services.AddIdentityServer()
                
                .AddAspNetIdentity<Utente>()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())       
                .AddInMemoryApiResources(Config.GetApiResources())           
                .AddInMemoryClients(Config.GetClients())                
                .AddProfileService<CustomProfileServices>()
                .AddResourceOwnerValidator<CustomResourceOwnerPasswordValidator>()
                
                //.AddTestUsers(Config.GetUsers())                
                .AddDeveloperSigningCredential();

            services.AddControllersWithViews();       
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env )
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
            app.UseIdentityServer();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
