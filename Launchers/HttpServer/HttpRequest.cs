using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace HttpServer
{
    public class HttpRequest
    {
        public string HttpMethod { get; set; }
        public Stream InputStream { get; set; }
        public string RawUrl { get; set; }
        public Encoding ContentEncoding { get; set; }
        public string RequestedController { get; set; }
        public string RequestedResource { get; set; }

        public string Body
        {
            get
            {
                var reader = new StreamReader(InputStream, Encoding.UTF8);
                var body = reader.ReadToEnd();
                reader.Close();
                InputStream.Close();
                return body;
            }
        }

        public HttpRequest() { }

        public HttpRequest(HttpListenerRequest listenerRequest)
        {
            HttpMethod = listenerRequest.HttpMethod.ToLower();
            RawUrl = listenerRequest.RawUrl;
            ContentEncoding = listenerRequest.ContentEncoding;
            InputStream = listenerRequest.InputStream;
            RequestedController = ParseController(RawUrl);
            RequestedResource = ParseResource(RawUrl, RequestedController);
        }

        public string ParseController(string rawUrl)
        {
            return rawUrl.Split('/')[1].ToLower();
        }

        public string ParseResource(string rawUrl, string controller)
        {
            return rawUrl.Remove(0, controller.Count() + 1).TrimStart('/').TrimEnd('/');
        }
    }
}
