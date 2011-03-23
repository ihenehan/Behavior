using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Behavior.Common.LookUps;

namespace Behavior.Common.Models
{
    public class Scenario : Item
    {
        private string name;

        public bool ExpectFailure { get; set; }
        public bool Selected { get; set; }
        public string ScenarioType { get; set; }
        public string ExpectedResult { get; set; }
        public List<Interaction> Interactions { get; set; }
        public List<TestData> TestDataRows { get; set; }

        public override string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool IsExpanded { get; set; }

        public Scenario()
        {
            Name = "";
            Description = "";
            Selected = true;
            ScenarioType = StringLookUps.ScenarioTypesDefault;
            Id = Guid.NewGuid();
            Tags = new List<string>();
            Type = "Scenario";
            IsLast = true;
            Interactions = new List<Interaction>();
            ChildrenIds = new List<Guid>();
            ChildrenType = null;
            TestDataRows = new List<TestData>();
        }

        public Scenario Clone()
        {
            var serialized = JsonConvert.SerializeObject(this, Formatting.Indented);

            return JsonConvert.DeserializeObject<Scenario>(serialized);
        }
    }
}
