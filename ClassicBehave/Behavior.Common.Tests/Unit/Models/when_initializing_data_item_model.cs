using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Common.Models;

namespace Behavior.Common.Tests.Unit.Models
{
    [TestFixture]
    public class when_initializing_data_item_model
    {
        private DataItem dataItem = new DataItem();

        [Test]
        public void data_item_name_should_be_empty()
        {
            dataItem.Name.ShouldBe("");
        }

        [Test]
        public void data_item_description_should_be_empty()
        {
            dataItem.Description.ShouldBe("");
        }

        [Test]
        public void data_item_data_should_be_empty()
        {
            dataItem.Data.ShouldBe("");
        }

        [Test]
        public void data_item_id_should_be_valid_guid()
        {
            dataItem.Id.ShouldNotBe(null);
        }

        [Test]
        public void data_item_tags_count_should_be_zero()
        {
            dataItem.Tags.Count().ShouldBe(0);
        }

        [Test]
        public void data_item_type_should_be_dataitem()
        {
            dataItem.Type.ShouldBe("DataItem");
        }

        [Test]
        public void data_item_is_last_should_be_true()
        {
            dataItem.IsLast.ShouldBe(true);
        }
    }
}
