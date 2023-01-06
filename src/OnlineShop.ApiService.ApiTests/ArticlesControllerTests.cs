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
    public class ArticlesControllerTests : BaseRepoControllerTests
    {
        public ArticlesControllerTests() : base() { }

        [Test]
        public async virtual Task GIVEN_ArticlesController_WHEN_I_add_entity_THEN_it_is_being_added_to_database()
        {
            var expected = Fixture.Build<Article>().Create();

            var addResponseJsonContent = JsonConvert.SerializeObject(expected);
            var addResponseHttpContent = new StringContent(addResponseJsonContent, Encoding.UTF8, "application/json");
            var addResponse = await SystemUnderTests.PostAsync($"articles/{RepoActions.Add}", addResponseHttpContent);
            Assert.IsTrue(addResponse.IsSuccessStatusCode);

            var getOneResponse = await SystemUnderTests.GetAsync($"articles?Id={expected.Id}");
            Assert.IsTrue(getOneResponse.IsSuccessStatusCode);
            var getOneResponseContent = await getOneResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<Article>(getOneResponseContent);
            AssertObjectsAreEqual(expected, actual);


            var jsonContentRemove = JsonConvert.SerializeObject(actual.Id);
            var httpContentRemove = new StringContent(jsonContentRemove, Encoding.UTF8, "application/json");
            var removeResponse = await SystemUnderTests.PostAsync($"articles/{RepoActions.Remove}", httpContentRemove);
            Assert.IsTrue(removeResponse.IsSuccessStatusCode);
        }

        [Test]
        public async virtual Task GIVEN_ArticlesController_WHEN_I_add_several_entities_THEN_it_is_being_added_to_database()
        {
            var expected1 = Fixture.Build<Article>().Create();
            var expected2 = Fixture.Build<Article>().Create();

            var articlesToAdd = new[] { expected1, expected2 };
            var jsonContent = JsonConvert.SerializeObject(articlesToAdd);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var addRangeResponse = await SystemUnderTests.PostAsync($"articles/{RepoActions.AddRange}", httpContent);
            Assert.IsTrue(addRangeResponse.IsSuccessStatusCode);
            var addRangeResponseContent = await addRangeResponse.Content.ReadAsStringAsync();
            var addedArticlesIds = JsonConvert.DeserializeObject<IEnumerable<Guid>>(addRangeResponseContent);

            var getAllResponse = await SystemUnderTests.GetAsync($"articles/{RepoActions.GetAll}");
            Assert.IsTrue(getAllResponse.IsSuccessStatusCode);
            var getAllResponseContent = await getAllResponse.Content.ReadAsStringAsync();
            var addedArticles = JsonConvert.DeserializeObject<IEnumerable<Article>>(getAllResponseContent);

            foreach (var articleId in addedArticlesIds)
            {
                var expectedArticle = articlesToAdd.Single(o => o.Id == articleId);
                var actualArticle = addedArticles.Single(o => o.Id == articleId);
                AssertObjectsAreEqual(expectedArticle, actualArticle);
            }

            var jsonContentRemove = JsonConvert.SerializeObject(addedArticles.Select(order => order.Id));
            var httpContentRemove = new StringContent(jsonContentRemove, Encoding.UTF8, "application/json");
            var removeResponse = await SystemUnderTests.PostAsync($"articles/{RepoActions.RemoveRange}", httpContentRemove);
            Assert.IsTrue(removeResponse.IsSuccessStatusCode);
        }

        [Test]
        public async virtual Task GIVEN_ArticlesController_WHEN_I_update_entuty_THEN_it_is_being_updated_in_database()
        {
            var expected = Fixture.Build<Article>().Create();

            var jsonContent = JsonConvert.SerializeObject(expected);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var addResponse = await SystemUnderTests.PostAsync($"articles/{RepoActions.Add}", httpContent);
            Assert.IsTrue(addResponse.IsSuccessStatusCode);

            expected.Name = Fixture.Create<string>();
            expected.Description = Fixture.Create<string>();

            var jsonContentUpdate = JsonConvert.SerializeObject(expected);
            var httpContentUpdate = new StringContent(jsonContentUpdate, Encoding.UTF8, "application/json");
            var updateResponse = await SystemUnderTests.PostAsync($"articles/{RepoActions.Update}", httpContentUpdate);
            Assert.IsTrue(updateResponse.IsSuccessStatusCode);
            var updateResponseContent = await updateResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<Article>(updateResponseContent);

            AssertObjectsAreEqual(expected, actual);

            var jsonContentRemove = JsonConvert.SerializeObject(actual.Id);
            var httpContentRemove = new StringContent(jsonContentRemove, Encoding.UTF8, "application/json");
            var removeResponse = await SystemUnderTests.PostAsync($"articles/{RepoActions.Remove}", httpContentRemove);
            Assert.IsTrue(removeResponse.IsSuccessStatusCode);
        }

        private void AssertObjectsAreEqual(Article expected, Article actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Description, actual.Description);
        }
    }
}
