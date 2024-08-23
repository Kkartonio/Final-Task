using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using TechTalk.SpecFlow;
using BoDi;
using NUnit.Framework;

namespace WebDriver3.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void InitializeWebDriver()
        {
            string browser = TestContext.Parameters.Get("browser", "Edge");

            switch (browser.ToLower())
            {
                case "chrome":
                    _driver = new ChromeDriver();
                    break;
                case "edge":
                    _driver = new EdgeDriver();
                    break;
                default:
                    throw new ArgumentException($"Unsupported browser: {browser}");
            }

            _objectContainer.RegisterInstanceAs<IWebDriver>(_driver);
        }

        [AfterScenario]
        public void CloseWebDriver()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}