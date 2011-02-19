using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using CookComputing.XmlRpc;
using System.Threading;
using Behavior.Remote;
using Behavior.Remote.Client;
using Behavior.Remote.Results;

namespace FixtureLauncher
{
    public class FixtureLauncher
    {
        public static Process fixtureProc { get; set; }
        public static string procFinished { get; set; }

        private static int timeout;
        private static string fixtureServerPath;
        private static string fixtureUri;

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

            string port = "65000";

            foreach (string arg in args)
            {
                if (arg.ToLower().StartsWith("fixturepath="))
                    fixtureServerPath = arg.Split("=".ToCharArray())[1];

                if (arg.ToLower().StartsWith("port="))
                    port = arg.Split("=".ToCharArray())[1];

                if (arg.ToLower().StartsWith("timeout="))
                    if (!int.TryParse(arg.Split("=".ToCharArray())[1], out timeout))
                        timeout = 600000; //default ten minute timeout.

                if (arg.ToLower().StartsWith("uri"))
                    fixtureUri = arg.Split('=')[1];

            }

            var server = new FixtureLauncher();

            var listener = new HttpListener();
            
            var strIPAddress = ipAddressToUse.ToString();

            string prefix = "http://" + ipAddressToUse.ToString() + ":" + port + "/";
            
            listener.Prefixes.Add(prefix);
            
            listener.Start();

            Console.WriteLine("FixtureLauncher listener started on http://" + ipAddressToUse.ToString() + ":" + port + "/");

            server.Listen(listener);
        }

        public void Listen(HttpListener fixtureListener)
        {
            while (true)
            {
                HttpListenerContext context = fixtureListener.GetContext();

                ListenerCallback(context);
            }
        }

        private void ListenerCallback(HttpListenerContext fixtureContext)
        {
            try
            {
                var context = fixtureContext.Request.RawUrl.Replace("/", "");

                var responseMsg = "Unknown request.";

                var encoding = fixtureContext.Request.ContentEncoding;

                if (context.ToLower().Equals(fixtureUri))
                {
                    if (fixtureContext.Request.HttpMethod.ToLower().Equals("get"))
                        responseMsg = Get();

                    if (fixtureContext.Request.HttpMethod.ToLower().Equals("post"))
                        responseMsg = Post(fixtureContext.Request);

                    if (fixtureContext.Request.HttpMethod.ToLower().Equals("delete"))
                        responseMsg = Delete();
                }

                if (context.ToLower().Equals("iisresetstart"))
                    if (fixtureContext.Request.HttpMethod.ToLower().Equals("post"))
                        responseMsg = PostIISReset(fixtureContext.Request, "/START");

                if (context.ToLower().Equals("iisresetstop"))
                    if (fixtureContext.Request.HttpMethod.ToLower().Equals("post"))
                        responseMsg = PostIISReset(fixtureContext.Request, "/STOP");

                var response = fixtureContext.Response.OutputStream;

                var writer = new StreamWriter(response, encoding);

                writer.Write(responseMsg);

                writer.Close();

                fixtureContext.Response.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
                Console.Write(e.ToString());

                string responseMsg = "-1000";

                var response = fixtureContext.Response.OutputStream;
                
                var encoding = Encoding.UTF8;
                
                var writer = new StreamWriter(response, encoding);

                writer.Write(responseMsg);
                
                writer.Close();
                
                fixtureContext.Response.Close();
            }
        }

        private string Get()
        {
            try
            {
                var response = "Busy";

                if (fixtureProc == null)
                {
                    if (string.IsNullOrEmpty(procFinished))
                        response = "Ready";
                    else
                        response = procFinished;
                }
                else if (fixtureProc.HasExited)
                {
                    procFinished = "Complete: ExitCode=" + fixtureProc.ExitCode;

                    fixtureProc = null;
                }
                else
                    response = "Busy";

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("FixtureLauncher.Get: " + e.Message + "\r\n" + e.ToString());

                return "FixtureLauncher.Get: " + e.Message + "\r\n" + e.ToString();
            }
        }

        private string Delete()
        {
            Console.WriteLine("FixtureLauncher.ListenerCallback: Processing HTTP DELETE request.");

            var response = "";

            if ((fixtureProc == null) && (fixtureProc.HasExited))
                response = "Ready";
            else
            {
                if (!fixtureProc.HasExited)
                    fixtureProc.Kill();

                fixtureProc = null;

                response = "Ready";
            }

            Console.WriteLine("FixtureLauncher.ListenerCallback: DELETE returned " + response);

            return response;
        }

        private string Post(HttpListenerRequest request)
        {
            Console.WriteLine("FixtureLauncher.ListenerCallback: Processing HTTP POST request.");

            string response;

            if (fixtureProc == null)
            {
                fixtureProc = new Process();

                fixtureProc.StartInfo = new ProcessStartInfo(fixtureServerPath + "\\FixtureServer.exe", "");

                fixtureProc.StartInfo.UseShellExecute = true;

                fixtureProc.StartInfo.WorkingDirectory = fixtureServerPath;

                bool started = fixtureProc.Start();

                if (started)
                {
                    var proxy = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));

                    proxy.Url = "http://" + request.LocalEndPoint.Address.ToString() + ":64000/" + fixtureUri;

                    var Result = new Result();

                    int timeout = 0;

                    while (timeout < 1000)
                    {
                        try
                        {
                            Result = proxy.Echo("OK");

                            timeout = 1000;

                            Console.WriteLine("FixtureLauncher.ListenerCallback: Echo returned " + Result.retrn.ToString());
                        }
                        catch (WebException webEx)
                        {
                            Console.Write(webEx.ToString());

                            response = "FAIL";
                        }

                        Thread.Sleep(100);

                        Application.DoEvents();

                        timeout = timeout + 100;
                    }

                    if (Result.retrn.Equals("OK"))
                        response = proxy.Url;
                    else
                    {
                        response = "FixtureServer is unresponsive. Killing process!";

                        fixtureProc.Kill();

                        fixtureProc = null;
                    }
                }
                else
                    response = "Error - Could not start TestClient process!";
            }
            else
            {
                response = "Busy";
            }

            Console.WriteLine("FixtureLauncher.ListenerCallback: POST returned " + response);

            return response;
        }
        
        private string PostIISReset(HttpListenerRequest request, string arg)
        {
            Console.WriteLine("FixtureLauncher.PostIISReset: Processing " + arg + " request.");

            Process IISResetProc = new Process();

            Stream body = request.InputStream;

            Encoding encoding = Encoding.UTF8;

            StreamReader reader = new System.IO.StreamReader(body, encoding);

            string s = reader.ReadToEnd();

            reader.Close();

            body.Close();

            string response;

            IISResetProc = new Process();

            IISResetProc.StartInfo = new ProcessStartInfo("IISReset", arg);

            IISResetProc.StartInfo.UseShellExecute = true;

            IISResetProc.StartInfo.WorkingDirectory = "c:\\";

            bool started = IISResetProc.Start();

            if (started)
            {
                IISResetProc.WaitForExit(180000);

                response = IISResetProc.ExitCode.ToString();

                if (!IISResetProc.HasExited)
                    IISResetProc.Kill();

                IISResetProc = null;
            }
            else
                response = "-1";

            Console.WriteLine("FixtureLauncher.PostIISReset: " + arg + " returned " + response);

            return response;
        }

        private static void OutputEventHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            Console.WriteLine(outLine.Data);

            ProcessMonitor.TimeoutCounter = 0;
        }
    }
}
