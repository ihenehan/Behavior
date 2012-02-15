using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace HttpServer
{
    public class RouteRegistry : Dictionary<string, IController>
    {
        public void Register(IController controller)
        {
            this.Add(controller.Name, controller);
        }

        public HttpResponse Route(HttpRequest request)
        {
            var controller = this[request.RequestedController];

            if (controller.MethodMap.ContainsKey(request.HttpMethod))
                return controller.MethodMap[request.HttpMethod].DynamicInvoke(request) as HttpResponse;

            return HttpStatus.NotImplemented("Unknown HTTP method: " + request.HttpMethod);
        }
    }
}
