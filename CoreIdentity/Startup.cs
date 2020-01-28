using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreIdentity.Infrastructure;
using CoreIdentity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreIdentity
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

         
            services.AddDbContext<ApplicationIdentityDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DataConnection")));
            services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordValidator>();
            services.AddTransient<IUserValidator<ApplicationUser>, CustomUserValidator>();
            //services.AddTransient<IdentityErrorDescriber, CustomIdentityErrorDescriber>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options=> 
            {
               
                options.User.RequireUniqueEmail = true;
                
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            }).AddUserValidator<CustomUserValidator>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>() 
                .AddDefaultTokenProviders();
            

            CookieBuilder cookieBuilder = new CookieBuilder();
            cookieBuilder.Name = "MySite";
            cookieBuilder.HttpOnly = false;
            
            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Account/Login";
                options.Cookie = cookieBuilder;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan=System.TimeSpan.FromDays(60);

            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Admin}/{action=Index}/{id?}");
            });
        }
    }
}
