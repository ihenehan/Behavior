using WatiN.Core;

namespace WatinExtensions
{
    public static class SelectListExtensions
    {
        public static void SelectByValueAndFireEvents(this SelectList selectList, string value)
        {
            selectList.SelectByValue(value);
            JavaScripty.FireEvents(selectList);
        }

        public static void SelectAndFireEvents(this SelectList selectList, string value)
        {
            selectList.Select(value);
            JavaScripty.FireEvents(selectList);
        }
    }
}