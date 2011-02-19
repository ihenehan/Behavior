using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;

namespace Behavior.Builders
{
    public class ScenarioBuilder : IScenarioBuilder
    {
        public List<Item> Build(List<Item> items)
        {
            return items;
        }
    }
}
