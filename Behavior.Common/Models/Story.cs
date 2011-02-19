using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Behavior.Common.Models
{
    public class Story : Item
    {
        [JsonIgnore]
        public List<Scenario> Scenarios { get; set; }

        public Story()
        {
            Name = "";
            Description = "";
            Id = Guid.NewGuid();
            Tags = new List<string>();
            Type = "Story";
            IsLast = true;
            ChildrenType = typeof(Scenario);
            ChildrenIds = new List<Guid>();
            Scenarios = new List<Scenario>();
        }
    }
}
