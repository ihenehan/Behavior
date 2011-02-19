using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Common.Models;
using Behavior.Common.LookUps;

namespace Behavior.Common.Tests.Unit.Models
{
    [TestFixture]
    public class when_initializing_scenario_model
    {
        private Scenario scenario = new Scenario();

        [Test]
        public void scenario_name_should_be_empty()
        {
            scenario.Name.ShouldBe("");
        }

        [Test]
        public void scenario_description_should_be_empty()
        {
            scenario.Description.ShouldBe("");
        }

        [Test]
        public void scenario_selected_should_be_true()
        {
            scenario.Selected.ShouldBe(true);
        }

        [Test]
        public void scenario_scenariotype_should_be_default()
        {
            scenario.ScenarioType.ShouldBe(StringLookUps.ScenarioTypesDefault);
        }

        [Test]
        public void scenario_interaction_ids_count_should_be_zero()
        {
            scenario.ChildrenIds.Count().ShouldBe(0);
        }

        [Test]
        public void scenario_interactions_count_should_be_zero()
        {
            scenario.Interactions.Count().ShouldBe(0);
        }

        [Test]
        public void scenario_id_should_be_valid_guid()
        {
            scenario.Id.ShouldNotBe(null);
        }

        [Test]
        public void scenario_tags_count_should_be_zero()
        {
            scenario.Tags.Count().ShouldBe(0);
        }

        [Test]
        public void scenario_type_should_be_scenario()
        {
            scenario.Type.ShouldBe("Scenario");
        }
    }
}
