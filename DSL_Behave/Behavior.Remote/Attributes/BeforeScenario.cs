using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Remote.Attributes
{
    public class BeforeScenario : Attribute
    {
        public string Name { get; set; }

        public BeforeScenario()
        {
            Name = "BeforeScenario";
        }

        public BeforeScenario(string name)
        {
            Name = name;
        }
    }
}
