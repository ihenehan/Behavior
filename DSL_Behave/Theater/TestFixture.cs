using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using CookComputing.XmlRpc;
using NUnit.Framework;
using Behavior.Remote;
using Behavior.Remote.Server;
using Behavior.Remote.Results;
using Behavior.Remote.Attributes;

namespace Theater
{
    public class TestFixture : RemoteServer, IRemoteServer
    {
        public override void Dispose()
        {

        }
    }
}
