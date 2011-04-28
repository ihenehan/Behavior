using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;
using Behavior.Remote.Client;
using Behavior.Remote.Server;
using Behavior.Common.Configuration;
using System.Reflection;

namespace Behavior.Remote.Server
{
    public class RemoteServerProxy : IRemoteClient
    {
        public BehaviorConfiguration Config { get; set; }
        public IRemoteServer RemoteServer { get; set; }

        public RemoteServerProxy(BehaviorConfiguration config)
        {
            Config = config;

            RemoteServer = ResolveFixture(Config.LocalFixtureAssembly, Config.LocalFixtureName);
        }

        public static IRemoteServer ResolveFixture(string assemblyPath, string name)
        {
            var assembly = System.Reflection.Assembly.LoadFrom(assemblyPath);

            var type = assembly.GetType(name);

            var server = type.GetConstructor(new Type[] { }).Invoke(new object[] { }) as RemoteServer;

            server.StoryContext = new Dictionary<string, object>();

            server.ScenarioContext = new Dictionary<string, object>();

            return server as IRemoteServer;
        }

        public Results.Result Echo(string message)
        {
            return RemoteServer.Echo(message);
        }

        public object[] get_keyword_arguments(string name)
        {
            return RemoteServer.get_keyword_arguments(name);
        }

        public object[] get_parameter_names(string keywordName)
        {
            return RemoteServer.get_parameter_names(keywordName);
        }

        public string get_keyword_documentation(string name)
        {
            return RemoteServer.get_keyword_documentation(name);
        }

        public object get_method_by_attribute(string name, string attributeType)
        {
            return RemoteServer.get_method_by_attribute(name, attributeType);
        }

        public object[] get_keyword_names()
        {
            return RemoteServer.get_keyword_names();
        }

        public object run_keyword(string name, object[] args)
        {
            return RemoteServer.run_keyword(name, args);
        }



        public System.Security.Cryptography.X509Certificates.X509CertificateCollection ClientCertificates
        {
            get { throw new NotImplementedException(); }
        }

        public string ConnectionGroupName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public System.Net.CookieContainer CookieContainer
        {
            get { throw new NotImplementedException(); }
        }

        public System.Net.ICredentials Credentials
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool EnableCompression
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Expect100Continue
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public System.Net.WebHeaderCollection Headers
        {
            get { throw new NotImplementedException(); }
        }

        public Guid Id
        {
            get { throw new NotImplementedException(); }
        }

        public int Indentation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool KeepAlive
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public XmlRpcNonStandard NonStandard
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool PreAuthenticate
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Version ProtocolVersion
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public System.Net.IWebProxy Proxy
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public event XmlRpcRequestEventHandler RequestEvent;

        public System.Net.CookieCollection ResponseCookies
        {
            get { throw new NotImplementedException(); }
        }

        public event XmlRpcResponseEventHandler ResponseEvent;

        public System.Net.WebHeaderCollection ResponseHeaders
        {
            get { throw new NotImplementedException(); }
        }

        public string[] SystemListMethods()
        {
            throw new NotImplementedException();
        }

        public string SystemMethodHelp(string MethodName)
        {
            throw new NotImplementedException();
        }

        public object[] SystemMethodSignature(string MethodName)
        {
            throw new NotImplementedException();
        }

        public int Timeout
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Url
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool UseEmptyParamsTag
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool UseIndentation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool UseIntTag
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool UseStringTag
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string UserAgent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Encoding XmlEncoding
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string XmlRpcMethod
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
