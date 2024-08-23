using log4net;
using log4net.Config;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.IO;

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
}