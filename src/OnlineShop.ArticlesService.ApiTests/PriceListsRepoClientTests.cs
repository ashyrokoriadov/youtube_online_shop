using AutoFixture;
using IdentityModel.Client;
using NUnit.Framework;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.Clients.ArticlesService;
using OnlineShop.Library.Clients.IdentityServer;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineShop.ArticlesService.ApiTests
{
    public class PriceListsRepoClientTests : ArticleServiceRepoBaseApiTest<PriceListsClient, PriceList>
    {
        private ArticlesClient AriclesClient;
        private Guid _articleId;

        [Test]
        public async override Task GIVEN_Repo_Client_WHEN_I_add_entity_THEN_it_is_being_added_to_database()
        {
            var article = Fixture.Build<Article>().Create();

            var addArticleResponse = await AriclesClient.Add(article);
            Assert.IsTrue(addArticleResponse.IsSuccessfull);
            _articleId = article.Id;

            await base.GIVEN_Repo_Client_WHEN_I_add_entity_THEN_it_is_being_added_to_database();

            var removeArticleResponse = await AriclesClient.Remove(article.Id);
            Assert.IsTrue(removeArticleResponse.IsSuccessfull);
        }

        [Test]
        public async override Task GIVEN_Repo_Client_WHEN_I_add_several_entities_THEN_it_is_being_added_to_database()
        {
            var article = Fixture.Build<Article>().Create();

            var addArticleResponse = await AriclesClient.Add(article);
            Assert.IsTrue(addArticleResponse.IsSuccessfull);
            _articleId = article.Id;

            await base.GIVEN_Repo_Client_WHEN_I_add_several_entities_THEN_it_is_being_added_to_database();

            var removeArticleResponse = await AriclesClient.Remove(article.Id);
            Assert.IsTrue(removeArticleResponse.IsSuccessfull);
        }

        [Test]
        public async override Task GIVEN_Repo_Client_WHEN_I_update_entuty_THEN_it_is_being_updated_in_database()
        {
            var article = Fixture.Build<Article>().Create();

            var addArticleResponse = await AriclesClient.Add(article);
            Assert.IsTrue(addArticleResponse.IsSuccessfull);
            _articleId = article.Id;

            await base.GIVEN_Repo_Client_WHEN_I_update_entuty_THEN_it_is_being_updated_in_database();

            var removeArticleResponse = await AriclesClient.Remove(article.Id);
            Assert.IsTrue(removeArticleResponse.IsSuccessfull);
        }

        protected override void CreateSystemUnderTests()
        {
            SystemUnderTests = new PriceListsClient(new HttpClient(), ServiceAdressOptions);
            AriclesClient = new ArticlesClient(new HttpClient(), ServiceAdressOptions);
        }

        protected override async Task AuthorizeSystemUnderTests()
        {
            var token = await LoginClient.GetApiTokenByClientSeceret(IdentityServerApiOptions);
            SystemUnderTests.HttpClient.SetBearerToken(token.AccessToken);
            AriclesClient.HttpClient.SetBearerToken(token.AccessToken);
        }

        protected override void AssertObjectsAreEqual(PriceList expected, PriceList actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Price, actual.Price);
            Assert.AreEqual(expected.ValidFrom, actual.ValidFrom);
            Assert.AreEqual(expected.ValidTo, actual.ValidTo);
        }

        protected override void AmendExpectedEntityForUpdating(PriceList expected)
        {
            expected.Name = Fixture.Create<string>();
            expected.Price = Fixture.Create<decimal>();
            expected.ValidFrom = Fixture.Create<DateTime>();
            expected.ValidTo = Fixture.Create<DateTime>();
        }

        protected override PriceList CreateExpectedEntity() => Fixture.Build<PriceList>().With(e => e.ArticleId, _articleId).Create();
    }
}
