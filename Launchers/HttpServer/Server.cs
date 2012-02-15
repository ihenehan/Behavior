using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace HttpServer
{
    public class Server
    {
        public RouteRegistry RouteRegistry
        {
            get { return routeRegistry; }
            set { routeRegistry = value; }
        }

        private HttpListener listener;
        private RouteRegistry routeRegistry = new RouteRegistry();

        public Server(List<string> addresses)
        {
            listener = CreateListener(addresses);
        }

        public void Listen()
        {
            while (true)
                ProcessRequest(listener.GetContext());
        }

        public void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                var request = new HttpRequest(context.Request);

                var response = HttpStatus.NotFound(request.RequestedController);

                if (string.IsNullOrEmpty(request.RequestedController) || request.RequestedController.StartsWith("index"))
                    request.RequestedController = "index";

                if (RouteRegistry.ContainsKey(request.RequestedController))
                    response = RouteRegistry.Route(request);

                response.Write(context, context.Request.ContentEncoding);
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());

                var response = HttpStatus.InternalError(e.ToString());

                response.Write(context, Encoding.UTF8);
            }
        }

        public HttpListener CreateListener(List<string> addresses)
        {
            var listener = new HttpListener();

            addresses.ForEach(a => listener.Prefixes.Add(a));

            listener.Start();

            return listener;
        }
    }
}
