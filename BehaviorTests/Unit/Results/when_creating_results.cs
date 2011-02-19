using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Remote.Results;
using BehaviorTests.Extensions;

namespace BehaviorTests.Unit.Results
{
    [TestFixture]
    public class when_creating_results
    {
        [Test]
        public void given_pass_and_no_message()
        {
            var result = Result.CreatePass();

            result.status.ShouldBe("PASS");
        }

        [Test]
        public void given_pass_with_simple_return_object()
        {
            var result = Result.CreatePass("foobar");

            result.status.ShouldBe("PASS");

            result.retrn.ShouldBe("foobar");
        }

        [Test]
        public void given_pass_with_complex_return_object()
        {
            var dictionary = new Dictionary<string, int>();

            dictionary.Add("foobar", 123);

            var result = Result.CreatePass(dictionary);

            result.status.ShouldBe("PASS");

            result.retrn.ShouldBe(dictionary);
        }

        [Test]
        public void given_fail_with_no_error_message()
        {
            var result = Result.CreateFail();

            result.status.ShouldBe("FAIL");

            result.error.ShouldBe("");
        }

        [Test]
        public void given_fail_with_error_message()
        {
            var result = Result.CreateFail("foobar");

            result.status.ShouldBe("FAIL");

            result.error.ShouldBe("foobar");
        }

        [Test]
        public void given_create_stubbed()
        {
            var result = Result.CreateStubbed("foobar");

            result.status.ShouldBe("FAIL");

            result.error.ShouldBe("KEYWORD_STUBBED: foobar");
        }
    }
}
