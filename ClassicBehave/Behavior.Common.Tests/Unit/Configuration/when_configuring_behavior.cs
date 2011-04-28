using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Common.Configuration;

namespace Behavior.Common.Tests.Unit.Configuration
{
    [TestFixture]
    public class when_configuring_behavior
    {
        private BehaviorConfiguration config;

        [SetUp]
        public void Setup()
        {
            config = new BehaviorConfiguration();

            config.TestVariables.Add("${foo}", "bar");
        }

        [Test]
        public void given_constructor_call_tag_lists_should_not_be_null()
        {
            config.IncludeTags.ShouldNotBe(null);
            config.ExcludeTags.ShouldNotBe(null);
        }

        [Test]
        public void given_constructor_call_variable_list_should_not_be_null()
        {
            config.TestVariables.ShouldNotBe(null);
        }

        [Test]
        public void given_test_variable_should_replace_in_string_with_value()
        {
            var stringToUpdate = "Go to ${foo}.";

            config.InsertTestVariables(stringToUpdate).ShouldBe("Go to bar.");
        }

        [Test]
        public void given_missing_test_variable_should_return_original_string()
        {
            var stringToUpdate = "Go to ${missing}.";

            config.InsertTestVariables(stringToUpdate).ShouldBe("Go to ${missing}.");
        }
    }
}
