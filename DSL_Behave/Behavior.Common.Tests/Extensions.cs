using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Common.Models;

namespace Behavior.Common.Tests
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

        public static void ShouldContain(this List<string> actual, params string[] expected)
        {
            foreach (string s in expected)
                Assert.IsTrue(actual.Contains(s));
        }

        public static void ShouldContain(this List<object> actual, params object[] expected)
        {
            foreach( object o in expected)
                Assert.IsTrue(actual.Contains(o));
        }
    }
}
