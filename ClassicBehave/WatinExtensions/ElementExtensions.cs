using System.Threading;
using WatiN.Core;

namespace WatinExtensions
{
    public static class ElementExtensions
    {
        public static void Click(this Element e, int delay)
        {
            e.Click();
            Thread.Sleep(delay);
        }
    }
}