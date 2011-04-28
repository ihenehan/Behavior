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

            htmlBuilder.Append("<Div>" + ScenarioResult.Scenario.ScenarioType + ": " + ScenarioResult.Scenario.Name + " - " + ScenarioResult.Result.status + "<Div>");

            if (!string.IsNullOrEmpty(ScenarioResult.Result.retrn.ToString()))
                htmlBuilder.AppendLine("<Font Color=green>---->Return: " + ScenarioResult.Result.retrn.ToString() + "</Font>");

            ScenarioResult.StepResults.ForEach(i => htmlBuilder.AppendLine(new StepReport(i).ToHtml()));

            var lineBreak = "</br>";

            if (ScenarioResult.Scenario.ScenarioType.Equals("Scenario Common"))
                lineBreak = "";

            htmlBuilder.Append("</Div>" + lineBreak);

            return htmlBuilder.ToString();
        }
    }
}
