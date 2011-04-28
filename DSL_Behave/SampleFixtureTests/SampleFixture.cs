using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Remote.Results;
using Behavior.Remote.Attributes;
using Behavior.Common.Models;
using NUnit.Framework;

namespace SampleFixtureTests
{
    [Fixture]
    public class SampleFixture : BaseFixture
    {
        [Step("I have a common step")]
        public void IHaveACommonStep(Table table)
        {
            Assert.NotNull(table);
            Assert.NotNull(table.HeaderCells);
            Assert.NotNull(table.DataRows);
        }

        [Step("I have a step table")]
        public void IHaveAStepTable(Table table)
        {
            Assert.NotNull(table);
            Assert.NotNull(table.HeaderCells);
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
            Assert.AreEqual("true", StoryContext["story"] as string);
            Assert.AreEqual("true", ScenarioContext["scenario"] as string);
        }

        [Step("the system is available")]
        public void TheSystemIsAvailable()
        {

        }

        [Step("the database is in a known state.")]
        public void DataInKnownState()
        {
            if(!StoryContext.ContainsKey("story"))
                StoryContext.Add("story", "true");
        }

        [Step("the environment is cleaned up.")]
        public void EnvironmentIsClean()
        {

        }

        [Step("I am logged in.")]
        public void IAmLoggedIn()
        {
            if(!ScenarioContext.ContainsKey("scenario"))
                ScenarioContext.Add("scenario", "true");
        }

        [Step("I am logged out.")]
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
