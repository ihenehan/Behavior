using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NUnit.Framework;
using Behavior.Common.Repository;
using Behavior.Common.Models;

namespace Behavior.Common.Tests.Unit.Repository
{
    [TestFixture]
    public class when_deleting_items : in_item_repository
    {
        [SetUp]
        public void Setup()
        {
            Given_Repository(false);
            projectItem.Stories.Add(storyItem);
            storyItem.Scenarios.Add(scenarioItem);
        }

        [Test]
        public void given_existing_scenario_should_remove_file()
        {
            var scenario = storyItem.Scenarios.First();

            var path = dataPath + "\\scenario\\" + scenario.Id + ".jsn";

            repo.Delete(scenario);

            File.Exists(dataPath).ShouldBe(false);
        }

        [Test]
        public void given_existing_story_should_remove_file()
        {
            var story = projectItem.Stories.First();

            var path = dataPath + "\\story\\" + story.Id + ".jsn";

            repo.Delete(story);

            File.Exists(dataPath).ShouldBe(false);
        }

        [Test]
        public void given_existing_project_should_remove_file()
        {
            var path = dataPath + "\\project\\" + projectItem.Id + ".jsn";

            repo.Delete(projectItem);

            File.Exists(dataPath).ShouldBe(false);
        }
    }
}
