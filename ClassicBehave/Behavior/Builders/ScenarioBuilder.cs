using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;

namespace Behavior.Builders
{
    public class ScenarioBuilder : IScenarioBuilder
    {
        private List<Item> builtScenarios;

        public List<Item> Build(List<Item> items)
        {
            builtScenarios = new List<Item>();

            foreach (Scenario s in items)
                if (s.TestDataRows != null && s.TestDataRows.Count > 0)
                    BuildForScenario(s.Clone());
                else
                    return items;

            return builtScenarios;
        }

        public void BuildForScenario(Scenario scenario)
        {
            var rowIndex = 0;

            foreach (TestData t in scenario.TestDataRows)
            {
                scenario.Name = scenario.Name + " [" + rowIndex + "]";

                builtScenarios.Add(BuildForRow(scenario, t));

                rowIndex++;
            }
        }

        public Item BuildForRow(Scenario scenario, TestData testData)
        {
            foreach (Interaction i in scenario.Interactions)
                foreach (DataItem d in i.DataItems)
                    d.Data = testData[d.Name];

            return scenario;
        }
    }
}
