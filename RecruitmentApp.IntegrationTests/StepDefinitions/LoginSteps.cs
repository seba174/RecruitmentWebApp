using System;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;
using Xunit;

namespace RecruitmentApp.IntegrationTests.StepDefinitions
{
    [Binding]
    public class LoginSteps
    {
        private const string baseUrl = "https://localhost:44313/";
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder().AddJsonFile("testsettings.json").Build();

        private readonly FirefoxDriver driver;
        private readonly ScenarioContext context;

        public LoginSteps(ScenarioContext injectedContext)
        {
            context = injectedContext;

            FirefoxOptions firefoxOptions = new FirefoxOptions
            {
                AcceptInsecureCertificates = true
            };

            driver = new FirefoxDriver(@"C:\Selenium\Firefox", firefoxOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(6);
        }

        [Given(@"I am at the login page")]
        public void GivenIAmAtTheLoginPage()
        {
            driver.Navigate().GoToUrl("https://localhost:44313/");
            driver.FindElementById("loginButton").Click();
        }

        [When(@"I fill in the following form")]
        public void WhenIFillInTheFollowingForm(Table table)
        {
            foreach (var row in table.Rows)
            {
                var field = driver.FindElementById(row["field"]);
                
                field.SendKeys(Configuration[row["value"]]);
            }
        }

        [When(@"I click the login button")]
        public void WhenIClickTheLoginButton()
        {
            driver.FindElementById("next").Click();
        }

        [Then(@"I should be at the home page")]
        public void ThenIShouldBeAtTheHomePage()
        {
            driver.FindElementById("logoutButton");
            Assert.Equal(baseUrl, driver.Url);

            driver.Close();
        }
    }
}
