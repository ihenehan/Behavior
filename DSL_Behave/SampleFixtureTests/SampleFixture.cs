using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Remote.Results;
using Behavior.Remote.Attributes;
using Behavior.Common.Models;
using NUnit.Framework;
using CookComputing.XmlRpc;

namespace SampleFixtureTests
{
    [Fixture]
    public class SampleFixture : BaseFixture
    {
        public SampleFixture()
        {
            ScenarioContext = new Dictionary<string, object>();
        }

        private string Story
        {
            get { return StoryContext["story"] as string; }
            set { StoryContext["story"] = value; }
        }

        private string Scenario
        {
            get { return ScenarioContext["scenario"] as string; }
            set { ScenarioContext["scenario"] = value; }
        }

        [Step("I have a common step")]
        public void IHaveACommonStep(Table table)
        {
            Assert.NotNull(table);
            Assert.NotNull(table.HeaderRow);
            Assert.NotNull(table.DataRows);
        }

        [Step("I have a step table")]
        public void IHaveAStepTable(Table table)
        {
            Assert.NotNull(table);
            Assert.NotNull(table.HeaderRow);
            Assert.NotNull(table.DataRows);
            Assert.AreEqual(2, table.DataRows.Count);
        }

        [Step("I am a [arg] user")]
        public void IAmAnArgUser(string role)
        {
            Assert.IsFalse(string.IsNullOrEmpty(role));
        }

        [Step("I write a [arg] test")]
        public void IWriteAArgTest(string testType)
        {
            Assert.IsFalse(string.IsNullOrEmpty(testType));
        }

        [Step("it should execute correctly")]
        public void ItShouldExecuteCorrectly()
        {
            Assert.AreEqual("true", Story);
            Assert.AreEqual("true", Scenario);
        }

        [Step("the system is available")]
        public void TheSystemIsAvailable()
        {

        }

        [Step("Add item to story context")]
        public void AddItemToStoryContext()
        {
            Story = "true";
        }

        [Step("Clean up the environment")]
        public void EnvironmentIsClean()
        {

        }

        [Step("Add item to scenario context")]
        public void AddItemToScenarioContext()
        {
            Scenario = "true";
        }

        [Step("Key [arg] is in ScenarioContext")]
        public void KeyIsInScenarioContext(string key)
        {
            Assert.IsTrue(ScenarioContext.ContainsKey(key));
        }

        [Step("Key [arg] is not in ScenarioContext")]
        public void KeyIsNotInScenarioContext(string key)
        {
            Assert.IsFalse(ScenarioContext.ContainsKey(key));
        }

        [Step("Log out.")]
        public void IAmLoggedOut()
        {

        }

        [Step("I have [arg] multiple arguments of [arg]")]
        public void IHaveMultipleArgs(string arg1, string arg2)
        {
            Assert.IsFalse(string.IsNullOrEmpty(arg1));
            Assert.IsFalse(string.IsNullOrEmpty(arg2));
        }

        [Step("I have argument at end [arg]")]
        public void IHaveArgAtEnd(string arg)
        {
            Assert.IsFalse(string.IsNullOrEmpty(arg));
        }
    }
}
