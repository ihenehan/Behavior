using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using HttpServer;

namespace SeleniumLauncher
{
    public class IEController : Controller
    {
        public IEController() : base() {}

        public override HttpResponse Get(HttpRequest request)
        {
            Console.WriteLine("IEController.Get: " + request.RequestedResource);

            var ieProcs = Process.GetProcessesByName("iexplore");

            return HttpStatus.Ok(ieProcs.Count().ToString() + " instances of IE running.");
        }

        public override HttpResponse Delete(HttpRequest request)
        {
            Console.WriteLine("IEController.Delete: " + request.RequestedResource);

            KillProcs("iexplore");

            return HttpStatus.Ok("Done");
        }

        public void KillProcs(string name)
        {
            var ieProcs = Process.GetProcessesByName(name).ToList();

            foreach (Process p in ieProcs)
            {
                p.CloseMainWindow();

                if (!p.HasExited)
                    p.Kill();
            }
        }
    }
}
