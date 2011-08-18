using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Behavior.Remote.Results;

namespace Behavior.Remote.Client
{
    public interface IRestClient
    {
        Result RequestFixtureLaunch(string url);
        Result GetRequest(string url);
        Result PostRequest(string url);
        Result DeleteRequest(string url);
    }
}
