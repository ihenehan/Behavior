using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Common.Models;
using Behavior.Common.Parser;
using Behavior.Common.Tests;

namespace Behavior.Common.Tests.Unit.Parser
{
    [TestFixture]
    public class when_parsing_story_files : TestStoryParserBase
    {
        [SetUp]
        public void Setup()
        {
            GivenStory("Unit\\Parser\\NotGherkin.story");
        }

        [Test]
        public void given_default_story_should_not_be_null()
        {
            story.ShouldNotBe(null);
        }

        [Test]
        public void given_default_story_should_have_one_attribute()
        {
            story.Tags.Count.ShouldBe(1);

            story.Tags[0].Name.ShouldBe("storyAttribute");
        }

        [Test]
        public void given_default_story_description_should_be_correct()
        {
            story.DescriptionLines.Count.ShouldBe(3);
            story.DescriptionLines[0].ShouldBe("As a user, ");
            story.DescriptionLines[1].ShouldBe("I want to write a test, ");
            story.DescriptionLines[2].ShouldBe("So that behavior can be validated ");
        }

        [Test]
        public void given_default_story_should_contain_one_before_story_type()
        {
            story.BeforeStories.Count.ShouldBe(1);
        }

        [Test]
        public void given_default_story_should_contain_one_after_story_type()
        {
            story.AfterStories.Count.ShouldBe(1);
        }

        [Test]
        public void given_default_story_should_have_two_scenarios()
        {
            story.Scenarios.Count.ShouldBe(2);
        }

        [Test]
        public void given_default_story_should_contain_one_scenario_outline_type()
        {
            story.Scenarios.Any(s => s.ScenarioType.Equals("Scenario Outline")).ShouldBe(true);
        }

        [Test]
        public void given_default_story_should_contain_one_scenario_types()
        {
            story.Scenarios.Where(s => s.ScenarioType.Equals("Scenario")).Count().ShouldBe(1);
        }

        [Test]
        public void given_default_story_should_contain_one_common_scenario_type()
        {
            story.ScenarioCommon.Where(s => s.ScenarioType.Equals("Scenario Common")).Count().ShouldBe(1);
        }

        [Test]
        public void given_default_story_test_sequence_should_contain_nine_scenarios()
        {
            story.TestSequence.Count.ShouldBe(18);
        }

        [Test]
        public void given_default_story_data_should_include_key_role_with_valid_value()
        {
            story.Scenarios.First(s => s.Table != null).Table.GetCellValue("role", 1).ShouldBe("admin");
        }

        [Test]
        public void given_default_story_data_should_return_empty_string_for_negative_row_index()
        {
            story.Scenarios.First(s => s.Table != null).Table.GetCellValue("role", -1).ShouldBe("");
        }

        [Test]
        public void given_default_story_scenario_should_have_one_before_scenario_type()
        {
            story.Scenarios.First(s => s.ScenarioType.Equals("Scenario")).BeforeScenarios.Count.ShouldBe(1);
        }

        [Test]
        public void given_default_story_scenario_should_have_one_after_scenario_type()
        {
            story.Scenarios.First(s => s.ScenarioType.Equals("Scenario")).AfterScenarios.Count.ShouldBe(1);
        }
    }

    [TestFixture]
    public class given_test_data_for_scenario : TestStoryParserBase
    {
        private List<string> lines = new List<string>();
        private Scenario outline;

        [SetUp]
        public void Setup()
        {
            lines = new List<string>();

            lines.Add("| role | type |");
            lines.Add("| admin | test |");
            lines.Add("|user | dev |");

            var count = 0;

            outline = new Scenario() { Name = "Foo", ScenarioType = "Scenario Outline" };

            outline.Table = new Table().Parse(lines, ref count);
        }

        [Test]
        public void should_have_two_header_cells()
        {
            outline.Table.HeaderRow.Count.ShouldBe(2);
        }

        [Test]
        public void should_have_two_data_rows()
        {
            outline.Table.DataRows.Count.ShouldBe(2);
        }

        [Test]
        public void header_cells_should_contain_correct_keys()
        {
            outline.Table.HeaderRow.ShouldContain("role", "type");
        }

        [Test]
        public void data_rows_should_contain_correct_values()
        {
            outline.Table.DataRows[0].DataCells.ShouldContain("admin", "test");
            
            outline.Table.DataRows[1].DataCells.ShouldContain("user", "dev");
        }
    }

    [TestFixture]
    public class when_parsing_steps : TestStoryParserBase
    {
        [Test]
        public void given_quoted_parameter_should_replace_with_arg_token()
        {
            GivenStep("foo \"param\" bar");
            
            step.ProcessedCommand.ShouldBe("foo [arg] bar");
        }

        [Test]
        public void given_quoted_parameter_is_first_should_replace_with_arg_token()
        {
            GivenStep("\"param\" foo bar");

            step.ProcessedCommand.ShouldBe("[arg] foo bar");
        }

        [Test]
        public void given_two_quoted_parameters_should_replace_with_arg_tokens()
        {
            GivenStep("foo \"param\" bar \"param2\" foo");
            
            step.ProcessedCommand.ShouldBe("foo [arg] bar [arg] foo");
        }

        [Test]
        public void given_quoted_parameter_at_end_should_replace_with_arg_token()
        {
            GivenStep("foo bar \"param1\"");

            step.ProcessedCommand.ShouldBe("foo bar [arg]");
        }

        [Test]
        public void given_two_consecutive_parameters_should_replace_with_arg_tokens()
        {
            GivenStep("foo \"param\"\"param2\" foo");
            
            step.ProcessedCommand.ShouldBe("foo [arg][arg] foo");
        }

        [Test]
        public void given_bracketed_parameter_should_replace_with_arg_token()
        {
            GivenStep("foo <param> bar");
            
            step.ProcessedCommand.ShouldBe("foo [arg] bar");
        }

        [Test]
        public void given_bracketed_parameter_at_end_should_replace_with_arg_token()
        {
            GivenStep("foo bar <param1>");

            step.ProcessedCommand.ShouldBe("foo bar [arg]");
        }

        [Test]
        public void given_bracketed_parameter_is_first_should_replace_with_arg_token()
        {
            GivenStep("<param> foo bar");
            
            step.ProcessedCommand.ShouldBe("[arg] foo bar");
        }

        [Test]
        public void given_two_bracketed_parameters_should_replace_with_arg_tokens()
        {
            GivenStep("foo <param> bar <param2> foo");
            
            step.ProcessedCommand.ShouldBe("foo [arg] bar [arg] foo");
        }

        [Test]
        public void given_two_consecutive_bracketed_parameters_should_replace_with_arg_tokens()
        {
            GivenStep("foo <param><param2> foo");
            
            step.ProcessedCommand.ShouldBe("foo [arg][arg] foo");
        }

        [Test]
        public void given_quoted_and_bracketed_parameters_should_replace_with_arg_tokens()
        {
            GivenStep("foo <param> bar \"param2\" foo");
            
            step.ProcessedCommand.ShouldBe("foo [arg] bar [arg] foo");
        }

        [Test]
        public void given_two_identical_parameters_should_replace_with_arg_tokens()
        {
            GivenStep("foo <param> bar <param> foo");

            step.ProcessedCommand.ShouldBe("foo [arg] bar [arg] foo");
        }

        [Test]
        public void given_two_identical_bracketed_parameters_parameter_list_should_contain_both_parameters()
        {
            GivenStep("foo <param> bar <param> foo");

            step.Parameters.Count.ShouldBe(2);
            step.Parameters.All(a => a.Equals("<param>")).ShouldBe(true);
        }

        [Test]
        public void given_two_identical_quoted_parameters_parameter_list_should_contain_both_parameters()
        {
            GivenStep("foo \"param\" bar \"param\" foo");

            step.Parameters.Count.ShouldBe(2);
            step.Parameters.All(a => a.Equals("param")).ShouldBe(true);
        }

        [Test]
        public void given_no_parameters_processed_command_should_equal_raw_command()
        {
            GivenStep("foo bar");

            step.ProcessedCommand.ShouldBe(step.RawCommand);
        }

        [Test]
        public void given_quoted_and_bracketed_parameters_arg_list_should_contain_all_values()
        {
            GivenStep("foo <param> bar \"param2\" foo");

            step.Parameters.ShouldContain("<param>", "param2");
        }

        [Test]
        public void given_no_parameters_arg_list_should_be_empty()
        {
            GivenStep("foo bar");

            step.Parameters.Count.ShouldBe(0);
        }
    }

    public class TestStoryParserBase
    {
        protected Story story;
        protected ProcessedStep step;

        public void GivenStory(string file)
        {
            var parser = new StoryParser(file);

            story = parser.Story;
        }

        public void GivenStep(string command)
        {
            step = new ProcessedStep(command);
        }
    }
}
