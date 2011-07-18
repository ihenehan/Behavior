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
    [Fixture]
    public class TheaterMock : RemoteServer, IRemoteServer
    {
        [Step("I am a Theater Patron")]
        public void IAmATheaterPatron()
        {
            Assert.IsTrue(true);
        }

        [Step("I have purchased a ticket")]
        public void IHavePurchasedATicket()
        {
            Assert.IsTrue(true);
        }

        [Step("today is seven days from the performance date")]
        public void DateIsSevenDaysFromPerformance()
        {
            Assert.IsTrue(true);
        }

        [Step("I receive a reminder email")]
        public void IReceiveAReminderEmail()
        {
            Assert.IsTrue(true);
        }

        [Step("a performance exists")]
        public void APerformanceExists()
        {
            Assert.IsTrue(true);
        }

        [Step("I supply a price for Adult Tickets")]
        public void ISupplyAnAdultTicketPrice()
        {
            Assert.IsTrue(false);
        }

        [Step("I supply a price for [arg] Tickets")]
        public void ISupplyATicketPrice(string arg)
        {
            Console.WriteLine("Arg = " + arg);
            Assert.IsTrue(true);
        }

        [Step("that price is stored and associated with Adult Tickets.")]
        public void PriceIsStoredAndAssociatedWithAdultTickets()
        {
            Assert.IsTrue(false);
        }

        [Step("that price is stored and associated with [arg] Tickets.")]
        public void PriceIsStoredAndAssociatedWithTickets(string arg)
        {
            Console.WriteLine("Arg = " + arg);
            Assert.IsTrue(true);
        }

        public override void Dispose()
        {

        }
    }
}
