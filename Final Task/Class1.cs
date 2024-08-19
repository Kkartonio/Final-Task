using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using FluentAssertions;
using log4net;
using log4net.Config;
using System;
using System.IO;

namespace WebDriver3
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class WebDriverTests
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebDriverTests));

        [OneTimeSetUp]
        public void SetupLogging()
        {
            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        [Test]
        [TestCase("", "anypassword", "Username is required", TestName = "Test1 - Empty Username")]
        [TestCase("anyuser", "", "Password is required", TestName = "Test2 - Empty Password")]
        [TestCase("standard_user", "secret_sauce", "Swag Labs", TestName = "Test3 - Valid Login")]
        public void ChromeLoginTests(string username, string password, string expectedMessage)
        {
            IWebDriver driver = new ChromeDriver();
            ExecuteLoginTest(driver, username, password, expectedMessage);
        }

        [Test]
        [TestCase("", "anypassword", "Username is required", TestName = "Test1 - Empty Username")]
        [TestCase("anyuser", "", "Password is required", TestName = "Test2 - Empty Password")]
        [TestCase("standard_user", "secret_sauce", "Swag Labs", TestName = "Test3 - Valid Login")]
        public void EdgeLoginTests(string username, string password, string expectedMessage)
        {
            IWebDriver driver = new EdgeDriver();
            ExecuteLoginTest(driver, username, password, expectedMessage);
        }

        private void ExecuteLoginTest(IWebDriver driver, string username, string password, string expectedMessage)
        {
            try
            {
                log.Info("Starting test with Username: " + username + " and Password: " + password);

                // Given: The user is on the login page
                driver.Navigate().GoToUrl("https://www.saucedemo.com/");
                driver.Manage().Window.Maximize();

                // When: The user enters username and password
                IWebElement txtName = driver.FindElement(By.CssSelector("#user-name"));
                txtName.SendKeys(username);

                IWebElement txtPass = driver.FindElement(By.CssSelector("#password"));
                txtPass.SendKeys(password);

                // And: The user submits the form
                txtPass.SendKeys(Keys.Enter);

                // Then: The system should display the appropriate message or redirect to the dashboard
                if (string.IsNullOrEmpty(username))
                {
                    ValidateErrorMessage(driver, expectedMessage);
                }
                else if (string.IsNullOrEmpty(password))
                {
                    ValidateErrorMessage(driver, expectedMessage);
                }
                else
                {
                    ValidateTitle(driver, expectedMessage);
                }
            }
            catch (Exception ex)
            {
                log.Error("Test failed with exception: ", ex);
            }
            finally
            {
                driver?.Quit();
                driver?.Dispose();
            }
        }

        private void ValidateErrorMessage(IWebDriver driver, string expectedMessage)
        {
            try
            {
                IWebElement errorMessage = driver.FindElement(By.CssSelector("h3[data-test='error']"));
                errorMessage.Text.Should().Contain(expectedMessage);
                log.Info($"Error message validated: '{expectedMessage}'");
            }
            catch (NoSuchElementException ex)
            {
                log.Error("Error message not found.", ex);
            }
        }

        private void ValidateTitle(IWebDriver driver, string expectedTitle)
        {
            try
            {
                string actualTitle = driver.Title;
                actualTitle.Should().Be(expectedTitle);
                log.Info($"Title validated: '{expectedTitle}'");
            }
            catch (NoSuchElementException ex)
            {
                log.Error("Title element not found.", ex);
            }
        }
    }
}
