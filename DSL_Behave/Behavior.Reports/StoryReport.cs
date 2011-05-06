using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;
using Behavior.Remote.Results;

namespace Behavior.Reports
{
    public class StoryReport
    {
        public StoryResult StoryResult { get; set; }

        public StoryReport(StoryResult storyResult)
        {
            StoryResult = storyResult;
        }

        public string ToHtml()
        {
            var htmlBuilder = new StringBuilder();

            htmlBuilder.Append("<Div><B>Story: " + StoryResult.Story.Name + ":</B> " + StoryResult.Result.status + "<Div>");

            htmlBuilder.Append("<Div><B>StartTime:</B> " + StoryResult.StartTime + " " +
                                    "<B>EndTime:</B> " + StoryResult.EndTime + " " +
                                    "<B>ExecutionTime:</B> " + StoryResult.ExecutionTime + "<Div>");
            
            foreach (string s in StoryResult.Story.DescriptionLines)
                htmlBuilder.Append("<Div>" + s + "<Div>");

            if (!string.IsNullOrEmpty(StoryResult.Result.error))
                htmlBuilder.Append("<Div><Font Color=red>Error: " + StoryResult.Result.error + "</Font></Div>");

            StoryResult.ScenarioResults.ForEach(s => htmlBuilder.AppendLine(new ScenarioReport(s).ToHtml()));

            htmlBuilder.Append("</Div></br>");

            return htmlBuilder.ToString();
        }
    }
}
