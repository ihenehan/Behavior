using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Remote.Attributes
{
    public class AfterCriterion : Attribute
    {
        public string Name { get; set; }

        public AfterCriterion()
        {
            Name = "AfterCriterion";
        }

        public AfterCriterion(string name)
        {
            Name = name;
        }
    }
}
