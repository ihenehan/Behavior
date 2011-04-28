using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;


namespace Behavior.Common.Builders
{
    public interface IScenarioBuilder
    {
        List<Scenario> Build(Scenario scenario, List<Scenario> commonScenarios);
    }
}
