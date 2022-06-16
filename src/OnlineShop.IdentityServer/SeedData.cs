// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using OnlineShop.Library.Common.Models;
using OnlineShop.Library.Data;
using OnlineShop.Library.UserManagementService.Models;

namespace OnlineShop.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<UsersDbContext>(options =>
               options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<UsersDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<UsersDbContext>();
                    context.Database.Migrate();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var andrey = userMgr.FindByNameAsync("andrey").Result;
                    if (andrey == null)
                    {
                        andrey = new ApplicationUser
                        {
                            UserName = "andrey",
                            Email = "andrey@email.com",
                            EmailConfirmed = true,
                            DefaultAddress =  new Address()
                            {
                                City = "Warsaw",
                                Country = "Poland",
                                PostalCode = "00-001",
                                AddressLine1 = "Jasna 21",
                                AddressLine2 = "34"
                            },
                            DeliveryAddress = new Address()
                            {
                                City = "Kraków",
                                Country = "Poland",
                                PostalCode = "30-001",
                                AddressLine1 = "Wspólna 45"
                            },
                        };
                        var result = userMgr.CreateAsync(andrey, "Pass_123").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(andrey, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Andrey Shyrokoryadov"),
                            new Claim(JwtClaimTypes.GivenName, "Andrey"),
                            new Claim(JwtClaimTypes.FamilyName, "Shyrokoryadov"),
                            new Claim(JwtClaimTypes.WebSite, "https://ashyrokoriadov.github.io/"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("Andrey has been created");
                    }
                    else
                    {
                        Log.Debug("Andrey already exists");

                        if(andrey.DefaultAddress == null)
                        {
                            andrey.DefaultAddress = new Address()
                            {
                                City = "Warsaw",
                                Country = "Poland",
                                PostalCode = "00-001",
                                AddressLine1 = "Jasna 21",
                                AddressLine2 = "34"
                            };
                        }

                        if (andrey.DeliveryAddress == null)
                        {
                            andrey.DeliveryAddress = new Address()
                            {
                                City = "Kraków",
                                Country = "Poland",
                                PostalCode = "30-001",
                                AddressLine1 = "Wspólna 45"
                            };
                        }

                        var result = userMgr.UpdateAsync(andrey).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Log.Debug("Andrey has been updated");
                    }
                }
            }
        }
    }
}
