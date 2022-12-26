using AutoFixture;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.Clients.IdentityServer;
using OnlineShop.Library.Clients.OrdersService;
using OnlineShop.Library.Options;
using OnlineShop.Library.OrdersService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.OrdersService.ApiTests
{
    public class OrderedArticlesRepoClientTests
    {
        private readonly Fixture _fixture = new Fixture();
        private IdentityServerClient _identityServerClient;
        private OrdersClient _ordersClient;
        private OrderedArticlesClient _systemUnderTests;

        public OrderedArticlesRepoClientTests()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [SetUp]
        public async Task Setup()
        {
            var serviceAdressOptionsMock = new Mock<IOptions<ServiceAdressOptions>>();
            serviceAdressOptionsMock.Setup(m => m.Value).Returns(new ServiceAdressOptions() { OrdersService = "https://localhost:5005", IdentityServer = "https://localhost:5001" });

            _ordersClient = new OrdersClient(new HttpClient(), serviceAdressOptionsMock.Object);
            _systemUnderTests = new OrderedArticlesClient(new HttpClient(), serviceAdressOptionsMock.Object);
            _identityServerClient = new IdentityServerClient(new HttpClient(), serviceAdressOptionsMock.Object);

            var identityOptions = new IdentityServerApiOptions()
            {
                ClientId = "test.client",
                ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A"
            };

            var token = await _identityServerClient.GetApiToken(identityOptions);
            _systemUnderTests.HttpClient.SetBearerToken(token.AccessToken);
            _ordersClient.HttpClient.SetBearerToken(token.AccessToken);
        }

        [Test]
        public async Task GIVEN_Ordered_Articles_Repo_Client_WHEN_I_add_article_THEN_it_is_being_added_to_database()
        {
            var order = _fixture.Build<Order>()
                .With(o => o.Articles, Enumerable.Empty<OrderedArticle>().ToList())
                .Create();

            var addOrderResponse = await _ordersClient.Add(order);
            Assert.IsTrue(addOrderResponse.IsSuccessfull);

            var expected = _fixture.Build<OrderedArticle>()
                .With(oa => oa.Order, order)
                .With(oa => oa.OrderId, order.Id)
                .Create();

            var addOrderedArticleResponse = await _systemUnderTests.Add(expected);
            Assert.IsTrue(addOrderedArticleResponse.IsSuccessfull);

            var getOneResponse = await _systemUnderTests.GetOne(addOrderedArticleResponse.Payload);
            Assert.IsTrue(getOneResponse.IsSuccessfull);
            var actual = getOneResponse.Payload;

            AssertObjectsAreEqual(expected, actual);

            var removeOrderResponse = await _ordersClient.Remove(addOrderResponse.Payload);
            Assert.IsTrue(removeOrderResponse.IsSuccessfull);
        }

        [Test]
        public async Task GIVEN_Ordered_Articles_Repo_Client_WHEN_I_add_several_ordered_articles_THEN_it_is_being_added_to_database()
        {
            var order = _fixture.Build<Order>()
               .With(o => o.Articles, Enumerable.Empty<OrderedArticle>().ToList())
               .Create();

            var addOrderResponse = await _ordersClient.Add(order);
            Assert.IsTrue(addOrderResponse.IsSuccessfull);

            var expected1 = _fixture.Build<OrderedArticle>()
                .With(oa => oa.Order, order)
                .With(oa => oa.OrderId, order.Id)
                .Create();

            var expected2 = _fixture.Build<OrderedArticle>()
               .With(oa => oa.Order, order)
               .With(oa => oa.OrderId, order.Id)
               .Create();

            var orderedArticlesToAdd = new[] { expected1, expected2 };

            var addOrderedArticleResponse = await _systemUnderTests.AddRange(orderedArticlesToAdd);
            Assert.IsTrue(addOrderedArticleResponse.IsSuccessfull);

            var getAllResponse = await _systemUnderTests.GetAll();
            Assert.IsTrue(getAllResponse.IsSuccessfull);
            var addedOrderedArticles = getAllResponse.Payload;

            foreach (var orderedArticleId in addOrderedArticleResponse.Payload)
            {
                var expectedOrder = orderedArticlesToAdd.Single(o => o.Id == orderedArticleId);
                var actualOrder = addedOrderedArticles.Single(o => o.Id == orderedArticleId);
                AssertObjectsAreEqual(expectedOrder, actualOrder);
            }

            var removeRangeResponse = await _systemUnderTests.RemoveRange(addOrderedArticleResponse.Payload);
            Assert.IsTrue(removeRangeResponse.IsSuccessfull);

            var removeOrderResponse = await _ordersClient.Remove(order.Id);
            Assert.IsTrue(removeOrderResponse.IsSuccessfull);
        }

        [Test]
        public async Task GIVEN_Ordered_Articles_Repo_Client_WHEN_I_update_ordered_article_THEN_it_is_being_update_in_database()
        {
            var order = _fixture.Build<Order>()
               .With(o => o.Articles, Enumerable.Empty<OrderedArticle>().ToList())
               .Create();

            var addOrderResponse = await _ordersClient.Add(order);
            Assert.IsTrue(addOrderResponse.IsSuccessfull);

            var expected = _fixture.Build<OrderedArticle>()
                .With(oa => oa.Order, order)
                .With(oa => oa.OrderId, order.Id)
                .Create();

            var addOrderedArticleResponse = await _systemUnderTests.Add(expected);
            Assert.IsTrue(addOrderedArticleResponse.IsSuccessfull);

            expected.Name = _fixture.Create<string>();
            expected.Description = _fixture.Create<string>();
            expected.Price = _fixture.Create<decimal>();
            expected.Quantity = _fixture.Create<int>();

            var updateResponse = await _systemUnderTests.Update(expected);
            Assert.IsTrue(updateResponse.IsSuccessfull);
            var actual = updateResponse.Payload;

            AssertObjectsAreEqual(expected, actual);

            var removeOrderResponse = await _ordersClient.Remove(addOrderResponse.Payload);
            Assert.IsTrue(removeOrderResponse.IsSuccessfull);
        }

        private void AssertObjectsAreEqual(OrderedArticle expected, OrderedArticle actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Price, actual.Price);
            Assert.AreEqual(expected.Quantity, actual.Quantity);

            if (expected.Price != actual.Price)
            {
                    expected.PriceListName = "Manualy assigned";
            }
        }
    }
}
