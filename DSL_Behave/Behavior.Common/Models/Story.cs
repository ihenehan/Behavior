using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Behavior.Common.Extensions;
using Behavior.Common.Builders;
using Behavior;
using Behavior.Common.Repository;

namespace Behavior.Common.Models
{
    public class Story : Item
    {
        public List<string> DescriptionLines { get; set; }
        public List<Criterion> BeforeStories { get; set; }
        public List<Criterion> AfterStories { get; set; }
        public List<Criterion> CriterionCommon { get; set; }
        public List<Criterion> Criteria { get; set; }
        public List<string> IncludeTags { get; set; }
        public List<string> ExcludeTags { get; set; }
        public IRepository Repository { get; set; }
        
        public Story()
        {
            Name = "";
            Description = "";
            DescriptionLines = new List<string>();
            Tags = new List<Tag>();
            Criteria = new List<Criterion>();
            BeforeStories = new List<Criterion>();
            AfterStories = new List<Criterion>();
            CriterionCommon = new List<Criterion>();
        }

        public List<Criterion> TestSequence
        {
            get
            {
                var sequence = new List<Criterion>();

                sequence.AddRange(BeforeStories);

                foreach (Criterion s in Criteria)
                {
                    if (s.ShouldRun(IncludeTags, ExcludeTags))
                    {
                        sequence.Add(new Criterion() { CriterionType = "Context Reset" });

                        var builder = new CriterionBuilder();

                        if (s.CriterionType.Equals("Criterion Outline"))
                            sequence.AddRange(builder.BuildCriterionFromOutline(s, CriterionCommon));
                        
                        else
                            sequence.AddRange(builder.BuildCriterion(s, CriterionCommon));
                    }
                }

                sequence.AddRange(AfterStories);

                return sequence;
            }
        }
    }
}
