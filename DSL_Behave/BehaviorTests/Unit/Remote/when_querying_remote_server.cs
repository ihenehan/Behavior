using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Remote;
using Behavior.Remote.Server;
using Behavior.Remote.Results;
using Behavior.Tests.Extensions;
using Behavior.Remote.Attributes;
using System.Reflection;

namespace Behavior.Tests.Unit.Remote
{
    [TestFixture]
    public class when_querying_remote_server
    {
        private TestServer server = new TestServer();

        [Test]
        public void should_contain_names_for_method_with_attribute()
        {
            var methodName = server.get_method_by_attribute("Void Keyword With No Params", "step") as string;

            methodName.ShouldBe("VoidKeywordWithNoParams");
        }

        [Test]
        public void should_find_default_before_story_method_if_implemented()
        {
            var methodName = server.get_method_by_attribute("BeforeStory", "beforestory") as string;

            methodName.ShouldBe("DefaultBeforeStory");
        }

        [Test]
        public void should_return_empty_if_default_after_story_is_not_implemented()
        {
            var methodName = server.get_method_by_attribute("AfterStory", "afterstory") as string;

            methodName.ShouldBe(string.Empty);
        }

        [Test]
        public void should_find_default_before_criterion_method_if_implemented()
        {
            var methodName = server.get_method_by_attribute("BeforeCriterion", "beforecriterion") as string;

            methodName.ShouldBe("DefaultBeforecriterion");
        }

        [Test]
        public void should_return_empty_if_default_after_criterion_is_not_implemented()
        {
            var methodName = server.get_method_by_attribute("AfterCriterion", "aftercriterion") as string;

            methodName.ShouldBe(string.Empty);
        }

        [Test]
        public void should_find_before_story_method()
        {
            var methodName = server.get_method_by_attribute("bf", "beforestory") as string;

            methodName.ShouldBe("BeforeStory");
        }

        [Test]
        public void should_find_after_story_method()
        {
            var methodName = server.get_method_by_attribute("af", "afterstory") as string;

            methodName.ShouldBe("AfterStory");
        }

        [Test]
        public void should_find_before_criterion_method()
        {
            var methodName = server.get_method_by_attribute("bs", "beforecriterion") as string;

            methodName.ShouldBe("Beforecriterion");
        }

        [Test]
        public void should_find_after_criterion_method()
        {
            var methodName = server.get_method_by_attribute("as", "aftercriterion") as string;

            methodName.ShouldBe("Aftercriterion");
        }

        [Test]
        public void should_return_keyword_names()
        {
            var names = server.get_keyword_names().ToList();

            names.ShouldContain("VoidKeywordWithNoParams", "VoidKeywordWithParams");
        }

        [Test]
        public void should_contain_keyword_parameter_names()
        {
            var parameters = server.get_parameter_names("Void Keyword With Params").ToList();

            parameters.ShouldContain("pString", "pBool", "pInt");
        }

        [Test]
        public void should_not_contain_any_parameter_names()
        {
            var parameters = server.get_parameter_names("Void Keyword With No Params").ToList();

            parameters.ShouldBeEmpty();
        }

        [Test]
        public void should_contain_keyword_arguments()
        {
            var args = server.get_keyword_arguments("Void Keyword With Params").ToList();

            args.ShouldContain("String", "Boolean", "Int32");
        }

        [Test]
        public void should_return_echo_string()
        {
            var result = server.Echo("foobar") as Result;

            result.retrn.ShouldBe("foobar");
        }

        [Test]
        public void should_find_keywords_in_bound_classes()
        {
            var result = server.get_keyword_names().ToList();

            result.Any(n => n.Equals("VoidKeywordWithNoParams")).ShouldBe(true);

            result.Any(n => n.Equals("BoolKeywordInSecondClass")).ShouldBe(true);
        }

        [Test]
        public void should_not_find_unbound_methods()
        {
            var result = server.get_keyword_names().ToList();

            result.Any(n => n.Equals("Dispose")).ShouldBe(false);
        }

        [Test]
        public void get_keyword_documentations_should_return_not_implemented()
        {
            var result = server.get_keyword_documentation("VoidKeywordWithNoParams");

            result.ShouldBe("NotImplemented.");
        }

        [Test]
        public void run_keyword_should_handle_exception_and_return_valid_result()
        {
            var result = server.run_keyword("ThrowsException", new object[] { }) as Result;

            result.status.ShouldBe("FAIL");

            string.IsNullOrEmpty(result.error).ShouldBe(false);

            string.IsNullOrEmpty(result.traceback).ShouldBe(false);
        }
    }

    [Fixture]
    public class TestServer : RemoteServer, IRemoteServer
    {
        public override void Dispose() { }

        [Step("Void Keyword With No Params")]
        public void VoidKeywordWithNoParams()
        {
            return;
        }

        [Step("Void Keyword With Params")]
        public void VoidKeywordWithParams(string pString, bool pBool, int pInt)
        {
            return;
        }

        [Step("Result Keyword")]
        public Result ResultKeyword()
        {
            var result = new Result();

            result.status = "was run";

            return result;
        }

        [Step("")]
        public Result ThrowsException()
        {
            throw new Exception();
        }

        [BeforeStory]
        public Result DefaultBeforeStory()
        {
            return Result.CreatePass("DefaultBeforeStory");
        }

        [BeforeCriterion]
        public Result DefaultBeforecriterion()
        {
            return Result.CreatePass("DefaultBeforecriterion");
        }

        [BeforeStory("bf")]
        public Result BeforeStory()
        {
            return Result.CreatePass();
        }

        [AfterStory("af")]
        public Result AfterStory()
        {
            return Result.CreatePass();
        }

        [BeforeCriterion("bs")]
        public Result Beforecriterion()
        {
            return Result.CreatePass();
        }

        [AfterCriterion("as")]
        public Result Aftercriterion()
        {
            return Result.CreatePass();
        }
    }

    [Fixture]
    public class TestServer2
    {
        [Step("")]
        public bool BoolKeywordInSecondClass(bool seed)
        {
            return !seed;
        }
    }
}
