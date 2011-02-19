using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Common.Models;

namespace Behavior.Common.Tests.Unit.Models
{
    [TestFixture]
    public class when_initializing_story_model
    {
        private Story story = new Story();

        [Test]
        public void story_name_should_be_empty()
        {
            story.Name.ShouldBe("");
        }

        [Test]
        public void story_description_should_be_empty()
        {
            story.Description.ShouldBe("");
        }

        [Test]
        public void story_scenario_ids_count_should_be_zero()
        {
            story.ChildrenIds.Count().ShouldBe(0);
        }

        [Test]
        public void story_scenarios_count_should_be_zero()
        {
            story.Scenarios.Count.ShouldBe(0);
        }

        [Test]
        public void story_id_should_be_valid_guid()
        {
            story.Id.ShouldNotBe(null);
        }

        [Test]
        public void story_tags_count_should_be_zero()
        {
            story.Tags.Count.ShouldBe(0);
        }

        [Test]
        public void story_type_should_be_story()
        {
            story.Type.ShouldBe("Story");
        }
    }
}
