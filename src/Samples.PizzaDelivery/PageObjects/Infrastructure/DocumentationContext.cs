using System.Collections.Generic;
using System.IO;
using System.Linq;

using OpenQA.Selenium;

using Scenarioo.Api;
using Scenarioo.Model.Docu.Entities;

namespace Samples.PizzaDelivery.PageObjects.Infrastructure
{
    public class DocumentationContext
    {
        private const string OutputDirectory = "ScenariooOutput";

        private readonly string _scenarioId;

        private readonly IList<ScreenAnnotation> _screenAnnotations = new List<ScreenAnnotation>();

        private readonly ITakesScreenshot _screenshotDriver;

        private readonly string _useCaseId;

        private readonly ScenarioDocuWriter _writer;

        private int _currentStep;
        
        public DocumentationContext(ITakesScreenshot screenshotDriver, string useCaseId, string scenarioId)
        {
            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }

            _writer = new ScenarioDocuWriter(OutputDirectory, "Branch", "Build");

            _screenshotDriver = screenshotDriver;
            _useCaseId = useCaseId;
            _scenarioId = scenarioId;
        }

        public void RecordElementKeyboardAnnotation(IWebElement element)
        {
            RecordElementAnnotation(element, ScreenAnnotationStyle.Keyboard);
        }

        public void RecordElementClickAnnotation(IWebElement element)
        {
            RecordElementAnnotation(element, ScreenAnnotationStyle.Click);
        }

        private void RecordElementAnnotation(IWebElement element, ScreenAnnotationStyle style)
        {
            var location = element.Location;
            var size = element.Size;
            var screenAnnotation = new ScreenAnnotation(new ScreenRegion(location.X, location.Y, size.Width, size.Height))
            {
                Style = style
            };

            _screenAnnotations.Add(screenAnnotation);
        }

        public void SaveStep(string title)
        {
            var step = new Step
                       {
                           Index = _currentStep,
                           Title = title,
                           ScreenAnnotations = _screenAnnotations.ToList()
                       };

            _writer.SaveStep(_useCaseId, _scenarioId, step);
            _writer.SaveScreenshot(_useCaseId, _scenarioId, step, _screenshotDriver.GetScreenshot().AsByteArray);

            _screenAnnotations.Clear();
            _currentStep++;
        }
    }
}