using OpenQA.Selenium;

using Samples.PizzaDelivery.PageObjects.Infrastructure;

namespace Samples.PizzaDelivery.PageObjects
{
    public class HomePage
    {
        private readonly DocumentationContext _documentationContext;

        private readonly IWebDriver _driver;

        public HomePage(IWebDriver driver, DocumentationContext documentationContext)
        {
            _driver = driver;
            _documentationContext = documentationContext;
        }

        public HomePage Navigate()
        {
            _driver.Url = "http://scenarioo.org/pizza-delivery/prod/index.html";
            return this;
        }

        public HomePage EnterPhoneNumber(string phoneNumber)
        {
            var phoneNumberInput = _driver.FindElement(By.Id("phoneNumber"));

            phoneNumberInput.SendKeys(phoneNumber);
            _documentationContext.RecordElementKeyboardAnnotation(phoneNumberInput);

            _documentationContext.SaveStep("Enter phone number");
            return this;
        }

        public void ClickNext()
        {
            var nextButton = _driver.FindElement(By.Id("phoneNumber"));

            nextButton.FindElement(By.ClassName("next"));
            _documentationContext.RecordElementClickAnnotation(nextButton);

            _documentationContext.SaveStep("Click 'Next'");
        }
    }
}