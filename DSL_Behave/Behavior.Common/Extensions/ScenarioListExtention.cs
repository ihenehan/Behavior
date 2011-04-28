using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;

namespace Behavior.Common.Extensions
{
    public static class ScenarioListExtention
    {
        public static List<Scenario> Clone(this List<Scenario> scenarios)
        {
            var newList = new List<Scenario>();

            scenarios.ForEach(s => newList.Add(s.Clone()));

            return newList;
        }
    }
}
