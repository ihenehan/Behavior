using System;
using Behavior.Remote.Results;

namespace Behavior.Remote.Server
{    
    public interface IRemoteServer : IDisposable
    {        
        Result Echo(string message);

        object[] get_keyword_arguments(string name);

        string get_keyword_documentation(string name);

        object get_method_by_attribute(string name, string attributeType);

        object[] get_keyword_names();

        object[] get_parameter_names(string keywordName);

        object run_keyword(string name, object[] args);
    }
}
