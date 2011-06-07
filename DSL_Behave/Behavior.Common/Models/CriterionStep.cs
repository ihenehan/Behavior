using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Behavior.Common.Models
{
    public class CriterionStep : Item
    {
        public string Keyword { get; set; }
        public List<object> Parameters { get; set; }

        public CriterionStep()
        {
            Name = "";
            Description = "";
            Tags = new List<Tag>();
            Keyword = "Step";
            Parameters = new List<object>();
        }

        public CriterionStep(string line)
        {
            Keyword = FindKeyword(line);

            var processedStep = new ProcessedStep(line.Replace(Keyword, "").Trim());

            Name = processedStep.ProcessedCommand;

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

        public string FindKeyword(string line)
        {
            return line.Split(' ')[0].Trim();
        }
    }
}
