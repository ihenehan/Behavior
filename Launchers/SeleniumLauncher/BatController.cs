using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using HttpServer;
using System.Windows.Forms;

namespace SeleniumLauncher
{
    public class BatController : Controller
    {
        private Dictionary<string, Process> procList = new Dictionary<string, Process>();

        public BatController() : base() { }

        public override HttpResponse Get(HttpRequest request)
        {
            Console.WriteLine("BatController.Get: " + request.RequestedResource);

            if (!string.IsNullOrEmpty(request.RequestedResource))
                return GetProcessState(request.RequestedResource);

            return GetAllProcessesState();
        }

        public override HttpResponse Delete(HttpRequest request)
        {
            Console.WriteLine("BatController.Delete: " + request.RequestedResource);

            var content = "";

            if (string.IsNullOrEmpty(request.RequestedResource))
                content = RemoveAllProcesses(content);

            content = RemoveProcess(request.RequestedResource, content);

            return HttpStatus.Ok(content);
        }

        public override HttpResponse Post(HttpRequest request)
        {
            Console.WriteLine("BatController.Post: " + request.RequestedResource);

            if (File.Exists(request.RequestedResource + ".bat"))
                return StartProcess(request.RequestedResource);

            return HttpStatus.NotFound(request.RequestedResource);
        }

        public HttpResponse GetProcessState(string processKey)
        {
            if (procList.ContainsKey(processKey))
                if (!procList[processKey].HasExited)
                    return HttpStatus.Ok("Active");

            return HttpStatus.Ok("Inactive");
        }

        public HttpResponse GetAllProcessesState()
        {
            if (procList.Count > 0)
            {
                var builder = new StringBuilder();

                procList.ToList().ForEach(p => builder.Append(p.Key + " : " + "Active\r\n"));

                return HttpStatus.Ok(builder.ToString());
            }
                
            return HttpStatus.Ok("Ready - no processes active");
        }

        public HttpResponse StartProcess(string target)
        {
            var proc = new Process();

            proc.StartInfo.FileName = target + ".bat";

            proc.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

            proc.StartInfo.UseShellExecute = true;

            proc.StartInfo.WorkingDirectory = Application.StartupPath;

            Console.WriteLine("Starting: " + proc.StartInfo.WorkingDirectory + "\\" + proc.StartInfo.FileName);

            bool started = proc.Start();

            var response = HttpStatus.Ok("Success");

            if (!started)
            {
                response = HttpStatus.InternalError("Failed to start process: " + proc.StartInfo.WorkingDirectory + "\\" + proc.StartInfo.FileName);

                Console.WriteLine("StartProcess:" + response.Content);
            }

            procList.Add(target, proc);

            return response;
        }

        public string RemoveAllProcesses(string context)
        {
            procList.ToList().ForEach(p => context = RemoveProcess(p.Key, context));

            return context;
        }

        public string RemoveProcess(string key, string context)
        {
            if (procList.ContainsKey(key))
            {
                var proc = procList[key];

                if (proc != null)
                {
                    if (!proc.HasExited)
                    {
                        proc.CloseMainWindow();

                        if (!proc.HasExited)
                            proc.Kill();
                    }
                    procList.Remove(key);

                    context = context + "\r\n" + key + " has been stopped.";
                }
            }
            return context;
        }
    }
}
