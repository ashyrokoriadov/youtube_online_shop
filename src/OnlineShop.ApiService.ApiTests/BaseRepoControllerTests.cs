using AutoFixture;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OnlineShop.Library.Clients.UserManagementService;
using OnlineShop.Library.Common.Models;
using OnlineShop.Library.Options;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineShop.ApiService.ApiTests
{
    public class BaseRepoControllerTests
    {
        protected readonly Fixture Fixture = new Fixture();
        protected ILoginClient LoginClient;
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

            var env  = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            switch(env)
            {
                case "Docker":
                    serviceAdressOptionsMock.Setup(m => m.Value).Returns(new ServiceAdressOptions()
                    {
                        OrdersService = "http://localhost:5004",
                        //IdentityServer = "https://localhost:5001",
                        UserManagementService = "http://localhost:5002",
                        ApiService = "http://localhost:5008"
                    });
                    break;
                default:
                    serviceAdressOptionsMock.Setup(m => m.Value).Returns(new ServiceAdressOptions()
                    {
                        OrdersService = "https://localhost:5005",
                        IdentityServer = "https://localhost:5001",
                        UserManagementService = "https://localhost:5003",
                        ApiService = "https://localhost:5009"
                    });
                    break;
            }

            SystemUnderTests = new HttpClient() { BaseAddress = new Uri(serviceAdressOptionsMock.Object.Value.ApiService) };
            LoginClient = new LoginClient(new HttpClient(), serviceAdressOptionsMock.Object);

            var token = await LoginClient.GetApiTokenByUsernameAndPassword(new IdentityServerUserNamePassword()
            {
                UserName = "andrey",
                Password = "Pass_123"
            });

            SystemUnderTests.SetBearerToken(token.AccessToken);
        }
    }
}
