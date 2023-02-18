using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OnlineShop.Library.Clients.IdentityServer;
using OnlineShop.Library.Options;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using OnlineShop.Library.Clients;
using OnlineShop.Library.Common.Interfaces;
using System;

namespace OnlineShop.ArticlesService.ApiTests
{
    public abstract class ArticleServiceRepoBaseApiTest<TClient, TEntity> 
        where TClient : IRepoClient<TEntity>, IHttpClientContainer
        where TEntity : IIdentifiable
    {
        protected readonly Fixture Fixture = new Fixture();
        protected IOptions<ServiceAdressOptions> ServiceAdressOptions;
        protected IdentityServerApiOptions IdentityServerApiOptions;
        protected IdentityServerClient IdentityServerClient;
        protected TClient SystemUnderTests;

        public ArticleServiceRepoBaseApiTest()
        {
            ConfigureFixture();
        }

        protected virtual void ConfigureFixture()
        {
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [SetUp]
        public async Task Setup()
        {
            SetServiceAdressOptions();
            SetIdentityServerApiOptions();

            IdentityServerClient = new IdentityServerClient(new HttpClient(), ServiceAdressOptions);

            CreateSystemUnderTests(); 
            await AuthorizeSystemUnderTests();
        }

        protected virtual void SetServiceAdressOptions()
        {
            var serviceAdressOptionsMock = new Mock<IOptions<ServiceAdressOptions>>();

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            switch (env)
            {
                case "Docker":
                    serviceAdressOptionsMock.Setup(m => m.Value)
                        .Returns(new ServiceAdressOptions() { 
                            ArticlesService = "http://localhost:5006", 
                            IdentityServer = "http://192.168.1.233:5001" 
                        });
                    break;
                default:
                    serviceAdressOptionsMock.Setup(m => m.Value)
                        .Returns(new ServiceAdressOptions() { 
                            ArticlesService = "https://localhost:5007", 
                            IdentityServer = "https://localhost:5001" 
                        });
                    break;
            }
            
            ServiceAdressOptions = serviceAdressOptionsMock.Object;
        }

        protected virtual void SetIdentityServerApiOptions()
        {
            IdentityServerApiOptions = new IdentityServerApiOptions()
            {
                ClientId = "test.client",
                ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A"
            };
        }

        protected virtual async Task AuthorizeSystemUnderTests()
        {
            var token = await IdentityServerClient.GetApiToken(IdentityServerApiOptions);
            SystemUnderTests.HttpClient.SetBearerToken(token.AccessToken);
        }

        /// <summary>
        /// Creates an instance of type TClient.
        /// </summary>
        protected abstract void  CreateSystemUnderTests();

        [Test]
        public async virtual Task GIVEN_Repo_Client_WHEN_I_add_entity_THEN_it_is_being_added_to_database()
        {
            var expected = CreateExpectedEntity();

            var addResponse = await SystemUnderTests.Add(expected);
            Assert.IsTrue(addResponse.IsSuccessfull);

            var getOneResponse = await SystemUnderTests.GetOne(addResponse.Payload);
            Assert.IsTrue(getOneResponse.IsSuccessfull);
            var actual = getOneResponse.Payload;

            AssertObjectsAreEqual(expected, actual);

            var removeResponse = await SystemUnderTests.Remove(addResponse.Payload);
            Assert.IsTrue(removeResponse.IsSuccessfull);
        }

        [Test]
        public async virtual Task GIVEN_Repo_Client_WHEN_I_add_several_entities_THEN_it_is_being_added_to_database()
        {
            var expected1 = CreateExpectedEntity();
            var expected2 = CreateExpectedEntity();

            var entitiesToAdd = new[] { expected1, expected2 };

            var addRangeResponse = await SystemUnderTests.AddRange(entitiesToAdd);
            Assert.IsTrue(addRangeResponse.IsSuccessfull);

            var getAllResponse = await SystemUnderTests.GetAll();
            Assert.IsTrue(getAllResponse.IsSuccessfull);

            var addedEntities = getAllResponse.Payload;

            foreach (var entityId in addRangeResponse.Payload)
            {
                var expectedEntity = entitiesToAdd.Single(o => o.Id == entityId);
                var actualEntity = addedEntities.Single(o => o.Id == entityId);
                AssertObjectsAreEqual(expectedEntity, actualEntity);
            }

            var removeRangeResponse = await SystemUnderTests.RemoveRange(addRangeResponse.Payload);
            Assert.IsTrue(removeRangeResponse.IsSuccessfull);
        }

        [Test]
        public async virtual Task GIVEN_Repo_Client_WHEN_I_update_entuty_THEN_it_is_being_updated_in_database()
        {
            var expected = CreateExpectedEntity();

            var addResponse = await SystemUnderTests.Add(expected);
            Assert.IsTrue(addResponse.IsSuccessfull);

            AmendExpectedEntityForUpdating(expected);

            var updateResponse = await SystemUnderTests.Update(expected);
            Assert.IsTrue(updateResponse.IsSuccessfull);
            var actual = updateResponse.Payload;

            AssertObjectsAreEqual(expected, actual);

            var removeResponse = await SystemUnderTests.Remove(addResponse.Payload);
            Assert.IsTrue(removeResponse.IsSuccessfull);
        }

        protected abstract void AssertObjectsAreEqual(TEntity expected, TEntity actual);

        protected virtual TEntity CreateExpectedEntity() => Fixture.Build<TEntity>().Create();

        protected abstract void AmendExpectedEntityForUpdating(TEntity expected);
    }
}
