using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;

namespace OnlineShop.Ui.SeleniumTests
{
    [TestFixture]
    public class LoginFormTests
    {
        [Test]
        public void GIVEN_web_app_WHEN_I_provide_correct_login_and_password_THEN_I_login_correctly()
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

            var welcomeMessage = driver.FindElement(By.Id("login-form-welcome-message"));

            Assert.AreEqual("Hello, Andriy !", welcomeMessage.Text);

            driver.Quit();
        }

        [Test]
        public void GIVEN_web_app_and_logged_in_user_WHEN_I_click_logout_button_THEN_I_logout_correctly()
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

            var logoutButton = driver.FindElement(By.Id("login-form-logout-button"));
            logoutButton.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(5000);

            usernameTextBox = driver.FindElement(By.Id("login-form-username"));
            Assert.IsTrue(usernameTextBox.Displayed);

            driver.Quit();
        }
    }
}
