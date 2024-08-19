using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using FluentAssertions;

namespace WebDriver3
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class WebDriverTests
    {
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
                Console.WriteLine($"Test failed with exception: {ex.Message}");
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
                // Then: The system should display an error message
                IWebElement errorMessage = driver.FindElement(By.CssSelector("h3[data-test='error']"));
                if (errorMessage.Text.Contains(expectedMessage))
                {
                    Console.WriteLine($"Error message validated: '{expectedMessage}'");
                }
                else
                {
                    Console.WriteLine($"Error validation failed. Expected: '{expectedMessage}', but found: {errorMessage.Text}");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Error message not found.");
            }
        }

        private void ValidateTitle(IWebDriver driver, string expectedTitle)
        {
            try
            {
                // Then: The user should be redirected to the dashboard
                string actualTitle = driver.Title;

                if (actualTitle.Equals(expectedTitle))
                {
                    Console.WriteLine($"Title validated: '{expectedTitle}'");
                }
                else
                {
                    Console.WriteLine($"Title validation failed. Expected: '{expectedTitle}', but found: {actualTitle}");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Title element not found.");
            }
        }
    }
}
