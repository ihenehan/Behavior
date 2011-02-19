using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using NUnit.Framework;
using Behavior.Common.Repository;
using Behavior.Common.Models;

namespace Behavior.Common.Tests.Unit.Repository
{
    [TestFixture]
    public class when_creating_new_items : in_item_repository
    {
        [SetUp]
        public void Setup()
        {
            Given_Repository(true);
        }

        [Test]
        public void given_project_should_return_typeof_project()
        {
            Assert.AreEqual(typeof(Project), projectItem.GetType());
        }

        [Test]
        public void given_story_should_return_typeof_story()
        {
            Assert.AreEqual(typeof(Story), storyItem.GetType());
        }

        [Test]
        public void given_scenario_should_return_typeof_scenario()
        {
            Assert.AreEqual(typeof(Scenario), scenarioItem.GetType());
        }

        [Test]
        public void given_project_should_contain_no_stories()
        {
            Assert.AreEqual(0, projectItem.Stories.Count);
        }

        [Test]
        public void given_story_should_contain_no_scenarios()
        {
            Assert.AreEqual(0, storyItem.Scenarios.Count);
        }

        [Test]
        public void given_scenario_should_contain_no_interactions()
        {
            Assert.AreEqual(0, scenarioItem.Interactions.Count);
        }

        [Test]
        public void given_project_childrentype_should_be_story()
        {
            Assert.AreEqual(typeof(Story), projectItem.ChildrenType);
        }

        [Test]
        public void given_story_childrentype_should_be_scenario()
        {
            Assert.AreEqual(typeof(Scenario), storyItem.ChildrenType);
        }

        [Test]
        public void given_scenario_childrentype_should_be_null()
        {
            Assert.IsNull(scenarioItem.ChildrenType);
        }
    }

    public class in_item_repository
    {
        protected ISerializer serializer;
        protected IRepository repo;
        protected string dataPath = ".\\TestData";
        protected Project projectItem;
        protected Story storyItem;
        protected Scenario scenarioItem;
        protected Interaction interactionItem;
        protected DataItem dataItem;

        public void Given_Repository(bool cleanFolders)
        {
            if(cleanFolders)
                DeleteFiles();

            serializer = new ItemJsonSerializer(dataPath);

            repo = new ItemRepository(serializer);

            repo.DataPath = dataPath;

            projectItem = repo.Create<Project>() as Project;
            storyItem = repo.Create<Story>() as Story;
            scenarioItem = repo.Create<Scenario>() as Scenario;
            interactionItem = new Interaction();
            dataItem = new DataItem();

        }

        public void DeleteFiles()
        {
            if (Directory.Exists(dataPath))
            {
                foreach(string d in Directory.GetDirectories(dataPath))
                    Directory.Delete(d, true);
            }
        }
    }
}
