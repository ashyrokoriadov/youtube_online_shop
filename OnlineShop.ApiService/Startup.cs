using OnlineShop.ApiService.Authorization;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OnlineShop.Library.Clients.IdentityServer;
using OnlineShop.Library.Clients.UserManagementService;
using OnlineShop.Library.Constants;
using OnlineShop.Library.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineShop.Library.Clients.ArticlesService;
using OnlineShop.Library.Clients;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.OrdersService.Models;
using OnlineShop.Library.Clients.OrdersService;

namespace OnlineShop.ApiService
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineShop.ApiService", Version = "v1" });
            });

            services.AddHttpClient<UsersClient>();
            services.AddHttpClient<RolesClient>();
            services.AddHttpClient<IdentityServerClient>();
            services.AddHttpClient<ArticlesClient>();
            services.AddHttpClient<PriceListsClient>();
            services.AddHttpClient<OrderedArticle>();
            services.AddHttpClient<Order>();

            services.AddTransient<IRolesClient, RolesClient>();
            services.AddTransient<IUsersClient, UsersClient>();
            services.AddTransient<IIdentityServerClient, IdentityServerClient>();
            services.AddTransient<IClientAuthorization, HttpClientAuthorization>();
            services.AddTransient<IRepoClient<Article>, ArticlesClient>();
            services.AddTransient<IRepoClient<PriceList>, PriceListsClient>();
            services.AddTransient<IRepoClient<OrderedArticle>, OrderedArticlesClient>();
            services.AddTransient<IRepoClient<Order>, OrdersClient>();

            services.Configure<IdentityServerApiOptions>(Configuration.GetSection(IdentityServerApiOptions.SectionName));
            services.Configure<ServiceAdressOptions>(Configuration.GetSection(ServiceAdressOptions.SectionName));

            services.AddAuthentication(
                IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.ApiName = "https://localhost:5001/resources";
                    options.RequireHttpsMetadata = false;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", IdConstants.WebScope);
                });
            });

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineShop.ApiService v1"));
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization("ApiScope");
            });
        }
    }
}
