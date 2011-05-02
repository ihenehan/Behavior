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
        public List<Scenario> BeforeStories { get; set; }
        public List<Scenario> AfterStories { get; set; }
        public List<Scenario> ScenarioCommon { get; set; }
        public List<Scenario> Scenarios { get; set; }
        public List<string> IncludeTags { get; set; }
        public List<string> ExcludeTags { get; set; }
        public IRepository Repository { get; set; }
        
        public Story()
        {
            Name = "";
            Description = "";
            DescriptionLines = new List<string>();
            Tags = new List<Tag>();
            Scenarios = new List<Scenario>();
            BeforeStories = new List<Scenario>();
            AfterStories = new List<Scenario>();
            ScenarioCommon = new List<Scenario>();
        }

        public List<Scenario> TestSequence
        {
            get
            {
                var sequence = new List<Scenario>();

                sequence.AddRange(BeforeStories);

                foreach (Scenario s in Scenarios)
                {
                    if (s.ShouldRun(IncludeTags, ExcludeTags))
                    {
                        sequence.Add(new Scenario() { ScenarioType = "Context Reset" });

                        sequence.AddRange(s.BeforeScenarios);

                        if (s.ScenarioType.Equals("Scenario Outline"))
                        {
                            var builder = new ScenarioBuilder(Repository);

                            sequence.AddRange(builder.Build(s, ScenarioCommon));
                        }
                        else
                        {
                            if (ScenarioCommon != null)
                                sequence.AddRange(ScenarioCommon.Clone());

                            sequence.Add(s);
                        }

                        sequence.AddRange(s.AfterScenarios);
                    }
                }

                sequence.AddRange(AfterStories);

                return sequence;
            }
        }
    }
}
