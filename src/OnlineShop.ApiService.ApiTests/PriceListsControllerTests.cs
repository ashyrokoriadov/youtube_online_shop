using AutoFixture;
using Newtonsoft.Json;
using NUnit.Framework;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.ApiService.ApiTests
{
    public class PriceListsControllerTests : BaseRepoControllerTests
    {
        public PriceListsControllerTests() : base() { }

        [Test]
        public async Task GIVEN_PriceListsController_WHEN_I_add_entity_THEN_it_is_being_added_to_database()
        {
            var article = Fixture.Build<Article>().Create();

            var addResponseJsonContent = JsonConvert.SerializeObject(article);
            var addResponseHttpContent = new StringContent(addResponseJsonContent, Encoding.UTF8, "application/json");
            var addResponse = await SystemUnderTests.PostAsync($"articles/{RepoActions.Add}", addResponseHttpContent);
            Assert.IsTrue(addResponse.IsSuccessStatusCode);

            var expected = Fixture.Build<PriceList>()
              .With(oa => oa.ArticleId, article.Id)
              .Create();

            var priceListAddJsonContent = JsonConvert.SerializeObject(expected);
            var priceListAddHttpContent = new StringContent(priceListAddJsonContent, Encoding.UTF8, "application/json");
            var priceListAddResponse = await SystemUnderTests.PostAsync($"pricelists/{RepoActions.Add}", priceListAddHttpContent);
            Assert.IsTrue(priceListAddResponse.IsSuccessStatusCode);

            var getOneResponse = await SystemUnderTests.GetAsync($"pricelists?Id={expected.Id}");
            Assert.IsTrue(getOneResponse.IsSuccessStatusCode);
            var getOneResponseContent = await getOneResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<PriceList>(getOneResponseContent);

            AssertObjectsAreEqual(expected, actual);


            var jsonContentRemove = JsonConvert.SerializeObject(article.Id);
            var httpContentRemove = new StringContent(jsonContentRemove, Encoding.UTF8, "application/json");
            var removeResponse = await SystemUnderTests.PostAsync($"articles/{RepoActions.Remove}", httpContentRemove);
            Assert.IsTrue(removeResponse.IsSuccessStatusCode);
        }

        [Test]
        public async Task GIVEN_PriceListsController_WHEN_I_add_several_entities_THEN_it_is_being_added_to_database()
        {
            var article = Fixture.Build<Article>().Create();

            var addResponseJsonContent = JsonConvert.SerializeObject(article);
            var addResponseHttpContent = new StringContent(addResponseJsonContent, Encoding.UTF8, "application/json");
            var addResponse = await SystemUnderTests.PostAsync($"articles/{RepoActions.Add}", addResponseHttpContent);
            Assert.IsTrue(addResponse.IsSuccessStatusCode);

            var expected1 = Fixture.Build<PriceList>()
              .With(oa => oa.ArticleId, article.Id)
              .Create();

            var expected2 = Fixture.Build<PriceList>()
              .With(oa => oa.ArticleId, article.Id)
              .Create();

            var priceListsToAdd = new[] { expected1, expected2 };
            var jsonContent = JsonConvert.SerializeObject(priceListsToAdd);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var addRangeResponse = await SystemUnderTests.PostAsync($"pricelists/{RepoActions.AddRange}", httpContent);
            Assert.IsTrue(addRangeResponse.IsSuccessStatusCode);
            var addRangeResponseContent = await addRangeResponse.Content.ReadAsStringAsync();
            var addedPriceListsIds = JsonConvert.DeserializeObject<IEnumerable<Guid>>(addRangeResponseContent);

            var getAllResponse = await SystemUnderTests.GetAsync($"pricelists/{RepoActions.GetAll}");
            Assert.IsTrue(getAllResponse.IsSuccessStatusCode);
            var getAllResponseContent = await getAllResponse.Content.ReadAsStringAsync();
            var addedPriceLists = JsonConvert.DeserializeObject<IEnumerable<PriceList>>(getAllResponseContent);

            foreach (var priceListId in addedPriceListsIds)
            {
                var expectedPriceList = priceListsToAdd.Single(o => o.Id == priceListId);
                var actualPriceList = addedPriceLists.Single(o => o.Id == priceListId);
                AssertObjectsAreEqual(expectedPriceList, actualPriceList);
            }

            var jsonContentRemove = JsonConvert.SerializeObject(addedPriceLists.Select(priceList => priceList.Id));
            var httpContentRemove = new StringContent(jsonContentRemove, Encoding.UTF8, "application/json");
            var removeResponse = await SystemUnderTests.PostAsync($"pricelists/{RepoActions.RemoveRange}", httpContentRemove);
            Assert.IsTrue(removeResponse.IsSuccessStatusCode);

            var aricleJsonContentRemove = JsonConvert.SerializeObject(article.Id);
            var articleHttpContentRemove = new StringContent(aricleJsonContentRemove, Encoding.UTF8, "application/json");
            var articleRemoveResponse = await SystemUnderTests.PostAsync($"articles/{RepoActions.Remove}", articleHttpContentRemove);
            Assert.IsTrue(articleRemoveResponse.IsSuccessStatusCode);
        }

        [Test]
        public async Task GIVEN_PriceListsController_WHEN_I_update_entuty_THEN_it_is_being_updated_in_database()
        {
            var article = Fixture.Build<Article>().Create();

            var addResponseJsonContent = JsonConvert.SerializeObject(article);
            var addResponseHttpContent = new StringContent(addResponseJsonContent, Encoding.UTF8, "application/json");
            var addResponse = await SystemUnderTests.PostAsync($"articles/{RepoActions.Add}", addResponseHttpContent);
            Assert.IsTrue(addResponse.IsSuccessStatusCode);

            var expected = Fixture.Build<PriceList>()
              .With(oa => oa.ArticleId, article.Id)
              .Create();

            var jsonContent = JsonConvert.SerializeObject(expected);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var priceListsAddResponse = await SystemUnderTests.PostAsync($"pricelists/{RepoActions.Add}", httpContent);
            Assert.IsTrue(priceListsAddResponse.IsSuccessStatusCode);

            expected.Name = Fixture.Create<string>();
            expected.Price = Fixture.Create<decimal>();
            expected.ValidFrom = Fixture.Create<DateTime>();
            expected.ValidTo = Fixture.Create<DateTime>();

            var jsonContentUpdate = JsonConvert.SerializeObject(expected);
            var httpContentUpdate = new StringContent(jsonContentUpdate, Encoding.UTF8, "application/json");
            var updateResponse = await SystemUnderTests.PostAsync($"pricelists/{RepoActions.Update}", httpContentUpdate);
            Assert.IsTrue(updateResponse.IsSuccessStatusCode);
            var updateResponseContent = await updateResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<PriceList>(updateResponseContent);

            AssertObjectsAreEqual(expected, actual);

            var jsonContentRemove = JsonConvert.SerializeObject(actual.Id);
            var httpContentRemove = new StringContent(jsonContentRemove, Encoding.UTF8, "application/json");
            var removeResponse = await SystemUnderTests.PostAsync($"pricelists/{RepoActions.Remove}", httpContentRemove);
            Assert.IsTrue(removeResponse.IsSuccessStatusCode);

            var aricleJsonContentRemove = JsonConvert.SerializeObject(article.Id);
            var articleHttpContentRemove = new StringContent(aricleJsonContentRemove, Encoding.UTF8, "application/json");
            var articleRemoveResponse = await SystemUnderTests.PostAsync($"articles/{RepoActions.Remove}", articleHttpContentRemove);
            Assert.IsTrue(articleRemoveResponse.IsSuccessStatusCode);
        }

        protected void AssertObjectsAreEqual(PriceList expected, PriceList actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Price, actual.Price);
            Assert.AreEqual(expected.ValidFrom, actual.ValidFrom);
            Assert.AreEqual(expected.ValidTo, actual.ValidTo);
        }
    }
}
