using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Common.Models;
using Behavior.Common.Repository;

namespace Behavior.Common.Tests.Unit.Repository
{
    [TestFixture]
    public class when_filtering_scenarios_by_tags : Given_Scenarios
    {
        [Test]
        public void given_include_tag_should_returned_tagged_scenarios()
        {
            GivenTwoScenarios();

            var taggedScenarios = repo.GetItemsByTags(scenarios, new List<string>() {"foo"}, new List<string>());

            Assert.AreEqual(1, taggedScenarios.Count);
            Assert.AreEqual("SC1", taggedScenarios[0].Name);
        }

        [Test]
        public void given_include_and_exclude_should_return_one_scenario()
        {
            GivenTwoScenariosWithExclude();

            var taggedScenarios = repo.GetItemsByTags(scenarios, new List<string>() { "foo" }, new List<string>() { "bar" });

            Assert.AreEqual(1, taggedScenarios.Count);
            Assert.AreEqual("SC1", taggedScenarios[0].Name);
        }

        [Test]
        public void given_include_should_return_both_scenarios()
        {
            GivenTwoScenariosWithExclude();

            var taggedScenarios = repo.GetItemsByTags(scenarios, new List<string>() { "foo" }, new List<string>());

            Assert.AreEqual(2, taggedScenarios.Count);
            Assert.AreEqual("SC1", taggedScenarios[0].Name);
            Assert.AreEqual("SC2", taggedScenarios[1].Name);
        }

        [Test]
        public void given_nonexistant_include_tag_should_return_no_scenarios()
        {
            GivenTwoScenariosWithExclude();

            var taggedScenarios = repo.GetItemsByTags(scenarios, new List<string>() { "NotThere" }, new List<string>());

            Assert.AreEqual(0, taggedScenarios.Count);
        }

        [Test]
        public void given_no_tags_should_return_no_scenarios()
        {
            GivenTwoScenariosWithExclude();

            var taggedScenarios = repo.GetItemsByTags(scenarios, new List<string>(), new List<string>());

            Assert.AreEqual(0, taggedScenarios.Count);
        }

        [Test]
        public void given_include_tags_case_should_not_matter()
        {
            GivenTwoScenarios();

            var taggedScenarios = repo.GetItemsByTags(scenarios, new List<string>() { "FOO" }, new List<string>());

            Assert.AreEqual(1, taggedScenarios.Count);
            Assert.AreEqual("SC1", taggedScenarios[0].Name);
        }

        [Test]
        public void given_exclude_tags_case_should_not_matter()
        {
            GivenTwoScenariosWithExclude();

            var taggedScenarios = repo.GetItemsByTags(scenarios, new List<string>() { "FOO" }, new List<string>() { "BAR" });

            Assert.AreEqual(1, taggedScenarios.Count);
            Assert.AreEqual("SC1", taggedScenarios[0].Name);
        }
    }

    public class Given_Scenarios
    {
        protected List<Scenario> scenarios = new List<Scenario>();
        protected ItemRepository repo = new ItemRepository(new ItemSerializer(".\\DataPath"));

        public void GivenTwoScenarios()
        {
            scenarios = new List<Scenario>();

            var scenario1 = new Scenario()
            {
                Name = "SC1",
                Tags = { new Tag("foo") }
            };

            var scenario2 = new Scenario()
            {
                Name = "SC2",
                Tags = { new Tag("bar") }
            };

            scenarios.Add(scenario1);
            scenarios.Add(scenario2);
        }

        public void GivenTwoScenariosWithExclude()
        {
            scenarios = new List<Scenario>();

            var scenario1 = new Scenario()
            {
                Name = "SC1",
                Tags = { new Tag("foo") }
            };

            var scenario2 = new Scenario()
            {
                Name = "SC2",
                Tags = { new Tag("foo"), new Tag("bar") }
            };

            scenarios.Add(scenario1);
            scenarios.Add(scenario2);
        }
    }
}
