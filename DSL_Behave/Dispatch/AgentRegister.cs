using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Dispatch
{
    public class AgentRegistrar
    {
        public List<Agent> Agents;

        public AgentRegistrar()
        {
            Agents = new List<Agent>();
        }

        public void RegisterAgent(Agent agent)
        {
            agent.IsActive = true;

            agent.LastKeepAlive = DateTime.Now;

            Agents.Add(agent);
        }

        public void UpdateAgent(Agent currentAgent)
        {
            var oldAgent = Agents.FirstOrDefault(a => a.Url.Equals(currentAgent.Url));

            if (oldAgent != null)
                oldAgent.LastKeepAlive = DateTime.Now;
            else
                RegisterAgent(currentAgent);
        }

        public void RemoveAgent(Guid agentId)
        {
            Agents.Remove(Agents.First(a => a.Id.Equals(agentId)));
        }

        public void SetAgentActive(Guid agentId)
        {
            var agent = Agents.First(a => a.Id.Equals(agentId));

            agent.IsActive = true;
        }

        public void SetAgentInactive(Guid agentId)
        {
            var agent = Agents.First(a => a.Id.Equals(agentId));

            agent.IsActive = false;
        }

        public Agent FindAvailableAgent(AgentType agentType)
        {
            var agent = Agents.FirstOrDefault(a => a.AgentType.Name.Equals(agentType.Name) && a.IsActive.Equals(false));

            agent.IsActive = true;

            return agent;
        }
    }
}
