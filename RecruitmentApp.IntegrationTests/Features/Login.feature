Feature: Login
  In order to access my account
  As a user of the website
  I want to log into the website

Scenario: Logging in with valid credentials
  Given I am at the login page
  When I fill in the following form
  | field | value |
  | logonIdentifier | UsernameFromTestSettings |
  | password | PasswordFromTestSettings |
  And I click the login button
  Then I should be at the home page