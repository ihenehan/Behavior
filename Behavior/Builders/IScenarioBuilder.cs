using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;


namespace Behavior.Builders
{
    interface IScenarioBuilder
    {
        List<Item> Build(List<Item> scenarios);
    }
}
