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
        public static void ShouldEqual(this Project actual, Project expected)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void ShouldEqual(this Story actual, Story expected)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void ShouldBe(this object actual, object expected)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void ShouldNotBe(this object actual, object expected)
        {
            Assert.IsFalse(actual.Equals(expected));
        }
    }
}
