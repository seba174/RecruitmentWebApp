using System;
using OpenQA.Selenium.Firefox;
using Xunit;
using Microsoft.Extensions.Configuration;

namespace RecruitmentApp.UITests
{
    public class JobOfferControllerUITests : IDisposable
    {
        private readonly FirefoxDriver driver;

        public static IConfiguration Configuration { get; } = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
        const string UISectionInAppSettings = "UITests";

        public JobOfferControllerUITests()
        {
            FirefoxOptions firefoxOptions = new FirefoxOptions
            {
                AcceptInsecureCertificates = true
            };

            driver = new FirefoxDriver(@"C:\Selenium\Firefox", firefoxOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(6);
        }

        public void Dispose()
        {
            driver.Close();
        }

        [Fact]
        public void ClikOnCreateJobOfferButtonMovesToCorrectView()
        {
            Login();

            string baseUrl = "https://localhost:44313/JobOffer";
            driver.Navigate().GoToUrl(baseUrl);
            driver.FindElementById("createJobOffer").Click();

            Assert.Equal(baseUrl + "/Create", driver.Url);
        }

        [Fact]
        public void DescriptionFieldValidationOnCreatingJobOfferInformsAboutErrorsWhenUserEntersTooFewCharacters()
        {
            Login();

            driver.Navigate().GoToUrl("https://localhost:44313/JobOffer/Create");
            driver.FindElementById("Description").SendKeys("String less than 100 characters long");
            driver.FindElementByClassName("navbar").Click();
            var invalidElements = driver.FindElementsByCssSelector(".field-validation-error");

            Assert.Single(invalidElements);
        }

        [Fact]
        public void ValidErrorIsShownOnCreatingJobOfferWithSalaryFromGreaterThanSalaryTo()
        {
            Login();

            driver.Navigate().GoToUrl("https://localhost:44313/JobOffer/Create");
            driver.FindElementById("SalaryTo").SendKeys("10");
            driver.FindElementById("SalaryFrom").SendKeys("100");
            driver.FindElementByClassName("navbar").Click();
            var invalidElements = driver.FindElementsByCssSelector(".field-validation-error");

            Assert.Single(invalidElements);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void ValidErrorIsShownOnCreatingJobOfferWithSalaryFromLowerOrEqualZero(int salaryFrom)
        {
            Login();

            driver.Navigate().GoToUrl("https://localhost:44313/JobOffer/Create");
            driver.FindElementById("SalaryFrom").SendKeys(salaryFrom.ToString());
            driver.FindElementByClassName("navbar").Click();
            var invalidElements = driver.FindElementsByCssSelector(".field-validation-error");

            Assert.Single(invalidElements);
        }

        private void Login()
        {
            driver.Navigate().GoToUrl("https://localhost:44313/");
            driver.FindElementById("loginButton").Click();
            driver.FindElementById("logonIdentifier").SendKeys(Configuration.GetSection(UISectionInAppSettings)["Login"]);
            driver.FindElementById("password").SendKeys(Configuration.GetSection(UISectionInAppSettings)["Password"]);
            driver.FindElementById("next").Click();
            driver.FindElementById("logoutButton");
        }
    }
}
