using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using HttpServer;

namespace SeleniumLauncher
{
    public class SeleniumLauncher
    {
        private static Server server;

        [STAThread]
        static void Main(string[] args)
        {
            string port = "65000";

            foreach (string arg in args)
                if (arg.ToLower().StartsWith("port="))
                    port = arg.Split("=".ToCharArray())[1];

            var addresses = new List<string>()
            { 
                "http://" + GetIP().ToString() + ":" + port + "/",
                "http://localhost:" + port + "/",
                "http://127.0.0.1:" + port + "/" 
            };

            Console.WriteLine("Host Name: " + Dns.GetHostName());
            addresses.ForEach(a => Console.WriteLine(a));

            server = new Server(addresses);
            RegisterControllers();
            server.Listen();
        }

        public static void RegisterControllers()
        {
            var controllers = new List<IController>() { new IndexController(),
                                                        new BatController(),
                                                        new IEController(),
                                                        new ScreenController() };

            controllers.ForEach(c => server.RouteRegistry.Register(c));
        }

        public static IPAddress GetIP()
        {
            var iphostentry = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ipAddress in iphostentry.AddressList)
                if (ipAddress.AddressFamily.Equals(AddressFamily.InterNetwork))
                    return ipAddress;

            return null;
        }
    }
}
