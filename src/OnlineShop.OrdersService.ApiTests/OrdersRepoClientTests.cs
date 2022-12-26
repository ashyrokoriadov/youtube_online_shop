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
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineShop.OrdersService.ApiTests
{
    public class OrdersRepoClientTests
    {
        private readonly Fixture _fixture = new Fixture();
        private IdentityServerClient _identityServerClient;
        private OrdersClient _systemUnderTests;

        public OrdersRepoClientTests()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [SetUp]
        public async Task Setup()
        {
            var serviceAdressOptionsMock = new Mock<IOptions<ServiceAdressOptions>>();
            serviceAdressOptionsMock.Setup(m => m.Value).Returns(new ServiceAdressOptions() { OrdersService = "https://localhost:5005", IdentityServer = "https://localhost:5001" });

            _systemUnderTests = new OrdersClient(new HttpClient(), serviceAdressOptionsMock.Object);
            _identityServerClient = new IdentityServerClient(new HttpClient(), serviceAdressOptionsMock.Object);

            var identityOptions = new IdentityServerApiOptions()
            {
                ClientId = "test.client",
                ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A"
            };

            var token = await _identityServerClient.GetApiToken(identityOptions);
            _systemUnderTests.HttpClient.SetBearerToken(token.AccessToken);
        }

        [Test]
        public async Task GIVEN_Orders_Repo_Client_WHEN_I_add_order_THEN_it_is_being_added_to_database()
        {
            var expected = _fixture.Build<Order>()
                .With(o => o.Articles, _fixture.CreateMany<OrderedArticle>().ToList())
                .Create();

            var addResponse = await _systemUnderTests.Add(expected);
            Assert.IsTrue(addResponse.IsSuccessfull);

            var getOneResponse = await _systemUnderTests.GetOne(addResponse.Payload);
            Assert.IsTrue(getOneResponse.IsSuccessfull);
            var actual = getOneResponse.Payload;

            AssertObjectsAreEqual(expected, actual);

            var removeResponse = await _systemUnderTests.Remove(addResponse.Payload);
            Assert.IsTrue(removeResponse.IsSuccessfull);
        }

        [Test]
        public async Task GIVEN_Orders_Repo_Client_WHEN_I_add_several_orders_THEN_it_is_being_added_to_database()
        {
            var expected1 = _fixture.Build<Order>()
                .With(o => o.Articles, _fixture.CreateMany<OrderedArticle>().ToList())
                .Create();

            var expected2 = _fixture.Build<Order>()
                .With(o => o.Articles, _fixture.CreateMany<OrderedArticle>().ToList())
                .Create();

            var ordersToAdd = new[] { expected1, expected2 };

            var addRangeResponse = await _systemUnderTests.AddRange(ordersToAdd);
            Assert.IsTrue(addRangeResponse.IsSuccessfull);

            var getAllResponse = await _systemUnderTests.GetAll();
            Assert.IsTrue(getAllResponse.IsSuccessfull);

            var addedOrders = getAllResponse.Payload;

            foreach(var orderId in addRangeResponse.Payload)
            {
                var expectedOrder = ordersToAdd.Single(o => o.Id == orderId);
                var actualOrder = addedOrders.Single(o => o.Id == orderId);
                AssertObjectsAreEqual(expectedOrder, actualOrder);
            }

            var removeRangeResponse = await _systemUnderTests.RemoveRange(addRangeResponse.Payload);
            Assert.IsTrue(removeRangeResponse.IsSuccessfull);
        }

        [Test]
        public async Task GIVEN_Orders_Repo_Client_WHEN_I_update_order_THEN_it_is_being_update_in_database()
        {
            var orderedArticles = _fixture.CreateMany<OrderedArticle>().ToList();

            var expected = _fixture.Build<Order>()
                .With(o => o.Articles, orderedArticles)
                .Create();

            var addResponse = await _systemUnderTests.Add(expected);
            Assert.IsTrue(addResponse.IsSuccessfull);

            orderedArticles.ForEach(oa => oa.Name = _fixture.Create<string>());

            expected.UserId = _fixture.Create<Guid>();
            expected.AddressId = _fixture.Create<Guid>();
            expected.Articles = orderedArticles;

            var updateResponse = await _systemUnderTests.Update(expected);
            Assert.IsTrue(updateResponse.IsSuccessfull);
            var actual = updateResponse.Payload;

            AssertObjectsAreEqual(expected, actual);

            var removeResponse = await _systemUnderTests.Remove(addResponse.Payload);
            Assert.IsTrue(removeResponse.IsSuccessfull);
        }

        private void AssertObjectsAreEqual(Order expected, Order actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.AddressId, actual.AddressId);
            Assert.AreEqual(expected.UserId, actual.UserId);
            Assert.AreEqual(expected.Created, actual.Created);
            Assert.AreEqual(expected.Articles.Count(), actual.Articles.Count());
        }
    }
}