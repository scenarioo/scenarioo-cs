using System;
using System.Windows.Forms;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

using Samples.PizzaDelivery.PageObjects;
using Samples.PizzaDelivery.PageObjects.Infrastructure;

using Xunit;

namespace Samples.PizzaDelivery
{
    public class OrderPizzaTests : IDisposable
    {
        private readonly ChromeDriver _driver = new ChromeDriver();

        public OrderPizzaTests()
        {
            PositionWindowOnSecondScreenIfExists(_driver);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _driver.Dispose();
        }

        [Fact]
        public void OrderPizzaPlusRedWine()
        {
            var documentationContext = new DocumentationContext(_driver, "OrderPizza", "Main");

            new HomePage(_driver, documentationContext)
                .Navigate()
                .EnterPhoneNumber("0791111111")
                .ClickNext();

            new ConfirmAddressPage(_driver, documentationContext)
                .AssertDisplayed()
                .ClickYes();
            
            // TODO create page objects
            _driver.FindElement(By.Id("v")).Click();
            _driver.FindElement(By.Id("step-selectPizza")).FindElement(By.ClassName("next")).Click();

            _driver.FindElement(By.Id("dv")).Click();
            _driver.FindElement(By.Id("step-selectDrinks")).FindElement(By.ClassName("next")).Click();

            Assert.Equal("Pizza Verdura", _driver.FindElement(By.Id("summary_pizza")).Text);
            Assert.Equal("Vino Rosso", _driver.FindElement(By.Id("summary_drinks")).Text);

            _driver.FindElement(By.Id("step-summary")).FindElement(By.ClassName("next")).Click();

            Assert.True(_driver.FindElement(By.Id("step-confirmation")).Displayed);
        }

        private static void PositionWindowOnSecondScreenIfExists(IWebDriver driver)
        {
            if (Screen.AllScreens.Length <= 1)
            {
                return;
            }

            foreach (var screen in Screen.AllScreens)
            {
                if (!screen.Equals(Screen.PrimaryScreen))
                {
                    // TODO check why this does not work
                    driver.Manage().Window.Maximize();
                    break;
                }
            }
        }
    }
}