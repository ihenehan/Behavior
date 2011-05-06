using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Remote.Attributes
{
    public class BeforeCriterion : Attribute
    {
        public string Name { get; set; }

        public BeforeCriterion()
        {
            Name = "BeforeCriterion";
        }

        public BeforeCriterion(string name)
        {
            Name = name;
        }
    }
}
