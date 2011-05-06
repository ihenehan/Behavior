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
    public class when_retrieving_story_files
    {
        [Test]
        public void given_default_datapath_should_return_one_story()
        {
            //var repo = new ItemRepository(new ItemSerializer("Unit\\Parser"));

            //repo.GetAllStories(false).Count.ShouldBe(1);
        }

        //[Test]
        //public void given_default_datapath_should_save_table_files()
        //{
        //    var repo = new ItemRepository(new ItemSerializer("Unit\\Parser"));

        //    repo.GetAllStories(true).Count.ShouldBe(1);
        //}
    }
}
