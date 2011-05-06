using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;
using Behavior.Remote.Results;

namespace Behavior.Reports
{
    public class CriterionReport
    {
        public CriterionResult CriterionResult { get; set; }

        public CriterionReport(CriterionResult criterionResult)
        {
            CriterionResult = criterionResult;
        }

        public string ToHtml()
        {
            var htmlBuilder = new StringBuilder();

            htmlBuilder.Append("<Div><B>->" + CriterionResult.Criterion.CriterionType + ":</B> " + CriterionResult.Criterion.Name + " - " + CriterionResult.Result.status + "<Div>");

            if (!string.IsNullOrEmpty(CriterionResult.Result.error))
                htmlBuilder.Append("<Div><Font Color=red>->Error: " + CriterionResult.Result.error + "</Font></Div>");

            CriterionResult.StepResults.ForEach(i => htmlBuilder.AppendLine(new StepReport(i).ToHtml()));

            //var lineBreak = "</br>";
            var lineBreak = "";

            if (CriterionResult.Criterion.CriterionType.Equals("Criterion Common"))
                lineBreak = "";

            htmlBuilder.Append("</Div>" + lineBreak);

            return htmlBuilder.ToString();
        }
    }
}
