using System;
using System.Net;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Behavior.Remote.Results;

namespace Behavior.Remote.Client
{
    public class LauncherClient : ILauncherClient
    {
        public Result RequestFixtureLaunch(string url, int timeoutSeconds)
        {
            var result = new Result();
            
            var fixtureStarted = false;

            while ((!fixtureStarted) && (timeoutSeconds > 0))
            {
                var statusResult = GetFixtureStatus(url);

                var retrn = (string)statusResult.retrn;

                if (!statusResult.status.ToLower().Equals("fail"))
                {
                    if (retrn.Equals("Busy"))
                    {
                        StopFixtureServer(url);

                        Delay.delay(1000);

                        Application.DoEvents();
                    }
                    else
                    {
                        result = StartFixtureServer(url);

                        if (statusResult.status.Equals("PASS"))
                            fixtureStarted = true;
                    }

                    timeoutSeconds--;
                }
                else
                {
                    return result.Fail(statusResult.error);
                }
            }

            if(!fixtureStarted)
                return result.Fail("Failed to start FixtureServer at URL " + url + "!");

            return result;
        }
        
        public Result GetFixtureStatus(string url)
        {
            Console.WriteLine("LauncherClient.GetFixtureStatus URL = " + url);

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                httpWebRequest.Method = "GET";

                var webResponse = httpWebRequest.GetResponse();

                var responseStream = webResponse.GetResponseStream();

                var reader = new StreamReader(responseStream, Encoding.UTF8);

                var s = reader.ReadToEnd();

                reader.Close();

                responseStream.Close();

                webResponse.Close();

                return Result.CreatePass(s);
            }

            catch (Exception e)
            {
                return Result.CreateFail(e.ToString());
            }
        }

        public Result StartFixtureServer(string url)
        {
            Console.WriteLine("LauncherClient.StartFixtureServer URL = " + url);

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                httpWebRequest.Method = "POST";

                var requestStream = httpWebRequest.GetRequestStream();

                var writer = new StreamWriter(requestStream, Encoding.UTF8);

                writer.Write(new object[] { url });

                writer.Flush();

                writer.Close();

                requestStream.Close();

                var webResponse = httpWebRequest.GetResponse();

                var responseStream = webResponse.GetResponseStream();

                var reader = new StreamReader(responseStream, Encoding.UTF8);

                var s = reader.ReadToEnd();

                reader.Close();

                responseStream.Close();

                webResponse.Close();

                return Result.CreatePass(s);
            }

            catch (Exception e)
            {
                return Result.CreateFail(e.ToString());
            }
        }

        public Result StopFixtureServer(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.Method = "DELETE";

            var webResponse = httpWebRequest.GetResponse();
            
            var responseStream = webResponse.GetResponseStream();
            
            var reader = new StreamReader(responseStream, Encoding.UTF8);

            var s = reader.ReadToEnd();

            Console.WriteLine("LauncherClient.StopFixtureServer response: " + s);

            reader.Close();
            
            responseStream.Close();
            
            webResponse.Close();

            return Result.CreatePass(s);
        }
    }
}
