using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;

namespace Behavior.Common.Configuration
{
    public class BehaviorConfiguration
    {
        public List<string> IncludeTags { get; set; }
        public List<string> ExcludeTags { get; set; }
        public string ResultFile { get; set; }
        public string DataPath { get; set; }
        public string Host { get; set; }
        public string FixtureType { get; set; }
        public string GuiDelay { get; set; }
        public Dictionary<string, string> TestVariables { get; set; }

        public BehaviorConfiguration()
        {
            IncludeTags = new List<string>();
            ExcludeTags = new List<string>();
        }

        public string InsertTestVariables(string original)
        {
            var updated = "";

            foreach (KeyValuePair<string, string> kv in TestVariables)
                if (original.Contains(kv.Key))
                    updated = original.Replace(kv.Key, kv.Value.ToString());

            if (string.IsNullOrEmpty(updated))
                return original;

            return updated;
        }
    }
}
