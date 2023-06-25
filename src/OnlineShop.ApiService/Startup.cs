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

using OnlineShop.Library.Clients.ArticlesService;
using OnlineShop.Library.Clients;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.OrdersService.Models;
using OnlineShop.Library.Clients.OrdersService;
using Microsoft.IdentityModel.Tokens;

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

            services.AddTransient<ILoginClient, LoginClient>();
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

            var serviceAddressOptions = new ServiceAdressOptions();
            Configuration.GetSection(ServiceAdressOptions.SectionName).Bind(serviceAddressOptions);

            services.AddAuthentication(
                IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = serviceAddressOptions.IdentityServer;
                    //options.ApiName = $"{serviceAddressOptions.IdentityServer}/resources";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters() { ValidateAudience = false };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", IdConstants.WebScope);
                });
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policyBuilder =>
                {
                    policyBuilder.WithOrigins("https://localhost:7163").AllowAnyHeader().AllowAnyMethod();
                    policyBuilder.WithOrigins("http://localhost:5163").AllowAnyHeader().AllowAnyMethod();
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
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization("ApiScope");
            });
        }
    }
}
