using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace SampleFixtureTests.DataProviders
{
    public interface IDataProvider
    {
        List<XmlRpcStruct> Data { get; set; }
    }
}
