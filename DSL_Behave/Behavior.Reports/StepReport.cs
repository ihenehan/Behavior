using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;
using Behavior.Remote.Results;

namespace Behavior.Reports
{
    public class StepReport
    {
        public StepResult StepResult { get; set; }

        public StepReport(StepResult stepResult)
        {
            StepResult = stepResult;
        }

        public string ToHtml()
        {
            var htmlBuilder = new StringBuilder();

            if(StepResult.Result.status.StartsWith("PASS"))
                htmlBuilder.Append("<Div><Font Color=green>-->" + StepResult.KeywordName + "</Font><Div>");
            else
                htmlBuilder.Append("<Div><Font Color=red>-->" + StepResult.KeywordName + ": " + StepResult.Result.status + "</Font><Div>");

            htmlBuilder.AppendLine(new ResultReport(StepResult.Result).ToHtml());

            htmlBuilder.Append("</Div>");

            return htmlBuilder.ToString();
        }
    }
}
