using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Behavior.Common.Models
{
    public class ScenarioStep : Item
    {
        public string Keyword { get; set; }
        public List<object> Parameters { get; set; }

        public ScenarioStep()
        {
            Name = "";
            Description = "";
            Tags = new List<Tag>();
            Keyword = "Step";
            Parameters = new List<object>();
        }

        public ScenarioStep(string keyword, string command)
        {
            var processedStep = new ProcessedStep(command);

            Name = processedStep.ProcessedCommand;

            Keyword = keyword;

            Parameters = processedStep.Parameters;
        }

        public void SetTestData(Table table)
        {
            Parameters.Add(table);
        }

        public void SetTestData(Table table, int row)
        {
            if (table != null && row > -1)
                for (int i = 0; i < Parameters.Count; i++)
                {
                    var key = Parameters[i].ToString().Replace("<", "").Replace(">", "");

                    if (Parameters[i].ToString().StartsWith("<") && Parameters[i].ToString().EndsWith(">"))
                        Parameters[i] = table.GetCellValue(key, row);
                }
        }
    }
}
