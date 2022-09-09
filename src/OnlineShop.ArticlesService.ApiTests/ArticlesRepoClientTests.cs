using AutoFixture;
using NUnit.Framework;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.Clients.ArticlesService;
using System.Net.Http;

namespace OnlineShop.ArticlesService.ApiTests
{
    public class ArticlesRepoClientTests : ArticleServiceRepoBaseApiTest<ArticlesClient, Article>
    {    
        protected override void CreateSystemUnderTests()
        {
            SystemUnderTests = new ArticlesClient(new HttpClient(), ServiceAdressOptions);
        }

        protected override void AssertObjectsAreEqual(Article expected, Article actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Description, actual.Description);
        }

        protected override void AmendExpectedEntityForUpdating(Article expected)
        {
            expected.Name = Fixture.Create<string>();
            expected.Description = Fixture.Create<string>();
        }
    }
}