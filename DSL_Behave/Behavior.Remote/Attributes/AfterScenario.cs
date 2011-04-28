using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Remote.Attributes
{
    public class AfterScenario : Attribute
    {
        public string Name { get; set; }

        public AfterScenario()
        {
            Name = "AfterScenario";
        }

        public AfterScenario(string name)
        {
            Name = name;
        }
    }
}
