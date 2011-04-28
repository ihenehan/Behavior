using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Remote.Results;

namespace Behavior.Remote.Client
{
    public interface ILauncherClient
    {
        Result RequestFixtureLaunch(string url, int timeoutSeconds);
        Result GetFixtureStatus(string url);
        Result StartFixtureServer(string url);
        Result StopFixtureServer(string url);
    }
}
