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
        private IRepository repo;
        private List<Scenario> builtScenarios;

        public ScenarioBuilder(IRepository repository)
        {
            repo = repository;
        }

        public List<Scenario> Build(Scenario scenario, List<Scenario> commonScenarios)
        {
            builtScenarios = new List<Scenario>();

            if (scenario.Table != null)
            {
                var count = 0;

                foreach (DataRow dr in scenario.Table.DataRows)
                {
                    if(scenario.BeforeScenarios != null)
                        builtScenarios.AddRange(scenario.BeforeScenarios.Clone());

                    if(commonScenarios != null)
                        builtScenarios.AddRange(commonScenarios.Clone());

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

                    builtScenarios.Add(newScenario);

                    if(scenario.AfterScenarios != null)
                        builtScenarios.AddRange(scenario.AfterScenarios.Clone());

                    count++;
                }
            }
            else
                builtScenarios.Add(scenario);

            return builtScenarios;
        }

        public string InsertValue(Table table, string parameter, int row)
        {
            var key = parameter.Replace("<", "").Replace(">", "");

            return table.GetValue(key, row);
        }
    }
}
