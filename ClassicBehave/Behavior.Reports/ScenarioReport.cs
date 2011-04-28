using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;
using Behavior.Remote.Results;

namespace Behavior.Reports
{
    public class ScenarioReport
    {
        public ScenarioResult ScenarioResult { get; set; }

        public ScenarioReport(ScenarioResult scenarioResult)
        {
            ScenarioResult = scenarioResult;
        }

        public string ToHtml()
        {
            var htmlBuilder = new StringBuilder();

            htmlBuilder.Append("<Div>" + ScenarioResult.ScenarioName + ": " + ScenarioResult.Result.status + "<Div>");

            if (!string.IsNullOrEmpty(ScenarioResult.Result.retrn.ToString()))
                htmlBuilder.AppendLine("<Font Color=green>---->Return: " + ScenarioResult.Result.retrn.ToString() + "</Font>");

            ScenarioResult.InteractionResults.ForEach(i => htmlBuilder.AppendLine(new InteractionReport(i).ToHtml()));

            htmlBuilder.Append("</Div></br>");

            return htmlBuilder.ToString();
        }
    }
}
