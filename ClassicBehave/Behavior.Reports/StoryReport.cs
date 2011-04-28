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

            htmlBuilder.Append("<Div><B>" + StoryResult.StoryName + ": " + StoryResult.Result.status + "</B><Div>");

            StoryResult.ScenarioResults.ForEach(s => htmlBuilder.AppendLine(new ScenarioReport(s).ToHtml()));

            htmlBuilder.Append("</Div>");

            return htmlBuilder.ToString();
        }
    }
}
