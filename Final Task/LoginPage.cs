using OpenQA.Selenium;

namespace WebDriver3.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement TxtName => _driver.FindElement(By.CssSelector("#user-name"));
        private IWebElement TxtPass => _driver.FindElement(By.CssSelector("#password"));
        private IWebElement ErrorMessage => _driver.FindElement(By.CssSelector("h3[data-test='error']"));

        public void EnterUsername(string username)
        {
            TxtName.SendKeys(username);  // Вводимо ім'я користувача
        }

        public void EnterPassword(string password)
        {
            TxtPass.SendKeys(password);  // Вводимо пароль
        }

        public void Submit()
        {
            TxtPass.SendKeys(Keys.Enter);  // Надсилаємо форму натисканням Enter
        }

        public string GetErrorMessage()
        {
            return ErrorMessage.Text;  // Отримуємо текст повідомлення про помилку
        }
    }

}
