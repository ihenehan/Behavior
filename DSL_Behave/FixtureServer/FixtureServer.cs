using System;
using System.Net;
using System.Net.Sockets;
using Behavior.Remote;
using SampleFixtureTests;
using CookComputing.XmlRpc;
using System.Collections.Generic;

namespace FixtureServer
{
    public class FixtureServer
    {
        private XmlRpcListenerService fixtureSvc = null;

        public static Dictionary<string, object> ScenarioContext { get; set; }
        public static Dictionary<string, object> StoryContext { get; set; }

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Host Name: " + Dns.GetHostName());

            var iphostentry = Dns.GetHostEntry(Dns.GetHostName());

            int nIP = 0;

            IPAddress ipAddressToUse = null;

            foreach (IPAddress ipAddress in iphostentry.AddressList)
            {
                Console.WriteLine("IP #" + ++nIP + ": " + ipAddress.ToString());

                if (ipAddress.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    ipAddressToUse = ipAddress;

                    break;
                }
            }

            var port = "64000";

            if (args.Length == 1)
                port = args[0];

            

            var server = new FixtureServer();

            ScenarioContext = new Dictionary<string, object>();
            StoryContext = new Dictionary<string, object>();

            var fixtureListener = new HttpListener();

            var prefix = "http://" + ipAddressToUse.ToString() + ":" + port + "/gui/";

            fixtureListener.Prefixes.Add(prefix);

            fixtureListener.Start();

            Console.WriteLine("FixtureServer listener started on " + prefix);

            server.Listen(fixtureListener);
        }

        public void Listen(HttpListener fixtureListener)
        {
            while (true)
            {
                var context = fixtureListener.GetContext();

                ListenerCallback(context);
            }
        }

        private void ListenerCallback(HttpListenerContext fixtureContext)
        {
            var context = fixtureContext.Request.RawUrl.Replace("/", "");

            if (context.ToLower().Equals("gui"))
            {
                if (fixtureSvc == null)
                    fixtureSvc = new SampleFixture() { ScenarioContext = FixtureServer.ScenarioContext,
                                                       StoryContext = FixtureServer.StoryContext };

                fixtureSvc.ProcessRequest(fixtureContext);
            }
        }
    }
}
