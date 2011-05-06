using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Common.Models;
using Behavior.Remote.Results;

namespace Behavior.Reports
{
    public class TestRunReport
    {
        public TestRunResult TestRunResult { get; set; }

        public TestRunReport(TestRunResult testRunResult)
        {
            TestRunResult = testRunResult;
        }

        public string ToHtml()
        {
            var htmlBuilder = new StringBuilder();

            htmlBuilder.Append("<Div><B> Test Run Report:</B> " + TestRunResult.Result.status + " <Div>");

            htmlBuilder.Append("<Div><B>StartTime:</B> " + TestRunResult.StartTime + " " +
                                    "<B>EndTime:</B> " + TestRunResult.EndTime + " " + 
                                    "<B>ExecutionTime:</B> " + TestRunResult.ExecutionTime + "<Div></br>");

            htmlBuilder.Append("<Div><B>Passing Stories:</B>" + TestRunResult.PassingStories.ToString() + "<Div>");
            htmlBuilder.Append("<Div><B>Passing Criteria:</B>" + TestRunResult.PassingCriteria.ToString() + "<Div>");
            htmlBuilder.Append("<Div><B>Passing Steps:</B>" + TestRunResult.PassingSteps + "<Div>");

            htmlBuilder.Append("<Div><B>Failing Stories:</B>" + TestRunResult.FailedStories.ToString() + "<Div>");
            htmlBuilder.Append("<Div><B>Failing Criteria:</B>" + TestRunResult.FailedCriteria.ToString() + "<Div>");
            htmlBuilder.Append("<Div><B>Failing Steps:</B>" + TestRunResult.FailedSteps + "<Div></br>");

            TestRunResult.StoryResults.ForEach(s => htmlBuilder.AppendLine(new StoryReport(s).ToHtml()));

            htmlBuilder.Append("</Div>");

            return htmlBuilder.ToString();
        }

        public void ToFile(string fileName)
        {
            var html = ToHtml();

            File.WriteAllText(fileName, html);
        }
    }
}
