using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Behavior.Common.Models
{
    public class Criterion : Item
    {
        private string name;

        public string CriterionType { get; set; }
        public List<CriterionStep> Steps { get; set; }
        public List<Criterion> BeforeCriteria { get; set; }
        public List<Criterion> AfterCriterion { get; set; }

        public override string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Criterion()
        {
            Name = "";
            Description = "";
            Tags = new List<Tag>();
            Steps = new List<CriterionStep>();
            BeforeCriteria = new List<Criterion>();
            AfterCriterion = new List<Criterion>();
        }

        public Criterion(Block block)
        {
            Name = block.Name;
            CriterionType = block.BlockType;
            Tags = block.Tags;
            Steps = new List<CriterionStep>();
            BeforeCriteria = new List<Criterion>();
            AfterCriterion = new List<Criterion>();
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

        public Criterion Clone()
        {
            var serialized = JsonConvert.SerializeObject(this, Formatting.Indented);

            return JsonConvert.DeserializeObject<Criterion>(serialized);
        }
    }
}
