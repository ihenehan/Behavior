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
    public class when_filtering_criteria_by_tags : Given_Criteria
    {
        [Test]
        public void given_include_tag_should_returned_tagged_criteria()
        {
            GivenTwoCriteria();

            var taggedCriteria = repo.GetItemsByTags(criteria, new List<string>() {"foo"}, new List<string>());

            Assert.AreEqual(1, taggedCriteria.Count);
            Assert.AreEqual("SC1", taggedCriteria[0].Name);
        }

        [Test]
        public void given_include_and_exclude_should_return_one_criterion()
        {
            GivenTwoCriteriaWithExclude();

            var taggedCriteria = repo.GetItemsByTags(criteria, new List<string>() { "foo" }, new List<string>() { "bar" });

            Assert.AreEqual(1, taggedCriteria.Count);
            Assert.AreEqual("SC1", taggedCriteria[0].Name);
        }

        [Test]
        public void given_include_should_return_both_criteria()
        {
            GivenTwoCriteriaWithExclude();

            var taggedCriteria = repo.GetItemsByTags(criteria, new List<string>() { "foo" }, new List<string>());

            Assert.AreEqual(2, taggedCriteria.Count);
            Assert.AreEqual("SC1", taggedCriteria[0].Name);
            Assert.AreEqual("SC2", taggedCriteria[1].Name);
        }

        [Test]
        public void given_nonexistant_include_tag_should_return_no_criteria()
        {
            GivenTwoCriteriaWithExclude();

            var taggedCriteria = repo.GetItemsByTags(criteria, new List<string>() { "NotThere" }, new List<string>());

            Assert.AreEqual(0, taggedCriteria.Count);
        }

        [Test]
        public void given_no_tags_should_return_no_criteria()
        {
            GivenTwoCriteriaWithExclude();

            var taggedCriterions = repo.GetItemsByTags(criteria, new List<string>(), new List<string>());

            Assert.AreEqual(0, taggedCriterions.Count);
        }

        [Test]
        public void given_include_tags_case_should_not_matter()
        {
            GivenTwoCriteria();

            var taggedCriterions = repo.GetItemsByTags(criteria, new List<string>() { "FOO" }, new List<string>());

            Assert.AreEqual(1, taggedCriterions.Count);
            Assert.AreEqual("SC1", taggedCriterions[0].Name);
        }

        [Test]
        public void given_exclude_tags_case_should_not_matter()
        {
            GivenTwoCriteriaWithExclude();

            var taggedCriterions = repo.GetItemsByTags(criteria, new List<string>() { "FOO" }, new List<string>() { "BAR" });

            Assert.AreEqual(1, taggedCriterions.Count);
            Assert.AreEqual("SC1", taggedCriterions[0].Name);
        }
    }

    public class Given_Criteria
    {
        protected List<Criterion> criteria = new List<Criterion>();
        protected ItemRepository repo = new ItemRepository(new ItemSerializer(".\\DataPath"));

        public void GivenTwoCriteria()
        {
            criteria = new List<Criterion>();

            var criterion1 = new Criterion()
            {
                Name = "SC1",
                Tags = { new Tag("foo") }
            };

            var criterion2 = new Criterion()
            {
                Name = "SC2",
                Tags = { new Tag("bar") }
            };

            criteria.Add(criterion1);
            criteria.Add(criterion2);
        }

        public void GivenTwoCriteriaWithExclude()
        {
            criteria = new List<Criterion>();

            var criterion1 = new Criterion()
            {
                Name = "SC1",
                Tags = { new Tag("foo") }
            };

            var criterion2 = new Criterion()
            {
                Name = "SC2",
                Tags = { new Tag("foo"), new Tag("bar") }
            };

            criteria.Add(criterion1);
            criteria.Add(criterion2);
        }
    }
}
