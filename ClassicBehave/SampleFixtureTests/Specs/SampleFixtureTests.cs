using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Behavior.Remote.Results;
using SampleFixtureTests.Constants;
using SampleFixtureTests.BehaviorExtensions;
using SampleFixtureTests.DataProviders;
using SampleFixtureTests.TestDataTypes;
using System.Collections;
using CookComputing.XmlRpc;

namespace SampleFixtureTests.Specs
{
    //[TestFixture]
    //public class SampleFixtureTests : BaseBrowserTest
    //{
    //    [Test]
    //    [Ignore]
    //    public void OpenGoogle()
    //    {
    //        var result = fixture.VerifyPageLoaded(Urls.Google);

    //        Wait(timeout);

    //        result.ShouldPass();
    //    }

    //    [Test]
    //    [Ignore]
    //    public void SearchGoogle()
    //    {
    //        var result = fixture.SearchGoogle("Google Maps");

    //        Wait(timeout);

    //        result.ShouldPass();
    //    }
    //}

    //[TestFixture]
    //public class SampleRemoteTests : BaseRemoteTest
    //{
    //    [Test]
    //    [Ignore]
    //    public void OpenGoogle()
    //    {
    //        var result = RunKeyword("VerifyPageLoaded", Urls.Google);

    //        Wait(timeout);

    //        result.ShouldPass();
    //    }

    //    [Test]
    //    [Ignore]
    //    public void SearchGoogle()
    //    {
    //        var result = RunKeyword("SearchGoogle", "Google Maps");

    //        Wait(timeout);

    //        result.ShouldPass();
    //    }
    //}

    [TestFixture]
    public class given_firm_admin_requests_all_letter_templates : BaseRemoteTest
    {
        [Test(Description = "Addition Test")]
        [TestCaseSource("TestDataList_FiveInts")]
        public void when_adding_ints(IntTestData data)
        {
            var result = RunKeyword("Addition", data.FirstInt, data.SecondInt);

            Assert.AreEqual(data.ExpectedAdd.ToString(), result.retrn);
        }

        [Test(Description = "Subtraction Test")]
        [TestCaseSource("TestDataList_TenInts")]
        public void when_subtracting_ints(IntTestData data)
        {
            var result = RunKeyword("Subtraction", data.FirstInt, data.SecondInt);

            Assert.AreEqual(data.ExpectedSubtract.ToString(), result.retrn);
        }

        public virtual IEnumerable TestDataList_FiveInts
        {
            get
            {
                var dataProvider = new IntDataProvider(5);

                foreach (IntTestData d in dataProvider.DataList)
                    yield return d;
            }
        }

        public virtual IEnumerable TestDataList_TenInts
        {
            get
            {
                var dataProvider = new IntDataProvider(10);

                foreach (IntTestData d in dataProvider.DataList)
                    yield return d;
            }
        }

        //[Test]
        //public void should_return_all_letter_templates()
        //{
        //    Assert.Fail("Not Implemented.");
        //}

        //[Test]
        //public void given_unsupported_tax_app_should_return_zero()
        //{
        //    Assert.Fail("Not Implemented.");
        //}

        //[Test]
        //public void given_supported_tax_app_with_no_templates_should_return_zero()
        //{
        //    Assert.Fail("Not Implemented.");
        //}
    }
}
