using OpenQA.Selenium;

using Samples.PizzaDelivery.PageObjects.Infrastructure;

using Xunit;

namespace Samples.PizzaDelivery.PageObjects
{
    public class ConfirmAddressPage
    {
        private readonly IWebDriver _driver;
        private readonly DocumentationContext _documentationContext;

        public ConfirmAddressPage(IWebDriver driver, DocumentationContext documentationContext)
        {
            _driver = driver;
            _documentationContext = documentationContext;
        }

        public ConfirmAddressPage AssertDisplayed()
        {
            Assert.True(_driver.FindElement(By.Id("step-confirmAddress")).Displayed);
            return this;
        }

        public void ClickYes()
        {
            _driver.FindElement(By.Id("step-confirmAddress")).FindElement(By.ClassName("yes")).Click();
            _documentationContext.SaveStep("Click Yes");
        }
    }
}