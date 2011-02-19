using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Remote.Results;
using WatiN.Core;
using SampleFixtureTests.Constants;
using WatinExtensions;
using System.Collections;

namespace SampleFixtureTests
{
    public partial class GoogleFixture : BaseFixture
    {
        public Result VerifyPageLoaded(string url)
        {
            if(browser.Url.Equals(url))
                return Result.CreatePass();

            return Result.CreateFail("Browser URL: " + browser.Url + " did not match expected URL: " + url);
        }

        public Result SearchGoogle(string searchText)
        {
            browser.FindByClass<TextField>(Classes.GoogleSearchTextBox).TypeTextFast(searchText);

            if (browser.FindById(Ids.GoogleMapsLinkId) != null)
                return Result.CreatePass();

            return Result.CreateFail("Could not find search text: " + searchText + " in the search result page.");
        }

        public Result IntAddition(int arg1, int arg2)
        {
            return Result.CreatePass((arg1 + arg2).ToString());
        }

        public Result IntSubtraction(int arg1, int arg2)
        {
            return Result.CreatePass((arg1 - arg2).ToString());
        }

        public Result Addition(string arg1, string arg2, string expected)
        {
            int iArg1 = -1;
            int iArg2 = -1;
            int iExpected = -1;

            int.TryParse(arg1, out iArg1);
            int.TryParse(arg2, out iArg2);
            int.TryParse(expected, out iExpected);

            if ((iArg1 + iArg2) == iExpected)
                return Result.CreatePass((iArg1 + iArg2).ToString());

            return Result.CreateFail(string .Format("Actual value {0} did not equal expected value {1}", (iArg1 + iArg2).ToString(), expected));
        }

        public Result Subtraction(string arg1, string arg2, string expected)
        {
            int iArg1 = -1;
            int iArg2 = -1;
            int iExpected = -1;

            int.TryParse(arg1, out iArg1);
            int.TryParse(arg2, out iArg2);
            int.TryParse(expected, out iExpected);

            if ((iArg1 - iArg2) == iExpected)
                return Result.CreatePass((iArg1 - iArg2).ToString());

            return Result.CreateFail(string.Format("Actual value {0} did not equal expected value {1}", (iArg1 - iArg2).ToString(), expected));
        }
    }
}
