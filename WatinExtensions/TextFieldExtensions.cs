using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core;

namespace WatinExtensions
{
    public static class TextFieldExtensions
    {
        public static void TypeTextFast(this TextField textField, string text)
        {
            textField.Value = text;
            textField.FireEvent("onfocusout");
            textField.FireEvent("onblur");
        }

        public static void TypeTextFastAndFireEvents(this TextField textField, string text)
        {
            textField.Value = text;
            JavaScripty.FireEvents(textField);
        }
    }
}
