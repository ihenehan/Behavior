using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HttpServer
{
    public class IndexController : Controller
    {
        public IndexController() : base() { }

        public override HttpResponse Get(HttpRequest request)
        {
            if (File.Exists(request.RequestedController + ".html"))
                return HttpStatus.Ok(File.ReadAllText(request.RequestedController + ".html"));

            return HttpStatus.Ok("");
        }
    }
}
