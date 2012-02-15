using System;
using System.Net;
using System.Collections.Generic;

namespace HttpServer
{
    public interface IController
    {
        string Name { get; }
        Dictionary<string, Delegate> MethodMap { get; }

        HttpResponse Delete(HttpRequest request);
        HttpResponse Get(HttpRequest request);
        HttpResponse Post(HttpRequest request);
        HttpResponse Put(HttpRequest request);
    }
}
