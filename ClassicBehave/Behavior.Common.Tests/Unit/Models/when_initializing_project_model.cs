using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Common.Models;

namespace Behavior.Common.Tests.Unit.Models
{
    [TestFixture]
    public class when_initializing_project_model
    {
        private Project project = new Project();

        [Test]
        public void project_name_should_be_empty()
        {
            project.Name.ShouldBe("");
        }

        [Test]
        public void project_description_should_be_empty()
        {
            project.Description.ShouldBe("");
        }

        [Test]
        public void project_story_ids_count_should_be_zero()
        {
            project.ChildrenIds.Count().ShouldBe(0);
        }

        [Test]
        public void project_stories_count_should_be_zero()
        {
            project.Stories.Count().ShouldBe(0);
        }

        [Test]
        public void project_id_should_be_valid_guid()
        {
            project.Id.ShouldNotBe(null);
        }

        [Test]
        public void project_tags_count_should_be_zero()
        {
            project.Tags.Count().ShouldBe(0);
        }

        [Test]
        public void project_type_should_be_project()
        {
            project.Type.ShouldBe("Project");
        }
    }
}
