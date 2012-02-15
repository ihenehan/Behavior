using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Tests
{
    public static class Extensions
    {
        public static void ShouldBe(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void ShouldContain(this List<string> actual, object expected)
        {
            Assert.Contains(expected, actual);
        }

        public static void ShouldNotContain(this List<string> actual, object expected)
        {
            Assert.False(actual.Contains(expected));
        }
    }
}
