using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Remote.Attributes
{
    public class AfterStory : Attribute
    {
        public string Name { get; set; }

        public AfterStory()
        {
            Name = "AfterStory";
        }

        public AfterStory(string name)
        {
            Name = name;
        }
    }
}
