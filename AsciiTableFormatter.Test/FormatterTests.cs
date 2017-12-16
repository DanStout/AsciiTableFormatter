using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace AsciiTableFormatter.Test
{
    [TestClass]
    public class FormatterTests
    {
        [TestMethod]
        public void TestOneRowOneColumn()
        {
            var test = new TestOne();
            var actual = Formatter.Format(new[] { test });
            Debug.WriteLine(actual);
            var expected = Clean(@"
o----------o
| FieldOne |
o----------o
| Hello    |
o----------o
");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestTwoRowsOneColumn()
        {
            var list = new[] { new TestOne(), new TestOne() };
            var actual = Formatter.Format(list);
            var expected = Clean(@"
o----------o
| FieldOne |
o----------o
| Hello    |
| Hello    |
o----------o
");
        }

        [TestMethod]
        public void TestOneRowTwoColumns()
        {
            var expected = Clean(@"
o-------o------o
| One   | Two  |
o-------o------o
| Three | Four |
o-------o------o
");
            var test = new TestTwo();
            var actual = Formatter.Format(new[] { test });
            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void TestIntegers()
        {
            var expected = Clean(@"
o----------o
|      One |
o----------o
|        1 |
|        2 |
| 23984734 |
o----------o
");

            var test1 = new TestThree()
            {
                One = 1
            };

            var test2 = new TestThree()
            {
                One = 2
            };

            var test3 = new TestThree()
            {
                One = 23984734
            };

            var actual = Formatter.Format(new[] { test1, test2, test3 });
            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestNullString()
        {
            var expected = Clean(@"
o-----------o---------o
|       Int | String  |
o-----------o---------o
|         1 | Bazinga |
|         6 | Bobo    |
| 943495740 |         |
o-----------o---------o
");
            var list = new[]
            {
                new TestFour()
                {
                    Int = 1,
                    String = "Bazinga"
                },
                new TestFour()
                {
                    Int = 6,
                    String = "Bobo"
                },
                new TestFour()
                {
                    Int = 0943495740,
                    String = null
                }
            };

            var actual = Formatter.Format(list);
            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestBoolAndDateTime()
        {
            var expected = Clean(@"
o-------o-----------------------------------------------------------------o------------------------o
| Bool  | Test                                                            | Date                   |
o-------o-----------------------------------------------------------------o------------------------o
| True  | true                                                            | 0001-01-01 12:00:00 AM |
| False | 918                                                             | 2017-12-06 9:52:45 PM  |
| True  | Hello, World! I'm a long text description. It just keeps going! | 1992-03-05 7:08:09 AM  |
o-------o-----------------------------------------------------------------o------------------------o
");
            var list = new[]
            {
                new TestFive()
                {
                    Bool = true,
                    Test = "true"
                },
                new TestFive()
                {
                    Bool = false,
                    Test = "918",
                    Date = new DateTime(2017, 12, 6, 21, 52, 45)
                },
                new TestFive()
                {
                    Bool = true,
                    Test = "Hello, World! I'm a long text description. It just keeps going!",
                    Date = new DateTime(1992, 3, 5, 7, 8, 9, DateTimeKind.Utc)
                }
            };

            var actual = Formatter.Format(list);
            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestStruct()
        {
            var expected = Clean(@"
o--------o-----o------------------o
|    One | Two |            Three |
o--------o-----o------------------o
|      3 | Hi  |           123.34 |
| 385745 | ??? | 84745.3498489754 |
o--------o-----o------------------o
");
            var list = new[]
            {
                new TestSix()
                {
                    One = 3,
                    Two = "Hi",
                    Three = 123.34M
                },
                new TestSix()
                {
                    One = 385745,
                    Two = "???",
                    Three = 84745.3498489754M
                }
            };
            var actual = Formatter.Format(list);
            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPrimitiveInts()
        {
            var expected = Clean(@"
o------------o
| 1567867876 |
|          2 |
|          3 |
|     423454 |
o------------o
");
            var list = new[] { 1567867876, 2, 3, 423454 };
            var actual = Formatter.Format(list);
            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPrimitiveStrings()
        {
            var expected = Clean(@"
o-------o
| Hi    |
| There |
| Billy |
| Bob   |
| Joe   |
o-------o
");
            var list = new[] { "Hi", "There", "Billy", "Bob", "Joe" };
            var actual = Formatter.Format(list);
            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPrimitiveMix()
        {
            var expected = Clean(@"
o------o
| Hi   |
| 123  |
| 15   |
| 2323 |
o------o
");

            var list = new object[] { "Hi", 123, 15,2323};
            var actual = Formatter.Format(list);
            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }

        private string Clean(string str)
        {
            return str.TrimStart().Replace("\r\n", "\n");
        }

        class TestOne
        {
            public string FieldOne
            {
                get
                {
                    return "Hello";
                }
            }
        }

        class TestTwo
        {
            public string One
            {
                get
                {
                    return "Three";
                }
            }

            public string Two
            {
                get
                {
                    return "Four";
                }
            }
        }

        class TestThree
        {
            public int One { get; set; }
        }

        class TestFour
        {
            public int Int { get; set; }
            public string String { get; set; }
        }

        class TestFive
        {
            public bool Bool { get; set; }
            public string Test { get; set; }
            public DateTime Date { get; set; }
        }

        struct TestSix
        {
            public int One { get; set; }
            public string Two { get; set; }
            public decimal Three { get; set; }
        }
    }
}
