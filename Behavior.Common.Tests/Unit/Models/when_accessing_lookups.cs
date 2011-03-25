using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Common.LookUps;
using Behavior.Common.Tests;

namespace Behavior.Common.Tests.Unit.Models
{
    [TestFixture]
    public class when_accessing_lookups
    {
        [Test]
        public void scenario_types_should_not_be_null()
        {
            StringLookUps.ScenarioTypes.ShouldNotBe(null);
        }

        [Test]
        public void scenario_types_should_contain_six_items()
        {
            StringLookUps.ScenarioTypes.Count.ShouldBe(5);
        }

        [Test]
        public void default_scenario_type_should_be_test()
        {
            StringLookUps.ScenarioTypesDefault.ShouldBe("Test");
        }
    }
}
