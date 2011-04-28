using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Remote.Attributes
{
    public class BeforeStory : Attribute
    {
        public string Name { get; set; }

        public BeforeStory()
        {
            Name = "BeforeStory";
        }

        public BeforeStory(string name)
        {
            Name = name;
        }
    }
}
