using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.ArgParser;
using Behavior.Common.Configuration;
using Behavior.Tests.Extensions;

namespace Behavior.Tests.Unit.ArgParser
{
    public class when_parsing_behavior_args
    {
        [TestFixture]
        public class given_an_empty_builder_string : When_Parsing_BehaviorConfiguration_Args
        {
            [SetUp]
            public void setup()
            {
                Given("");
            }

            [Test]
            public void DataPath_should_be_empty()
            {
                config.DataPath.ShouldBeEmpty();
            }

            [Test]
            public void ExcludeTags_should_be_empty()
            {
                config.ExcludeTags.ShouldBeEmpty();
            }

            [Test]
            public void IncludeTags_should_be_empty()
            {
                config.IncludeTags.ShouldBeEmpty();
            }

            [Test]
            public void FixtureType_should_be_empty()
            {
                config.FixtureContext.ShouldBeEmpty();
            }

            [Test]
            public void GuiDelay_should_be_empty()
            {
                config.GuiDelay.ShouldBeEmpty();
            }

            [Test]
            public void Host_should_be_empty()
            {
                config.Host.ShouldBeEmpty();
            }

            [Test]
            public void ResultFile_should_be_empty()
            {
                config.ResultFile.ShouldBeEmpty();
            }

            [Test]
            public void TestVariables_should_be_empty()
            {
                config.TestVariables.ShouldBeEmpty();
            }
        }

        [TestFixture]
        public class hostname_should_be_foo : When_Parsing_BehaviorConfiguration_Args
        {
            [Test]
            public void given_hostname_foo()
            {
                Given("host=foo");
            }

            [Test]
            public void order_doesnt_matter()
            {
                Given("host=foo", "fixture=prod");
            }

            [Test]
            public void key_case_doesnt_matter()
            {
                Given("HOST=foo");
            }

            [TearDown]
            public void should_assign()
            {
                config.Host.Equals("foo");
            }
        }

        [TestFixture]
        public class should_assign_to_builder : When_Parsing_BehaviorConfiguration_Args
        {
            [Test]
            public void given_result_file()
            {
                Given("resultfile=file");

                config.ResultFile.ShouldBe("file");
            }

            [Test]
            public void given_datapath()
            {
                Given("datapath=c:\\mypath");

                config.DataPath.ShouldBe("c:\\mypath");
            }

            [Test]
            public void given_exclude_tags()
            {
                Given("exclude=foo,bar");

                config.ExcludeTags.ShouldContain("foo", "bar");
            }

            [Test]
            public void given_include_tags()
            {
                Given("include=foo,bar");

                config.IncludeTags.ShouldContain("foo", "bar");
            }

            [Test]
            public void given_fixture_context()
            {
                Given("context=foo");

                config.FixtureContext.ShouldBe("foo");
            }

            [Test]
            public void given_fixture_type()
            {
                Given("fixture=foo");

                config.FixtureType.ShouldBe("foo");
            }

            [Test]
            public void given_islocal()
            {
                Given("islocal=true");

                config.IsLocal.ShouldBe(true);
            }

            [Test]
            public void given_gui_delay()
            {
                Given("delay=10");

                config.GuiDelay.ShouldBe("10");
            }

            [Test]
            public void given_host_url()
            {
                Given("host=123.234.345.456:1234");

                config.Host.ShouldBe("123.234.345.456:1234");
            }

            [Test]
            public void given_test_variables()
            {
                Given("${foo}=bar", "${test}=run");

                config.TestVariables.ShouldContain(new KeyValuePair<string, string>("${foo}", "bar"), new KeyValuePair<string, string>("${test}", "run"));
            }
        }

        public class When_Parsing_BehaviorConfiguration_Args
        {
            protected BehaviorConfiguration config;
            protected BehaviorArgParser parser;

            protected void Given(params string[] args)
            {
                config = new BehaviorConfiguration();

                parser = new BehaviorArgParser();

                parser.Parse(config, args);
            }
        }
    }
}
