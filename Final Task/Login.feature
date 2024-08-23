Feature: Login

  Scenario: Valid Login
    Given I navigate to the login page
    When I enter username "standard_user" and password "secret_sauce"
    And I submit the login form
    Then I should be redirected to the URL "https://www.saucedemo.com/inventory.html"

  Scenario Outline: Invalid Login
    Given I navigate to the login page
    When I enter username "<username>" and password "<password>"
    And I submit the login form
    Then I should see the error message "<errorMessage>"

    Examples:
      | username       | password      | errorMessage            |
      | ""             | anypassword   | Username is required    |
      | anyuser        | ""            | Password is required    |
