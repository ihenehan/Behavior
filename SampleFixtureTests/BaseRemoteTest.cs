using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CookComputing.XmlRpc;
using NUnit.Framework;
using Behavior.Remote;
using Behavior.Remote.Client;
using Behavior.Remote.Results;
using SampleFixtureTests.Constants;
using SampleFixtureTests.BehaviorExtensions;

namespace SampleFixtureTests
{
    public class BaseRemoteTest
    {
        protected ILauncherClient client;
        protected IRemoteClient proxy;
        protected string launcherUrl = "http://164.57.104.26:65000/gui";
        protected int timeout = 2000;

        [SetUp]
        public virtual void Setup()
        {
            client = new LauncherClient();

            var httpResult = client.RequestFixtureLaunch(launcherUrl, 10);

            httpResult.ShouldPass();

            proxy = XmlRpcProxyGen.Create(typeof(IRemoteClient)) as IRemoteClient;

            proxy.Url = httpResult.retrn as string;

            //RunKeyword("LaunchPageInBrowser", Urls.Google).ShouldPass();

            BeforeTest();
        }

        [TearDown]
        public virtual void TearDown() 
        {
            try
            {
                BeforeTeardown();
            }
            catch (Exception e)
            {
                Console.WriteLine("BaseBrowserTest.TearDown: BeforeTeardown threw exception " + e.Message + "\r\n" + e.ToString());
            }

            //RunKeyword("CloseBrowser", true).ShouldPass();

            AfterBrowserTeardown();

            client.StopFixtureServer(launcherUrl).ShouldPass();
        }

        protected virtual void AfterBrowserTeardown() {}

        protected virtual void BeforeTest() {}

        protected virtual void BeforeTeardown() {}

        public virtual Result RunKeyword(string name, params object[] parameters)
        {
            return new Result(proxy.run_keyword(name, parameters) as XmlRpcStruct);
        }

        public void Wait(int timeout = 60000)
        {
            Thread.Sleep(timeout);
        }
    }
}
