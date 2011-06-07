using System;
using System.Net;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Behavior.Remote.Results;

namespace Behavior.Remote.Client
{
    public class LauncherClient : HttpClient, ILauncherClient
    {
        public Result RequestFixtureLaunch(string url)
        {
            var result = new Result();
            
            var fixtureStarted = false;

            while ((!fixtureStarted))
            {
                var statusResult = GetRequest(url);

                var retrn = (string)statusResult.retrn;

                if (!statusResult.status.ToLower().Equals("fail"))
                {
                    if (retrn.Equals("Busy"))
                    {
                        DeleteRequest(url);

                        Application.DoEvents();
                    }
                    else
                    {
                        result = PostRequest(url);

                        if (statusResult.status.Equals("PASS"))
                            fixtureStarted = true;
                    }
                }
                else
                    return result.Fail(statusResult.error);
            }

            if(!fixtureStarted)
                return result.Fail("Failed to start FixtureServer at URL " + url + "!");

            return result;
        }

        public Result PostRequest(string url)
        {
            return PostRequest(url, url);
        }
    }
}
