using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Behavior.Common.Models
{
    public class Scenario : Item
    {
        private string name;

        public string ScenarioType { get; set; }
        public List<ScenarioStep> Steps { get; set; }
        public List<Scenario> BeforeScenarios { get; set; }
        public List<Scenario> AfterScenarios { get; set; }

        public override string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Scenario()
        {
            Name = "";
            Description = "";
            Tags = new List<Tag>();
            Steps = new List<ScenarioStep>();
            BeforeScenarios = new List<Scenario>();
            AfterScenarios = new List<Scenario>();
        }

        public bool ShouldRun(List<string> IncludeTags, List<string>ExcludeTags)
        {
            if (IncludeTags != null && IncludeTags.Count > 0)
                if (Tags.Any(t => IncludeTags.Any(i => i.ToLower().Equals(t.Name.ToLower()))))
                    if (ExcludeTags != null && ExcludeTags.Count > 0)
                        if (ExcludeTags.Any(e => Tags.Any(t => t.Name.ToLower().Equals(e.ToLower()))))
                            return false;
                        else
                            return true;
                    else
                        return true;
                else
                    return false;
            else
                if (ExcludeTags != null && ExcludeTags.Count > 0)
                    if (ExcludeTags.Any(e => Tags.Any(t => t.Name.ToLower().Equals(e.ToLower()))))
                        return false;
                    else
                        return true;
                else
                    return true;
        }

        public Scenario Clone()
        {
            var serialized = JsonConvert.SerializeObject(this, Formatting.Indented);

            return JsonConvert.DeserializeObject<Scenario>(serialized);
        }
    }
}
