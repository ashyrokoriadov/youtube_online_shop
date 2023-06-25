using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;

namespace OnlineShop.Ui.SeleniumTests
{
    [TestFixture]
    public class InitialTest
    {

        [Test]
        public void FirefoxSession()
        {
            IWebDriver driver = new FirefoxDriver();

            driver.Navigate().GoToUrl("https://www.selenium.dev/selenium/web/web-form.html");

            var title = driver.Title;
            Assert.AreEqual("Web form", title);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

            var textBox = driver.FindElement(By.Name("my-text"));
            var submitButton = driver.FindElement(By.TagName("button"));

            textBox.SendKeys("Selenium");
            submitButton.Click();

            var message = driver.FindElement(By.Id("message"));
            var value = message.Text;
            Assert.AreEqual("Received!", value);

            driver.Quit();
        }
    }
}