using System;
using System.IO;
using System.Threading;
using Behavior.Logging;
using SampleFixtureTests.Constants;
using SampleFixtureTests.BehaviorExtensions;
using NUnit.Framework;

namespace SampleFixtureTests
{
    public abstract class BaseBrowserTest
    {
        protected GoogleFixture fixture;
        protected int timeout = 2000;

        [SetUp]
        public virtual void Setup()
        {
            fixture = new GoogleFixture();

            fixture.LaunchPageInBrowser(Urls.Google).ShouldPass();

            BeforeTest();
        }

        [TearDown]
        public virtual void TearDown()
        {
            try
            {
                string name = this.GetType().ToString() + ".jpg";

                if (!fixture.IsLocal)
                    name = "D:\\TestResults\\" + name;

                if (File.Exists(name))
                    File.Delete(name);

                fixture.GetBrowserScreen(name).ShouldPass();
            }
            catch (Exception e)
            {
                Console.WriteLine("BaseBrowserTest.TearDown: Attempt to capture screen threw exception " + e.Message + "\r\n" + e.ToString());
            }

            try
            {
                BeforeTeardown();
            }
            catch (Exception e)
            {
                Console.WriteLine("BaseBrowserTest.TearDown: BeforeTeardown threw exception " + e.Message + "\r\n" + e.ToString());
            }

            fixture.CloseBrowser(true).ShouldPass();

            AfterBrowserTeardown();

            fixture.Dispose();

            fixture = null;
        }

        protected virtual void AfterBrowserTeardown() {}

        protected virtual void BeforeTest() {}

        protected virtual void BeforeTeardown() {}

        public void Wait(int timeout = 60000)
        {
            Thread.Sleep(timeout);
        }
    }
}