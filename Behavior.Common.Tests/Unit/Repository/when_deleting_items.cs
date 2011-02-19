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
        }

        //[Test]
        //public void given_existing_project_should_remove_file_and_return_null()
        //{
        //    var path = dataPath + "\\project\\" + projectItem.Id + ".jsn";

        //    var item = repo.Delete(projectItem, null);

        //    Assert.IsNull(item);

        //    Assert.False(File.Exists(dataPath));
        //}

        //[Test]
        //public void given_existing_story_should_remove_file_and_return_project()
        //{
        //    var story = projectItem.Children.First();

        //    var path = dataPath + "\\story\\" + story.Id + ".jsn";

        //    var item = repo.Delete(story, projectItem);

        //    Assert.AreEqual(typeof(Project), item.GetType());

        //    Assert.False(File.Exists(dataPath));
        //}

        //[Test]
        //public void given_existing_scenario_should_remove_file_and_return_story()
        //{
        //    var scenario = storyItem.Children.First();

        //    var path = dataPath + "\\scenario\\" + scenario.Id + ".jsn";

        //    var item = repo.Delete(scenario, storyItem);

        //    Assert.AreEqual(typeof(Story), item.GetType());

        //    Assert.False(File.Exists(dataPath));
        //}

        //[Test]
        //public void given_existing_interaction_should_remove_file_and_return_scenario()
        //{
        //    var interaction = scenarioItem.Children.First();

        //    var path = dataPath + "\\interaction\\" + interaction.Id + ".jsn";

        //    var item = repo.Delete(interaction, scenarioItem);

        //    Assert.AreEqual(typeof(Scenario), item.GetType());

        //    Assert.False(File.Exists(dataPath));
        //}

        //[Test]
        //public void given_existing_dataitem_should_remove_file_and_return_interaction()
        //{
        //    var dataItem = interactionItem.Children.First();

        //    var path = dataPath + "\\dataitem\\" + dataItem.Id + ".jsn";

        //    var item = repo.Delete(dataItem, interactionItem);

        //    Assert.AreEqual(typeof(Interaction), item.GetType());

        //    Assert.False(File.Exists(dataPath));
        //}
    }
}
