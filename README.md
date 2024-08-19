# WebDriver3 Test Automation Project

## Project Overview

This project automates the testing of the login functionality on the Sauce Demo website using Selenium WebDriver with NUnit. The tests cover various scenarios, including empty credentials, missing password, and valid login credentials, across multiple browsers (Chrome and Edge). The project also supports parallel test execution, logging, and parameterized tests.

## Task Description

The task involves testing the following use cases (UC) for the login form on the Sauce Demo website:

1. **UC-1: Test Login form with empty credentials**
   - Enter any credentials into the "Username" and "Password" fields.
   - Clear the inputs.
   - Hit the "Login" button.
   - Verify the error message: "Username is required".

2. **UC-2: Test Login form with missing password**
   - Enter any credentials in the "Username" field.
   - Enter a password.
   - Clear the "Password" input.
   - Hit the "Login" button.
   - Verify the error message: "Password is required".

3. **UC-3: Test Login form with valid credentials**
   - Enter a valid username listed under the "Accepted usernames" section.
   - Enter the password `secret_sauce`.
   - Click on "Login" and validate that the title of the dashboard page is “Swag Labs”.

## Technologies Used

- **Test Automation Tool:** Selenium WebDriver
- **Test Runner:** NUnit
- **Browsers:** 
  - Chrome
  - Edge
- **Locators:** CSS Selectors
- **Assertions:** FluentAssertions
- **Logging:** Log4Net (optional)
- **Patterns (optional):**
  - Singleton
  - Builder
  - Decorator
- **Test Automation Approach:** BDD (Behavior-Driven Development)
- **Parallel Execution:** Supported via NUnit’s `[Parallelizable]` attribute

## Project Structure

- `WebDriverTests.cs`: Contains the test methods for Chrome and Edge browsers. The tests are parameterized using NUnit's `[TestCase]` attribute.
- `ExecuteLoginTest`: A helper method that encapsulates the login logic and validation steps.
- `ValidateErrorMessage`: Validates the error messages displayed for incorrect login attempts.
- `ValidateTitle`: Confirms that the dashboard title is correct upon successful login.

## How to Run the Tests

1. **Prerequisites:**
   - Ensure that you have ChromeDriver and EdgeDriver installed and accessible in your system’s PATH.
   - Install the required NuGet packages: `NUnit`, `Selenium.WebDriver`, `FluentAssertions`, and `Log4Net` (if logging is enabled).

2. **Running the Tests:**
   - Use the NUnit test runner to execute the tests.
   - The tests are designed to run in parallel to speed up execution time.

3. **Logging:**
   - If Log4Net is configured, logs will be generated for each test run, providing detailed information about test execution.

## Conclusion

This project serves as a robust framework for testing login functionality using Selenium WebDriver and NUnit. It ensures thorough coverage of common login scenarios and supports advanced features like parallel execution and logging, making it a scalable solution for test automation.
