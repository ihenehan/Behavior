using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Common.Models;

namespace Behavior.Common.Tests.Unit.Models
{
    [TestFixture]
    public class when_initializing_interaction_model
    {
        private Interaction interaction = new Interaction();

        [Test]
        public void interaction_name_should_be_empty()
        {
            interaction.Name.ShouldBe("");
        }

        [Test]
        public void interaction_description_should_be_empty()
        {
            interaction.Description.ShouldBe("");
        }

        [Test]
        public void interaction_id_should_be_valid_guid()
        {
            interaction.Id.ShouldNotBe(null);
        }

        [Test]
        public void interaction_tags_count_should_be_zero()
        {
            interaction.Tags.Count().ShouldBe(0);
        }

        [Test]
        public void interaction_type_should_be_interaction()
        {
            interaction.Type.ShouldBe("Interaction");
        }

        [Test]
        public void interaction_data_item_ids_list_count_should_be_zero()
        {
            interaction.ChildrenIds.Count().ShouldBe(0);
        }

        [Test]
        public void interaction_data_items_count_should_be_zero()
        {
            interaction.DataItems.Count().ShouldBe(0);
        }

        [Test]
        public void interaction_is_last_should_be_true()
        {
            interaction.IsLast.ShouldBe(true);
        }
    }
}
