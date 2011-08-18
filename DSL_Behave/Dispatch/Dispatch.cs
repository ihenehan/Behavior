using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Dispatch
{
    public class Dispatch
    {
        public AgentRegistrar AgentRegistrar { get; set; }
        public Dictionary<Client, Agent> AgentMap { get; set; }

        public Dispatch()
        {
            AgentRegistrar = new AgentRegistrar();
        }

        public void RegisterAgent(Agent agent)
        {
            AgentRegistrar.RegisterAgent(agent);
        }

        public void UpdateAgentRegistration(Agent agent)
        {
            AgentRegistrar.UpdateAgent(agent);
        }

        public void RemoveAgent(Guid agentId)
        {
            AgentRegistrar.RemoveAgent(agentId);
        }

        public string AssignAgent(Client client)
        {
            var agent = AgentRegistrar.FindAvailableAgent(client.AgentType);

            AgentMap.Add(client, agent);

            return agent.Url;
        }

        public void ReleaseAgent(Client client)
        {
            AgentRegistrar.SetAgentInactive(AgentMap[client].Id);

            AgentMap.Remove(client);
        }

        public string RequestLaunch(Client client)
        {
            return AgentMap[client].Launch();
        }

        public string RequestStatus(Client client)
        {
            return AgentMap[client].Status();
        }

        public void RequextClose(Client client)
        {
            AgentMap[client].Close();
        }
    }
}
