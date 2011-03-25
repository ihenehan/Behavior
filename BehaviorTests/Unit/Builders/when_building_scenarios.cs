using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Builders;
using Behavior.Common.Models;
using Behavior.Tests.Extensions;

namespace BehaviorTests.Unit.Builders
{
    [TestFixture]
    public class when_building_scenarios
    {
        private Scenario scenario;
        private List<Item> scenarios;
        private List<Item> built;
        private ScenarioBuilder builder = new ScenarioBuilder();

        [SetUp]
        public void Setup()
        {
            scenario = new Scenario()
            {
                Name = "foo",
                Description = "bar",
                TestDataRows = new List<TestData>(),
                Interactions = new List<Interaction>()
            };

            var interaction1 = new Interaction()
            {
                Name = "Step 1",
                DataItems = new List<DataItem>()
            };

            var interaction2 = new Interaction()
            {
                Name = "Step 2",
                DataItems = new List<DataItem>()
            };

            var dataItem = new DataItem()
            {
                Name = "param1"
            };

            var dataItem2 = new DataItem()
            {
                Name = "param2"
            };

            interaction1.DataItems.Add(dataItem);
            interaction1.DataItems.Add(dataItem2);

            interaction2.DataItems.Add(dataItem);
            interaction2.DataItems.Add(dataItem2);

            var testData1 = new TestData();
            testData1.Add("param1", "foo");
            testData1.Add("param2", "bar");

            var testData2 = new TestData();
            testData2.Add("param1", "foo");
            testData2.Add("param2", "bar");

            scenario.Interactions.Add(interaction1);
            scenario.Interactions.Add(interaction2);

            scenario.TestDataRows.Add(testData1);
            scenario.TestDataRows.Add(testData2);

            scenarios = new List<Item>();
            scenarios.Add(scenario);

            built = builder.Build(scenarios);
        }

        [Test]
        public void given_two_test_data_rows_should_create_two_scenarios()
        {
            built.Count.ShouldBe(2);
        }

        [Test]
        public void given_two_test_data_rows_should_set_correct_data_items()
        {
            var scenario = built.First() as Scenario;

            var interaction = scenario.Interactions.First();

            var dataItem1 = interaction.DataItems[0];
            var dataItem2 = interaction.DataItems[1];

            dataItem1.Data.ShouldBe("foo");
            dataItem2.Data.ShouldBe("bar");
        }

        [Test]
        public void given_null_test_data_rows_should_return_same_scenarios()
        {
            scenarios.ForEach(s => (s as Scenario).TestDataRows = null);

            built = builder.Build(scenarios);

            built.ShouldBe(scenarios);
        }

        [Test]
        public void given_zero_test_data_rows_should_return_same_scenarios()
        {
            scenarios.ForEach(s => (s as Scenario).TestDataRows = new List<TestData>());

            built = builder.Build(scenarios);

            built.ShouldBe(scenarios);
        }
    }
}
