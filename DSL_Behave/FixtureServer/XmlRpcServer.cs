﻿using System;
using System.Net;
using System.Net.Sockets;
using Behavior.Remote;
using SampleFixtureTests;
using CookComputing.XmlRpc;
using System.Collections.Generic;

namespace Behavior.XmlRpcServer
{
    public class XmlRpcServer
    {
        private XmlRpcListenerService xmlRpcListenerService = null;

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
            

            var server = new XmlRpcServer();

            StoryContext = new Dictionary<string, object>();

            var fixtureListener = new HttpListener();

            var prefix = "http://" + ipAddressToUse.ToString() + ":" + port + "/sample/";

            fixtureListener.Prefixes.Add(prefix);

            fixtureListener.Start();

            Console.WriteLine("XmlRpcServer listener started on " + prefix);

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

            if (context.ToLower().Equals("sample"))
            {
                if (xmlRpcListenerService == null)
                    xmlRpcListenerService = new SampleFixture() { StoryContext = XmlRpcServer.StoryContext };

                xmlRpcListenerService.ProcessRequest(fixtureContext);
            }
        }
    }
}
