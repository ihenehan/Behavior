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

            htmlBuilder.Append("<Div><B>" + StoryResult.Story.Name + ": " + StoryResult.Result.status + "</B><Div>");

            htmlBuilder.Append("<Div>" + StoryResult.Story.Description + "<Div></br>");

            htmlBuilder.Append("<Div>StartTime: " + StoryResult.StartTime + "<Div>");
            htmlBuilder.Append("<Div>EndTime: " + StoryResult.EndTime + "<Div>");
            htmlBuilder.Append("<Div>ExecutionTime: " + StoryResult.ExecutionTime + "<Div></br>");


            StoryResult.ScenarioResults.ForEach(s => htmlBuilder.AppendLine(new ScenarioReport(s).ToHtml()));

            htmlBuilder.Append("</Div>");

            return htmlBuilder.ToString();
        }
    }
}
