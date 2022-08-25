using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.Common.Interfaces;
using OnlineShop.Library.Constants;
using OnlineShop.Library.Data;
using OnlineShop.Library.OrdersService.Models;
using OnlineShop.Library.OrdersService.Repos;
using OnlineShop.Library.UserManagementService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.OrdersService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<OrdersDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(ConnectionNames.OrdersConnection)));

            services.AddTransient<IOrdersRepo, OrdersRepo>();
            services.AddTransient<IRepo<OrderedArticle>, OrderedArticlesRepo>();

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //   .AddEntityFrameworkStores<UsersDbContext>()
            //   .AddDefaultTokenProviders();

            //services.AddAuthentication(
            //    IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        options.Authority = "https://localhost:5001";
            //        options.ApiName = "https://localhost:5001/resources";
            //        options.RequireHttpsMetadata = false;
            //    });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ApiScope", policy =>
            //    {
            //        policy.RequireAuthenticatedUser();
            //        policy.RequireClaim("scope", IdConstants.ApiScope);
            //    });
            //});

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
