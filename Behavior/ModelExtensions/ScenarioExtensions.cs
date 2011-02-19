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

            scenarioResult.ScenarioName = scenario.Name;

            scenario.Children = scenario.Children.OrderBy(d => d.Index).ToList();

            scenario.Children.ForEach(i => scenarioResult.InteractionResults.Add((i as Interaction).Run(proxy)));

            scenarioResult.SetResult();

            if (!scenario.ExpectFailure)
                return scenarioResult;
            else
                if (scenarioResult.Result.status.ToLower().Equals("pass"))
                {
                    scenarioResult.Result.status = "FAIL";
                    scenarioResult.Result.error = "Expected failure, but scenario incorrectly passed.";
                }
                else
                {
                    scenarioResult.Result.status = "PASS";
                    scenarioResult.Result.error = "";
                    scenarioResult.Result.retrn = "Scenario failed correctly.";
                }
            
            return scenarioResult;
        }
    }
}
