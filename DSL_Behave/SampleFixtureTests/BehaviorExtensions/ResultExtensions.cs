using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Remote.Results;

namespace SampleFixtureTests.BehaviorExtensions
{
    public static class ResultExtensions
    {
        public static void ShouldPass(this Result result)
        {
            Assert.AreEqual("pass", result.status.ToLower(), result.error);
        }

        public static void ShouldFail(this Result result)
        {
            Assert.AreEqual("fail", result.status.ToLower(), result.error);
        }
    }
}
