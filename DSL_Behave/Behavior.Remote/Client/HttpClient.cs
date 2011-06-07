using System;
using System.Net;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Behavior.Remote.Results;

namespace Behavior.Remote.Client
{
    public class HttpClient
    {
        public Result GetRequest(string url)
        {
            try
            {
                return ReadResponse(CreateRequest("GET", url));
            }

            catch (Exception e)
            {
                return Result.CreateFail(e.ToString());
            }
        }

        public Result PostRequest(string url, string body)
        {
            try
            {
                var httpWebRequest = CreateRequest("POST", url);

                WriteRequestBody(httpWebRequest, body);

                return ReadResponse(httpWebRequest);
            }

            catch (Exception e)
            {
                return Result.CreateFail(e.ToString());
            }
        }

        public Result DeleteRequest(string url)
        {
            try
            {
                return ReadResponse(CreateRequest("DELETE", url));
            }
            
            catch (Exception e)
            {
                return Result.CreateFail(e.ToString());
            }
        }

        public HttpWebRequest CreateRequest(string method, string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.Method = method;

            return httpWebRequest;
        }

        public void WriteRequestBody(HttpWebRequest request, string body)
        {
            var requestStream = request.GetRequestStream();

            var writer = new StreamWriter(requestStream, Encoding.UTF8);

            writer.Write(new object[] { body });

            writer.Flush();

            writer.Close();

            requestStream.Close();
        }

        public Result ReadResponse(HttpWebRequest request)
        {
            var webResponse = request.GetResponse();

            var body = ReadResponseBody(webResponse);

            webResponse.Close();

            return Result.CreatePass(body);
        }

        public string ReadResponseBody(WebResponse response)
        {
            var responseStream = response.GetResponseStream();

            var reader = new StreamReader(responseStream, Encoding.UTF8);

            var body = reader.ReadToEnd();

            reader.Close();

            responseStream.Close();

            return body;
        }
    }
}
