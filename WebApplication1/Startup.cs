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
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication1.Api.Middleware;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Policy;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Api.Services;
using WebApplication1.Hubs;

namespace WebApplication1
{
    public class Startup
    {
        private readonly IConfiguration _Configuration;

        public Startup(IConfiguration configuration)
        {
            _Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
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
            services.AddTokenAuthentication(_Configuration);
            services.AddSingleton<IJwtService, JwtService>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.Requirements.Add(new PolicyRequirement("Admin")));
            });
            services.AddSingleton<IAuthorizationHandler, PolicyAuthorizationHandler>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

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
            app.UseStatusCodePagesWithRedirects("/Error/{0}");
            app.UseExceptionHandler("/Error");
            app.UseSession();
       
            app.UseStaticFiles();
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",pattern: "{controller=Login}/{action=Index}/{id?}");
                endpoints.MapHub<LikeHub>("/likeHub");
            });
            
        }

        
    }
}
