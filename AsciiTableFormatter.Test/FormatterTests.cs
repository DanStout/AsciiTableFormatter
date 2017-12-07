using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AsciiTableFormatter.Test
{
    [TestClass]
    public class FormatterTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var test = new Test();
            var actual = Formatter.Format(new[] { test });

            var expected = @"
+----------+
| FieldOne |
+----------+
| Hello    |
+----------+";

            Assert.AreEqual(expected.Trim(), actual);
        }


        class Test
        {
            public string FieldOne
            {
                get
                {
                    return "Hello";
                }
            }
        }
    }
}
