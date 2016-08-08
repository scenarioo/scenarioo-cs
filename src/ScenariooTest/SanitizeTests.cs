using NUnit.Framework;

using Scenarioo;

using Shouldly;

namespace ScenariooTest
{
    [TestFixture]
    public class SanitizeTests
    {
        [Test]
        public void Remove_Diacritics_A()
        {
            "áàäâ".RemoveDiacritics().ShouldBe("aaaa");
        }

        [Test]
        public void Remove_Diacritics_E()
        {
            "éèëê".RemoveDiacritics().ShouldBe("eeee");
        }

        [Test]
        public void Remove_Diacritics_O()
        {
            "óòöô".RemoveDiacritics().ShouldBe("oooo");
        }

        [Test]
        public void Remove_Diacritics_s()
        {
            "ß".RemoveDiacritics().ShouldBe("s");
        }

        [Test]
        public void Remove_Diacritics_C()
        {
            "ç".RemoveDiacritics().ShouldBe("c");
        }
    }
}