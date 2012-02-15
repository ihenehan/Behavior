using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace HttpServer
{
    public class Controller : IController
    {
        public string Name 
        { 
            get { return this.GetType().Name.Replace("Controller", "").ToLower(); }
        }

        public Dictionary<string, Delegate> MethodMap
        {
            get { return methodMap; }
        }

        private Dictionary<string, Delegate> methodMap = new Dictionary<string, Delegate>();

        public Controller()
        {
            MethodMap.Add("post", new Func<HttpRequest, HttpResponse>(Post));
            MethodMap.Add("put", new Func<HttpRequest, HttpResponse>(Put));
            MethodMap.Add("get", new Func<HttpRequest, HttpResponse>(Get));
            MethodMap.Add("delete", new Func<HttpRequest, HttpResponse>(Delete));
        }

        public virtual HttpResponse Delete(HttpRequest request) { return HttpStatus.NotImplemented(ErrorMsg(request.HttpMethod)); }

        public virtual HttpResponse Get(HttpRequest request) { return HttpStatus.NotImplemented(ErrorMsg(request.HttpMethod)); }

        public virtual HttpResponse Post(HttpRequest request) { return HttpStatus.NotImplemented(ErrorMsg(request.HttpMethod)); }

        public virtual HttpResponse Put(HttpRequest request) { return HttpStatus.NotImplemented(ErrorMsg(request.HttpMethod)); }


        public string ErrorMsg(string method)
        {
            return this.GetType().Name + "." + method + ": Not Implemented.";
        }
    }
}
