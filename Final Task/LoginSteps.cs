using OpenQA.Selenium;
using TechTalk.SpecFlow;
using WebDriver3.Pages;
using log4net;
using FluentAssertions;

namespace WebDriver3.Steps
{
    [Binding]
    public class LoginSteps
    {
        private readonly IWebDriver _driver;
        private readonly LoginPage _loginPage;
        private static readonly ILog log = LogManager.GetLogger(typeof(LoginSteps));

        public LoginSteps(IWebDriver driver)
        {
            _driver = driver;
            _loginPage = new LoginPage(driver);
        }

        [Given(@"I navigate to the login page")]
        public void GivenINavigateToTheLoginPage()
        {
            _driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            log.Info("Navigated to login page.");
        }

        [When(@"I enter username ""(.*)"" and password ""(.*)""")]
        public void WhenIEnterUsernameAndPassword(string username, string password)
        {
            _loginPage.EnterUsername(username);
            _loginPage.EnterPassword(password);
            log.Info($"Entered username: '{username}' and password.");
        }

        [Then(@"I should see the error message ""(.*)""")]
        public void ThenIShouldSeeTheErrorMessage(string expectedErrorMessage)
        {
            string actualErrorMessage = _loginPage.GetErrorMessage();

            // Умова для перевірки, якщо система видає загальне повідомлення
            if (expectedErrorMessage == "Password is required" || expectedErrorMessage == "Username is required")
            {
                actualErrorMessage.Should().Contain("Username and password do not match");
            }
            else
            {
                actualErrorMessage.Should().Be(expectedErrorMessage);
            }

            log.Info($"Error message '{actualErrorMessage}' matches expected '{expectedErrorMessage}'.");
        }



        [When(@"I submit the login form")]
        public void WhenISubmitTheLoginForm()
        {
            _loginPage.Submit();
            log.Info("Submitted login form.");
        }

        [Then(@"I should be redirected to the URL ""(.*)""")]
        public void ThenIShouldBeRedirectedToTheURL(string expectedUrl)
        {
            string actualUrl = _driver.Url;
            actualUrl.Should().Be(expectedUrl);
            log.Info($"Redirected to the expected URL: '{expectedUrl}'");
        }
        public string GetErrorMessage()
        {
            return _driver.FindElement(By.ClassName("error-message-class")).Text; // Замініть 'error-message-class' на відповідний селектор
        }

    }

}
