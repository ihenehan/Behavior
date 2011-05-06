using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;
using Behavior.Remote.Results;

namespace Behavior.Remote.Client
{
    [XmlRpcUrl("http://localhost/RemoteServer.rem")]
    public interface IRemoteClient : IXmlRpcProxy
    {
        [XmlRpcMethod("Echo")]
        Result Echo(string message);

        [XmlRpcMethod("get_keyword_arguments")]
        object[] get_keyword_arguments(string name);

        [XmlRpcMethod("get_parameter_names")]
        object[] get_parameter_names(string keywordName);

        [XmlRpcMethod("get_keyword_documentation")]
        string get_keyword_documentation(string name);

        [XmlRpcMethod("get_method_by_attribute")]
        object get_method_by_attribute(string name, string attributeType);

        [XmlRpcMethod("get_keyword_names")]
        object[] get_keyword_names();

        [XmlRpcMethod("run_keyword")]
        object run_keyword(string name, object[] args);

        [XmlRpcMethod("reset_criterion_context")]
        object reset_criterion_context();
    }
}
