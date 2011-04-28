using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Behavior.Common.Models;
using Behavior.Remote.Results;
using Behavior.Remote.Client;

namespace Behavior.ModelExtensions
{
    public static class ScenarioExtensions
    {
        public static ScenarioResult Run(this Scenario scenario, IRemoteClient proxy)
        {
            var scenarioResult = new ScenarioResult();

            scenarioResult.Scenario = scenario;

            scenarioResult.StepResults.AddRange(RunScenario(scenario, -1, proxy).StepResults);

            scenarioResult.SetResult();

            return scenarioResult;
        }

        public static ScenarioResult RunScenario(Scenario scenario, int dataRow, IRemoteClient proxy)
        {
            var scenarioResult = new ScenarioResult() { Scenario = scenario };

            foreach (ScenarioStep s in scenario.Steps)
            {
                if(scenario.Table != null && scenario.Table.DataRows.Count > 0)
                    s.SetTestData(scenario.Table, dataRow);

                if (s.Table != null && s.Table.DataRows.Count > 0)
                    s.SetTestData(s.Table);


                scenarioResult.StepResults.Add(s.Run(proxy));

                scenarioResult.SetResult();

                if (scenarioResult.Result.status.Equals("FAIL"))
                    break;
            }

            return scenarioResult;
        }
    }
}
