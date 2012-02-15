using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;

namespace HttpServer
{
    public class HttpResponse
    {
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public object Content { get; set; }
        public string ContentType { get; set; }

        private Dictionary<string, ImageFormat> imageFormats = new Dictionary<string, ImageFormat>();

        public HttpResponse() 
        {
            imageFormats.Add("image/png", ImageFormat.Png);
            imageFormats.Add("image/jpeg", ImageFormat.Jpeg);
            imageFormats.Add("image/bmp", ImageFormat.Bmp);
        }

        public void Write(HttpListenerContext context, Encoding encoding)
        {
            context = MapResponse(context);

            var writer = new StreamWriter(context.Response.OutputStream, encoding);

            context.Response.ContentType = ContentType;

            if (Content.GetType().Equals(typeof(Bitmap)))
                (Content as Bitmap).Save(context.Response.OutputStream, imageFormats[ContentType]);
            else
                writer.Write(Content);

            writer.Close();

            context.Response.OutputStream.Close();
        }

        public HttpListenerContext MapResponse(HttpListenerContext context)
        {
            context.Response.StatusCode = StatusCode;
            context.Response.StatusDescription = StatusDescription;
            context.Response.Headers.Set(HttpResponseHeader.Server, "SeleniumLauncher");

            return context;
        }
    }
}
