using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using CookComputing.XmlRpc;
using NUnit.Framework;
using Behavior.Remote;
using Behavior.Remote.Server;
using Behavior.Remote.Results;
using Behavior.Remote.Attributes;
using Behavior.Logging;
using SampleFixtureTests.Constants;
using WatinExtensions;
using WatiN.Core;

namespace SampleFixtureTests
{
    [BindFixture]
    public partial class BaseFixture : RemoteServer, IRemoteServer
    {
        private bool isLocal = true;
        protected IE browser;

        public override void Dispose()
        {
            browser.Dispose();
        }

        public bool IsLocal
        {
            set { isLocal = value; }
            get { return isLocal; }
        }

        [BindKeyword]
        public Result ForceBrowserClose(bool forceClose)
        {
            if (isLocal || forceClose)
            {
                try
                {
                    browser = new IE(Urls.Root);

                    browser.WaitForComplete();
                }
                catch (Exception e)
                {
                    Console.WriteLine("BaseFixture.ForceBrowserForDatabaseReset: " + e.ToString());

                    Thread.Sleep(500);

                    browser = new IE(Urls.Root);

                    browser.WaitForComplete();
                }
                
                CloseBrowser(forceClose);
            }

            return Result.CreatePass();
        }

        [BindKeyword]
        public Result LaunchPageInBrowser(string Url)
        {
            try
            {
                browser = new IE(Url);

                browser.WaitForComplete();
            }
            catch (Exception e)
            {
                Console.WriteLine("BaseFixture.LaunchPageInBrowser: " + e.ToString());

                Thread.Sleep(500);

                browser = new IE(Url);

                browser.WaitForComplete();
            }

            browser.BringToFront();

            browser.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Maximize);

            return Result.CreatePass();
        }

        [BindKeyword]
        public Result GetBrowserScreen(string name)
        {
            browser.CaptureWebPageToFile(name);

            return Result.CreatePass();
        }

        [BindKeyword]
        public void GoBack()
        {
            browser.Back();
        }

        [BindKeyword]
        public Result CloseBrowser(bool forceClose)
        {
            try
            {
                browser.ClearCache();

                browser.ClearCookies();
                
                if (forceClose)
                    browser.ForceClose();
                else
                    browser.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine( "CloseBrowserException: " + e.ToString());
            }

            browser.Dispose();

            return Result.CreatePass();
        }
    }
}