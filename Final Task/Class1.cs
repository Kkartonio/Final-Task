using FluentAssertions;
using log4net.Config;
using log4net;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

namespace WebDriver3
{
    public class TestBase
    {
        protected IWebDriver driver;
        protected static readonly ILog log = LogManager.GetLogger(typeof(TestBase));

        [OneTimeSetUp]
        public void SetupLogging()
        {
            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        [SetUp]
        public void InitDriver()
        {
            string browser = TestContext.Parameters.Get("browser", "Edge");

            switch (browser.ToLower())
            {
                case "chrome":
                    driver = new ChromeDriver();
                    break;
                case "edge":
                    driver = new EdgeDriver();
                    break;
                default:
                    throw new ArgumentException($"Unsupported browser: {browser}");
            }

            log.Info($"Driver initialized: {browser}");
        }

        [TearDown]
        public void CloseDriver()
        {
            driver?.Quit();
            driver?.Dispose();
            log.Info("Driver closed.");
        }
    }
    public class LoginPage
    {
        private readonly IWebDriver driver;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        private IWebElement TxtName => driver.FindElement(By.CssSelector("#user-name"));
        private IWebElement TxtPass => driver.FindElement(By.CssSelector("#password"));
        private IWebElement ErrorMessage => driver.FindElement(By.CssSelector("h3[data-test='error']"));

        public void EnterUsername(string username)
        {
            TxtName.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            TxtPass.SendKeys(password);
        }

        public void Submit()
        {
            TxtPass.SendKeys(Keys.Enter);
        }

        public string GetErrorMessage()
        {
            return ErrorMessage.Text;
        }
    }

    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class WebDriverTests : TestBase
    {
        [Test]
        [TestCase("standard_user", "secret_sauce", "https://www.saucedemo.com/inventory.html", TestName = "Test3 - Valid Login")]
        public void HappyLoginTest(string username, string password, string expectedUrl)
        {
            ExecuteLoginTest(username, password);
            ValidateUrl(expectedUrl);
        }

        [Test]
        [TestCase("", "anypassword", "Username is required", TestName = "Test1 - Empty Username")]
        [TestCase("anyuser", "", "Password is required", TestName = "Test2 - Empty Password")]
        public void NegativeLoginTest(string username, string password, string expectedMessage)
        {
            ExecuteLoginTest(username, password);
            ValidateErrorMessage(expectedMessage);
        }

        private void ExecuteLoginTest(string username, string password)
        {
            var loginPage = new LoginPage(driver);
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            loginPage.EnterUsername(username);
            loginPage.EnterPassword(password);
            loginPage.Submit();
        }

        private void ValidateErrorMessage(string expectedMessage)
        {
            var loginPage = new LoginPage(driver);
            string actualMessage = loginPage.GetErrorMessage();
            actualMessage.Should().Contain(expectedMessage);
            log.Info($"Error message validated: '{expectedMessage}'");
        }

        private void ValidateUrl(string expectedUrl)
        {
            string actualUrl = driver.Url;
            actualUrl.Should().Be(expectedUrl);
            log.Info($"URL validated: '{expectedUrl}'");
        }
    }
}
