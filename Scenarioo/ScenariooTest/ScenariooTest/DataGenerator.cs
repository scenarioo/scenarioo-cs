using Scenarioo.Model.Docu.Entities;

namespace ScenariooTest
{
    public static class DataGenerator
    {
        public static ScreenAnnotation CreateScreenAnnotation(int x, int y, ScreenAnnotationStyle style, ScreenAnnotationClickAction? clickAction = null, string clickActionUrl = "")
        {
            return new ScreenAnnotation(new ScreenRegion(x, y, 200, 26))
            {
                Style = style,
                Title = style + "-title",
                ScreenText = "Screen text for " + style,
                Description = "Description text here.",
                ClickAction = clickAction,
                ClickActionUrl = clickActionUrl
            };
        }
    }
}