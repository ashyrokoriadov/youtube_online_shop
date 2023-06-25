using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;

namespace OnlineShop.Ui.SeleniumTests
{
    [TestFixture]
    public class ArticlesListTests
    {
        [Test]
        public void GIVEN_web_app_WHEN_I_add_article_to_cart_THEN_alert_message_is_shown()
        {
            var username = "andrey";
            var password = "Pass_123";

            IWebDriver driver = new FirefoxDriver();

            driver.Navigate().GoToUrl("https://localhost:7163");

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(5000);

            var usernameTextBox = driver.FindElement(By.Id("login-form-username"));
            usernameTextBox.SendKeys(username);

            var passwordTextBox = driver.FindElement(By.Id("login-form-password"));
            passwordTextBox.SendKeys(password);

            var submitButton = driver.FindElement(By.Id("login-form-submit-button"));
            submitButton.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(5000);

            var row = driver.FindElement(By.Id("44bc3931-3c6f-4a94-e4b3-08db71ad7813"));
            var quantityInput = row.FindElement(By.CssSelector("input[type=\"number\"]"));
            quantityInput.Click();
            quantityInput.SendKeys("2");

            var buyButton = row.FindElement(By.CssSelector("button[class~=buy]"));
            buyButton.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());

            IAlert alert = SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent().Invoke(driver);
            Assert.IsNotNull(alert);

            alert.Accept();

            driver.Quit();
        }
    }
}
