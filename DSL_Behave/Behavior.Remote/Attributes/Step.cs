using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Remote.Attributes
{
    public class Step : Attribute
    {
        public string Name { get; set; }

        public Step(string name)
        {
            Name = name;
        }
    }
}
