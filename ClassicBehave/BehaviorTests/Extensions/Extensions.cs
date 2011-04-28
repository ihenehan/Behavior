using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Behavior.Tests.Extensions
{
    public static class Extensions
    {
        public static void ShouldBe(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void ShouldNotBe(this object actual, object expected)
        {
            Assert.IsFalse(actual.Equals(expected));
        }

        public static void ShouldContain(this List<object> actual, params object[] expected)
        {
            foreach (object o in expected)
                Assert.IsTrue(actual.Contains(o));
        }

        public static void ShouldContain(this List<string> actual, params object[] expected)
        {
            foreach (object o in expected)
                Assert.IsTrue(actual.Contains(o));
        }

        public static void ShouldContain(this Dictionary<string, string> actual, params object[] expected)
        {
            foreach (object s in expected)
                Assert.IsTrue(actual.Any(a => a.Equals(s)));
        }

        public static void ShouldBeNull(this List<string> actual)
        {
            Assert.IsNull(actual);
        }

        public static void ShouldBeEmpty(this List<object> actual)
        {
            Assert.AreEqual(actual.Count(), 0);
        }

        public static void ShouldBeEmpty(this List<string> actual)
        {
            Assert.AreEqual(actual.Count(), 0);
        }

        public static void ShouldBeEmpty(this string actual)
        {
            Assert.IsTrue(string.IsNullOrEmpty(actual));
        }

        public static void ShouldBeEmpty(this Dictionary<string, string> actual)
        {
            Assert.AreEqual(0, actual.Count());
        }
    }
}
