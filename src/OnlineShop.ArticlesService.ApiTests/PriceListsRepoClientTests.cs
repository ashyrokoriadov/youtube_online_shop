using AutoFixture;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.Clients.ArticlesService;
using OnlineShop.Library.Clients.IdentityServer;
using OnlineShop.Library.Options;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineShop.ArticlesService.ApiTests
{
    public class PriceListsRepoClientTests
    {
        private readonly Fixture _fixture = new Fixture();
        private IdentityServerClient _identityServerClient;
        private PriceListsClient _systemUnderTests;
        private ArticlesClient _ariclesClient;

        public PriceListsRepoClientTests()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [SetUp]
        public async Task Setup()
        {
            var serviceAdressOptionsMock = new Mock<IOptions<ServiceAdressOptions>>();
            serviceAdressOptionsMock.Setup(m => m.Value).Returns(new ServiceAdressOptions() { ArticlesService = "https://localhost:5007", IdentityServer = "https://localhost:5001" });

            _systemUnderTests = new PriceListsClient(new HttpClient(), serviceAdressOptionsMock.Object);
            _ariclesClient = new ArticlesClient(new HttpClient(), serviceAdressOptionsMock.Object);

            _identityServerClient = new IdentityServerClient(new HttpClient(), serviceAdressOptionsMock.Object);

            var identityOptions = new IdentityServerApiOptions()
            {
                ClientId = "test.client",
                ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A"
            };

            var token = await _identityServerClient.GetApiToken(identityOptions);
            _systemUnderTests.HttpClient.SetBearerToken(token.AccessToken);
            _ariclesClient.HttpClient.SetBearerToken(token.AccessToken);
        }

        [Test]
        public async Task GIVEN_Orders_Repo_Client_WHEN_I_add_order_THEN_it_is_being_added_to_database()
        {
            var article = _fixture.Build<Article>().Create();

            var addArticleResponse = await _ariclesClient.Add(article);
            Assert.IsTrue(addArticleResponse.IsSuccessfull);

            var expected = _fixture.Build<PriceList>().With(x => x.ArticleId, article.Id).Create();

            var addResponse = await _systemUnderTests.Add(expected);
            Assert.IsTrue(addResponse.IsSuccessfull);

            var getOneResponse = await _systemUnderTests.GetOne(addResponse.Payload);
            Assert.IsTrue(getOneResponse.IsSuccessfull);
            var actual = getOneResponse.Payload;

            AssertObjectsAreEqual(expected, actual);

            var removeResponse = await _systemUnderTests.Remove(addResponse.Payload);
            Assert.IsTrue(removeResponse.IsSuccessfull);

            var removeArticleResponse = await _ariclesClient.Remove(article.Id);
            Assert.IsTrue(removeArticleResponse.IsSuccessfull);
        }

        [Test]
        public async Task GIVEN_Orders_Repo_Client_WHEN_I_add_several_orders_THEN_it_is_being_added_to_database()
        {
            var article = _fixture.Build<Article>().Create();

            var addArticleResponse = await _ariclesClient.Add(article);
            Assert.IsTrue(addArticleResponse.IsSuccessfull);

            var expected1 = _fixture.Build<PriceList>().With(x => x.ArticleId, article.Id).Create();
            var expected2 = _fixture.Build<PriceList>().With(x => x.ArticleId, article.Id).Create();
            var ordersToAdd = new[] { expected1, expected2 };

            var addRangeResponse = await _systemUnderTests.AddRange(ordersToAdd);
            Assert.IsTrue(addRangeResponse.IsSuccessfull);

            var getAllResponse = await _systemUnderTests.GetAll();
            Assert.IsTrue(getAllResponse.IsSuccessfull);

            var addedOrders = getAllResponse.Payload;

            foreach (var orderId in addRangeResponse.Payload)
            {
                var expectedOrder = ordersToAdd.Single(o => o.Id == orderId);
                var actualOrder = addedOrders.Single(o => o.Id == orderId);
                AssertObjectsAreEqual(expectedOrder, actualOrder);
            }

            var removeRangeResponse = await _systemUnderTests.RemoveRange(addRangeResponse.Payload);
            Assert.IsTrue(removeRangeResponse.IsSuccessfull);

            var removeArticleResponse = await _ariclesClient.Remove(article.Id);
            Assert.IsTrue(removeArticleResponse.IsSuccessfull);
        }

        [Test]
        public async Task GIVEN_Orders_Repo_Client_WHEN_I_update_order_THEN_it_is_being_update_in_database()
        {
            var article = _fixture.Build<Article>().Create();

            var addArticleResponse = await _ariclesClient.Add(article);
            Assert.IsTrue(addArticleResponse.IsSuccessfull);

            var expected = _fixture.Build<PriceList>().With(x => x.ArticleId, article.Id).Create();

            var addResponse = await _systemUnderTests.Add(expected);
            Assert.IsTrue(addResponse.IsSuccessfull);

            expected.Name = _fixture.Create<string>();
            expected.Price = _fixture.Create<decimal>();
            expected.ValidFrom = _fixture.Create<DateTime>();
            expected.ValidTo = _fixture.Create<DateTime>();

            var updateResponse = await _systemUnderTests.Update(expected);
            Assert.IsTrue(updateResponse.IsSuccessfull);
            var actual = updateResponse.Payload;

            AssertObjectsAreEqual(expected, actual);

            var removeResponse = await _systemUnderTests.Remove(addResponse.Payload);
            Assert.IsTrue(removeResponse.IsSuccessfull);

            var removeArticleResponse = await _ariclesClient.Remove(article.Id);
            Assert.IsTrue(removeArticleResponse.IsSuccessfull);
        }

        private void AssertObjectsAreEqual(PriceList expected, PriceList actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Price, actual.Price);
            Assert.AreEqual(expected.ValidFrom, actual.ValidFrom);
            Assert.AreEqual(expected.ValidTo, actual.ValidTo);
        }
    }
}
