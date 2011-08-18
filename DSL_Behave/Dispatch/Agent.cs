using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Dispatch
{
    public class Agent
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public AgentType AgentType { get; set; }
        public string Url { get; set; }
        public DateTime LastKeepAlive { get; set; }

        public Agent()
        {
            Id = Guid.NewGuid();
        }

        public string Launch()
        {
            //Return url of launched server.
            return "";
        }

        public string Status()
        {
            //Return status of agent;
            return "";
        }

        public void Close()
        {

        }
    }
}
