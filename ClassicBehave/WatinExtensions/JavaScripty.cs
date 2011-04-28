using WatiN.Core;

namespace WatinExtensions
{
    public static class JavaScripty
    {
        public static void FireEvents(Element element)
        {
            element.FireEvent("onfocusout");
            element.FireEvent("onblur");
            element.FireEvent("onchange");
        }
    }
}