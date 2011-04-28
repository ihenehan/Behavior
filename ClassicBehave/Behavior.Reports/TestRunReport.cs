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

            htmlBuilder.Append("<Div><B> Test Run Report: " + TestRunResult.Result.status + " </B><Div></br>");

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
