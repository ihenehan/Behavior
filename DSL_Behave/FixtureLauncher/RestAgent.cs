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

namespace Behavior.Agent
{
    public class RestAgent
    {
        public static Process xmlRpcServerProc { get; set; }
        public static string procFinished { get; set; }

        private static int timeout;
        private static string xmlRpcServerPath;
        private static string xmlRpcServerUri;

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
                    xmlRpcServerPath = arg.Split("=".ToCharArray())[1];

                if (arg.ToLower().StartsWith("port="))
                    port = arg.Split("=".ToCharArray())[1];

                if (arg.ToLower().StartsWith("timeout="))
                    if (!int.TryParse(arg.Split("=".ToCharArray())[1], out timeout))
                        timeout = 600000; //default ten minute timeout.

                if (arg.ToLower().StartsWith("uri"))
                    xmlRpcServerUri = arg.Split('=')[1];

            }

            var server = new RestAgent();

            var listener = new HttpListener();
            
            var strIPAddress = ipAddressToUse.ToString();

            string prefix = "http://" + ipAddressToUse.ToString() + ":" + port + "/";
            
            listener.Prefixes.Add(prefix);
            
            listener.Start();

            Console.WriteLine("RestAgent listener started on http://" + ipAddressToUse.ToString() + ":" + port + "/");

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

                if (context.ToLower().Equals(xmlRpcServerUri))
                {
                    if (fixtureContext.Request.HttpMethod.ToLower().Equals("get"))
                        responseMsg = Get();

                    if (fixtureContext.Request.HttpMethod.ToLower().Equals("post"))
                        responseMsg = Post(fixtureContext.Request);

                    if (fixtureContext.Request.HttpMethod.ToLower().Equals("delete"))
                        responseMsg = Delete();
                }

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

                if (xmlRpcServerProc == null)
                {
                    if (string.IsNullOrEmpty(procFinished))
                        response = "Ready";
                    else
                        response = procFinished;
                }
                else if (xmlRpcServerProc.HasExited)
                {
                    procFinished = "Complete: ExitCode=" + xmlRpcServerProc.ExitCode;

                    xmlRpcServerProc = null;
                }
                else
                    response = "Busy";

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("RestAgent.Get: " + e.Message + "\r\n" + e.ToString());

                return "RestAgent.Get: " + e.Message + "\r\n" + e.ToString();
            }
        }

        private string Delete()
        {
            Console.WriteLine("RestAgent.ListenerCallback: Processing HTTP DELETE request.");

            var response = "";

            if ((xmlRpcServerProc == null) && (xmlRpcServerProc.HasExited))
                response = "Ready";
            else
            {
                if (!xmlRpcServerProc.HasExited)
                    xmlRpcServerProc.Kill();

                xmlRpcServerProc = null;

                response = "Ready";
            }

            Console.WriteLine("RestAgent.ListenerCallback: DELETE returned " + response);

            return response;
        }

        private string Post(HttpListenerRequest request)
        {
            Console.WriteLine("RestAgent.ListenerCallback: Processing HTTP POST request.");

            string response;

            if (xmlRpcServerProc == null)
            {
                xmlRpcServerProc = new Process();

                xmlRpcServerProc.StartInfo = new ProcessStartInfo(xmlRpcServerPath + "\\XmlRpcServer.exe", "");

                xmlRpcServerProc.StartInfo.UseShellExecute = true;

                xmlRpcServerProc.StartInfo.WorkingDirectory = xmlRpcServerPath;

                bool started = xmlRpcServerProc.Start();

                if (started)
                {
                    var proxy = (IXmlRpcClient)XmlRpcProxyGen.Create(typeof(IXmlRpcClient));

                    proxy.Url = "http://" + request.LocalEndPoint.Address.ToString() + ":64000/" + xmlRpcServerUri;

                    var Result = new Result();

                    int timeout = 0;

                    while (timeout < 1000)
                    {
                        try
                        {
                            Result = proxy.Echo("OK");

                            timeout = 1000;

                            Console.WriteLine("RestAgent.ListenerCallback: Echo returned " + Result.retrn.ToString());
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
                        response = "XmlRpcServer is unresponsive. Killing process!";

                        xmlRpcServerProc.Kill();

                        xmlRpcServerProc = null;
                    }
                }
                else
                    response = "Error - Could not start TestClient process!";
            }
            else
            {
                response = "Busy";
            }

            Console.WriteLine("RestAgent.ListenerCallback: POST returned " + response);

            return response;
        }
        
        //private static void OutputEventHandler(object sendingProcess, DataReceivedEventArgs outLine)
        //{
        //    Console.WriteLine(outLine.Data);

        //    ProcessMonitor.TimeoutCounter = 0;
        //}
    }
}
