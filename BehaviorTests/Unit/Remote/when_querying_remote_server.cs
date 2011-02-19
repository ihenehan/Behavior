using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Remote;
using Behavior.Remote.Server;
using Behavior.Remote.Results;
using BehaviorTests.Extensions;

namespace BehaviorTests.Unit.Remote
{
    [TestFixture]
    public class when_querying_remote_server
    {
        private TestServer server = new TestServer();

        [Test]
        public void should_return_keyword_names()
        {
            var names = server.get_keyword_names().ToList();

            names.ShouldContain("VoidKeywordWithNoParams", "VoidKeywordWithParams");
        }

        [Test]
        public void should_contain_keyword_parameter_names()
        {
            var parameters = server.get_parameter_names("VoidKeywordWithParams").ToList();

            parameters.ShouldContain("pString", "pBool", "pInt");
        }

        [Test]
        public void should_not_contain_any_parameter_names()
        {
            var parameters = server.get_parameter_names("VoidKeywordWithNoParams").ToList();

            parameters.ShouldBeEmpty();
        }

        [Test]
        public void should_contain_keyword_arguments()
        {
            var args = server.get_keyword_arguments("VoidKeywordWithParams").ToList();

            args.ShouldContain("String", "Boolean", "Int32");
        }

        [Test]
        public void should_execute_keyword_and_return_int()
        {
            server.run_keyword("IntKeywordToRun", new object[] { 4 }).ShouldBe(16);
        }

        [Test]
        public void should_execute_keyword_and_return_string()
        {
            server.run_keyword("StringKeywordToRun", new object[] { "foo" }).ShouldBe("foofoo");
        }

        [Test]
        public void should_execute_keyword_and_return_bool()
        {
            server.run_keyword("BoolKeywordToRun", new object[] { true }).ShouldBe(false);
        }

        [Test]
        public void should_return_result_object()
        {
            var result = server.run_keyword("ResultKeyword", new object[] {}) as Result;

            result.status.ShouldBe("was run");
        }

        [Test]
        public void should_return_echo_string()
        {
            var result = server.run_keyword("Echo", new object[] { "foobar" }) as Result;

            result.retrn.ShouldBe("foobar");
        }
    }

    public class TestServer : RemoteServer, IRemoteServer
    {
        public override void Dispose() { }

        public void VoidKeywordWithNoParams()
        {
            return;
        }

        public void VoidKeywordWithParams(string pString, bool pBool, int pInt)
        {
            return;
        }

        public int IntKeywordToRun(int seed)
        {
            return seed * seed;
        }

        public string StringKeywordToRun(string seed)
        {
            return seed + seed;
        }

        public bool BoolKeywordToRun(bool seed)
        {
            return !seed;
        }

        public Result ResultKeyword()
        {
            var result = new Result();

            result.status = "was run";

            return result;
        }

    }
}
