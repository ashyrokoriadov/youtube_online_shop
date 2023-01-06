using AutoFixture;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OnlineShop.Library.Clients.IdentityServer;
using OnlineShop.Library.Common.Models;
using OnlineShop.Library.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.ApiService.ApiTests
{
    public class BaseRepoControllerTests
    {
        protected readonly Fixture Fixture = new Fixture();
        protected IdentityServerClient IdentityServerClient;
        protected HttpClient SystemUnderTests;

        public BaseRepoControllerTests()
        {
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [SetUp]
        public async Task Setup()
        {
            var serviceAdressOptionsMock = new Mock<IOptions<ServiceAdressOptions>>();
            serviceAdressOptionsMock.Setup(m => m.Value).Returns(new ServiceAdressOptions()
            {
                OrdersService = "https://localhost:5005",
                IdentityServer = "https://localhost:5001",
                ApiService = "https://localhost:5009"
            });

            SystemUnderTests = new HttpClient() { BaseAddress = new Uri(serviceAdressOptionsMock.Object.Value.ApiService) };
            IdentityServerClient = new IdentityServerClient(new HttpClient(), serviceAdressOptionsMock.Object);

            var token = await IdentityServerClient.GetApiToken(new IdentityServerUserNamePassword()
            {
                UserName = "andrey",
                Password = "Pass_123"
            });

            SystemUnderTests.SetBearerToken(token.AccessToken);
        }
    }
}
