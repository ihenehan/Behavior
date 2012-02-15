using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace HttpServer
{
    public class HttpStatus
    {
        public static HttpResponse NotFound(string resource)
        {
            return new HttpResponse()
            {
                StatusDescription = HttpStatusCode.NotFound.ToString(),
                StatusCode = 404,
                Content = resource + " not found."
            };
        }

        public static HttpResponse InternalError(string error)
        {
            return new HttpResponse()
            {
                StatusDescription = HttpStatusCode.InternalServerError.ToString(),
                StatusCode = 500,
                Content = error
            };
        }

        public static HttpResponse NotImplemented(string error)
        {
            return new HttpResponse()
            {
                StatusDescription = HttpStatusCode.NotImplemented.ToString(),
                StatusCode = 501,
                Content = error
            };
        }

        public static HttpResponse Ok(object content)
        {
            return Ok(content, "");
        }

        public static HttpResponse Ok(object content, string contentType)
        {
            return new HttpResponse()
            {
                Content = content,
                ContentType = contentType,
                StatusDescription = HttpStatusCode.OK.ToString(),
                StatusCode = 200
            };
        }
    }
}
