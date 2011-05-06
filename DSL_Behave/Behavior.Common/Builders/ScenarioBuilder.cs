using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;
using Behavior.Common.Repository;
using Behavior.Common.Extensions;

namespace Behavior.Common.Builders
{
    public class ScenarioBuilder : IScenarioBuilder
    {
        private List<Scenario> builtScenarios;

        public List<Scenario> BuildScenariosFromOutline(Scenario scenario, List<Scenario> commonScenarios)
        {
            builtScenarios = new List<Scenario>();

            if (scenario.Table != null)
            {
                var count = 0;

                foreach (DataRow dr in scenario.Table.DataRows)
                {
                    builtScenarios.AddRange(scenario.BeforeScenarios.Clone());

                    builtScenarios.AddRange(commonScenarios.Clone());

                    builtScenarios.Add(InsertParametricData(scenario, count));

                    builtScenarios.AddRange(scenario.AfterScenarios.Clone());

                    count++;
                }
            }
            else
                builtScenarios.Add(scenario);

            return builtScenarios;
        }

        public List<Scenario> BuildScenario(Scenario scenario, List<Scenario> scenarioCommon)
        {
            var sequence = new List<Scenario>();

            sequence.AddRange(scenario.BeforeScenarios);

            sequence.AddRange(scenarioCommon.Clone());

            sequence.Add(scenario);

            sequence.AddRange(scenario.AfterScenarios);

            return sequence;
        }

        public Scenario InsertParametricData(Scenario scenario, int count)
        {
            var newScenario = scenario.Clone();

            newScenario.Name = newScenario.Name + "_" + count;

            foreach (ScenarioStep i in newScenario.Steps)
                for (int x = 0; x < i.Parameters.Count; x++)
                    if (i.Parameters[x].GetType().Equals(typeof(string)))
                    {
                        if ((i.Parameters[x] as string).StartsWith("<"))
                            i.Parameters[x] = InsertValue(scenario.Table, i.Parameters[x] as string, count);
                    }
                    else
                        i.Parameters[x] = i.Table;

            return newScenario;
        }

        public string InsertValue(Table table, string parameter, int row)
        {
            var key = parameter.Replace("<", "").Replace(">", "");

            return table.GetCellValue(key, row);
        }
    }
}
