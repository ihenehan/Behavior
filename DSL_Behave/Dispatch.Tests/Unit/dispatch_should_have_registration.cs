using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Dispatch.Tests;
using Behavior.Dispatch;

namespace Behavior.Dispatch.Tests.Unit
{
    [TestFixture]
    public class dispatch_should_have_agent_registration
    {
        private AgentRegistrar agentRegistrar;
        private Agent agent;
        private Dispatch dispatch;

        [SetUp]
        public void Setup()
        {
            dispatch = new Dispatch();
            agentRegistrar = new AgentRegistrar();
            agent = new Agent();
            agentRegistrar.RegisterAgent(agent);
        }

        [Test]
        public void dispatch_should_have_registrar()
        {
            dispatch.AgentRegistrar.ShouldNotBe(null);
        }

        [Test]
        public void should_have_list_of_agents()
        {
            agentRegistrar.Agents.ShouldNotBe(null);
        }

        [Test]
        public void agent_should_have_id()
        {
            agent.Id.ShouldNotBe(null);
        }

        [Test]
        public void should_add_agent()
        {
            agentRegistrar.Agents.Count.ShouldBe(1);
        }

        [Test]
        public void should_remove_agent()
        {
            agentRegistrar.RemoveAgent(agent.Id);

            agentRegistrar.Agents.Count.ShouldBe(0);
        }

        [Test]
        public void should_set_agent_active_upon_registration()
        {
            agent.IsActive.ShouldBe(true);
        }
    }
}
