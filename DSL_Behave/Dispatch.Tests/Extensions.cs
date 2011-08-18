using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Dispatch.Tests
{
    public static class Extensions
    {
        public static void ShouldBe(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void ShouldNotBe(this object actual, object expected)
        {
            Assert.AreNotEqual(expected, actual);
        }

        public static void ShouldContain(this IList<object> actual, object expected)
        {
            Assert.IsTrue(actual.Any(i => i.Equals(expected)));
        }
    }
}
