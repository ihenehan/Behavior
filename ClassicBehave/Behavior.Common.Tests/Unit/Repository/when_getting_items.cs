using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Common.Repository;
using Behavior.Common.Models;

namespace Behavior.Common.Tests.Unit.Repository
{
    [TestFixture]
    public class when_getting_items : in_item_repository
    {
        [SetUp]
        public void Setup()
        {
            Given_Repository(true);
        }

        [Test]
        public void given_project_id_should_get_project()
        {
            var item = repo.Get<Project>(projectItem.Id);

            Assert.AreEqual(projectItem.Id, item.Id);
        }

        [Test]
        public void given_story_id_should_get_story()
        {
            var item = repo.Get<Story>(storyItem.Id);

            Assert.AreEqual(storyItem.Id, item.Id);
        }
        
        [Test]
        public void given_scenario_id_should_get_scenario()
        {
            var item = repo.Get<Scenario>(scenarioItem.Id);

            Assert.AreEqual(scenarioItem.Id, item.Id);
        }

        [Test]
        public void given_projects_should_return_one_project()
        {
            var items = repo.GetAll<Project>();

            Assert.AreEqual(1, items.Count);

            Assert.AreEqual(typeof(Project), items.First().GetType());
        }

        [Test]
        public void given_stories_should_return_one_story()
        {
            var items = repo.GetAll<Story>();

            Assert.AreEqual(1, items.Count);

            Assert.AreEqual(typeof(Story), items.First().GetType());
        }

        [Test]
        public void given_scenarios_should_return_one_scenario()
        {
            var items = repo.GetAll<Scenario>();

            Assert.AreEqual(1, items.Count);

            Assert.AreEqual(typeof(Scenario), items.First().GetType());
        }
    }
}
