using System;
using OpenQA.Selenium.Firefox;
using Xunit;

namespace RecruitmentApp.UITests
{
    public class JobOfferControllerUITests : IDisposable
    {
        private readonly FirefoxDriver driver;

        public JobOfferControllerUITests()
        {
            FirefoxOptions firefoxOptions = new FirefoxOptions
            {
                AcceptInsecureCertificates = true
            };

            driver = new FirefoxDriver(@"C:\Selenium\Firefox", firefoxOptions);
        }

        public void Dispose()
        {
            driver.Quit();
        }

        [Fact]
        public void ClikOnCreateJobOfferButtonMovesToCorrectView()
        {
            string baseUrl = "https://localhost:44313/JobOffer";
            driver.Navigate().GoToUrl(baseUrl);
            driver.FindElementById("createJobOffer").Click();

            Assert.Equal(baseUrl + "/Create", driver.Url);
        }

        [Fact]
        public void DescriptionFieldValidationOnCreatingJobOfferInformsAboutErrorsWhenUserEntersTooFewCharacters()
        {
            driver.Navigate().GoToUrl("https://localhost:44313/JobOffer/Create");
            driver.FindElementById("Description").SendKeys("String less than 100 characters long");
            driver.FindElementByTagName("body").Click();
            var invalidElements = driver.FindElementsByCssSelector(".field-validation-error");

            Assert.Single(invalidElements);
        }

        [Fact]
        public void ValidErrorIsShownOnCreatingJobOfferWithSalaryFromGreaterThanSalaryTo()
        {
            driver.Navigate().GoToUrl("https://localhost:44313/JobOffer/Create");
            driver.FindElementById("SalaryTo").SendKeys("10");
            driver.FindElementById("SalaryFrom").SendKeys("100");
            driver.FindElementByTagName("body").Click();
            var invalidElements = driver.FindElementsByCssSelector(".field-validation-error");

            Assert.Single(invalidElements);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void ValidErrorIsShownOnCreatingJobOfferWithSalaryFromLowerOrEqualZero(int salaryFrom)
        {
            driver.Navigate().GoToUrl("https://localhost:44313/JobOffer/Create");
            driver.FindElementById("SalaryFrom").SendKeys(salaryFrom.ToString());
            driver.FindElementByTagName("body").Click();
            var invalidElements = driver.FindElementsByCssSelector(".field-validation-error");

            Assert.Single(invalidElements);
        }
    }
}
