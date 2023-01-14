using AutoFixture;
using IdentityModel.Client;
using Newtonsoft.Json;
using NUnit.Framework;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.Constants;
using OnlineShop.Library.OrdersService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.ApiService.ApiTests
{
    public class OrdersControllerTests : BaseRepoControllerTests
    {
        public OrdersControllerTests() : base() { }

        [Test]
        public async Task GIVEN_Orders_Repo_Client_WHEN_I_add_order_THEN_it_is_being_added_to_database()
        {
            var expected = Fixture.Build<Order>()
                .With(o => o.Articles, Fixture.CreateMany<OrderedArticle>().ToList())
                .Create();

            var jsonContent = JsonConvert.SerializeObject(expected);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var addResponse = await SystemUnderTests.PostAsync($"orders/{RepoActions.Add}", httpContent);
            Assert.IsTrue(addResponse.IsSuccessStatusCode);

            var getOneResponse = await SystemUnderTests.GetAsync($"orders?Id={expected.Id}");
            Assert.IsTrue(getOneResponse.IsSuccessStatusCode);
            var getOneResponseContent = await getOneResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<Order>(getOneResponseContent);
            AssertObjectsAreEqual(expected, actual);

            var jsonContentRemove = JsonConvert.SerializeObject(actual.Id);
            var httpContentRemove = new StringContent(jsonContentRemove, Encoding.UTF8, "application/json");
            var removeResponse = await SystemUnderTests.PostAsync($"orders/{RepoActions.Remove}", httpContentRemove);
            Assert.IsTrue(removeResponse.IsSuccessStatusCode);
        }

        [Test]
        public async Task GIVEN_Orders_Repo_Client_WHEN_I_add_several_orders_THEN_it_is_being_added_to_database()
        {
            var expected1 = Fixture.Build<Order>()
                .With(o => o.Articles, Fixture.CreateMany<OrderedArticle>().ToList())
                .Create();

            var expected2 = Fixture.Build<Order>()
                .With(o => o.Articles, Fixture.CreateMany<OrderedArticle>().ToList())
                .Create();

            var ordersToAdd = new[] { expected1, expected2 };
            var jsonContent = JsonConvert.SerializeObject(ordersToAdd);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var addRangeResponse = await SystemUnderTests.PostAsync($"orders/{RepoActions.AddRange}", httpContent);
            Assert.IsTrue(addRangeResponse.IsSuccessStatusCode);
            var addRangeResponseContent = await addRangeResponse.Content.ReadAsStringAsync();
            var addedOrderIds = JsonConvert.DeserializeObject<IEnumerable<Guid>>(addRangeResponseContent);

            var getAllResponse = await SystemUnderTests.GetAsync($"orders/{RepoActions.GetAll}");
            Assert.IsTrue(getAllResponse.IsSuccessStatusCode);
            var getOneResponseContent = await getAllResponse.Content.ReadAsStringAsync();
            var addedOrders = JsonConvert.DeserializeObject<IEnumerable<Order>>(getOneResponseContent);

            foreach (var orderId in addedOrderIds)
            {
                var expectedOrder = ordersToAdd.Single(o => o.Id == orderId);
                var actualOrder = addedOrders.Single(o => o.Id == orderId);
                AssertObjectsAreEqual(expectedOrder, actualOrder);
            }

            var jsonContentRemove = JsonConvert.SerializeObject(addedOrders.Select(order => order.Id));
            var httpContentRemove = new StringContent(jsonContentRemove, Encoding.UTF8, "application/json");
            var removeResponse = await SystemUnderTests.PostAsync($"orders/{RepoActions.RemoveRange}", httpContentRemove);
            Assert.IsTrue(removeResponse.IsSuccessStatusCode);
        }

        [Test]
        public async Task GIVEN_Orders_Repo_Client_WHEN_I_update_order_THEN_it_is_being_update_in_database()
        {
            var orderedArticles = Fixture.CreateMany<OrderedArticle>().ToList();

            var expected = Fixture.Build<Order>()
                .With(o => o.Articles, orderedArticles)
                .Create();

            var jsonContent = JsonConvert.SerializeObject(expected);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var addResponse = await SystemUnderTests.PostAsync($"orders/{RepoActions.Add}", httpContent);
            Assert.IsTrue(addResponse.IsSuccessStatusCode);

            orderedArticles.ForEach(oa => oa.Name = Fixture.Create<string>());

            expected.UserId = Fixture.Create<Guid>();
            expected.AddressId = Fixture.Create<Guid>();
            expected.Articles = orderedArticles;

            var jsonContentUpdate = JsonConvert.SerializeObject(expected);
            var httpContentUpdate = new StringContent(jsonContentUpdate, Encoding.UTF8, "application/json");
            var updateResponse = await SystemUnderTests.PostAsync($"orders/{RepoActions.Update}", httpContentUpdate);
            Assert.IsTrue(updateResponse.IsSuccessStatusCode);
            var updateResponseContent = await updateResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<Order>(updateResponseContent);

            AssertObjectsAreEqual(expected, actual);

            var jsonContentRemove = JsonConvert.SerializeObject(actual.Id);
            var httpContentRemove = new StringContent(jsonContentRemove, Encoding.UTF8, "application/json");
            var removeResponse = await SystemUnderTests.PostAsync($"orders/{RepoActions.Remove}", httpContentRemove);
            Assert.IsTrue(removeResponse.IsSuccessStatusCode);
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