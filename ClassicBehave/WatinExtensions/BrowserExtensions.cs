using System;
using System.Threading;
using NUnit.Framework;
using WatiN.Core;
using WatiN.Core.Constraints;

namespace WatinExtensions
{
    public static class BrowserExtensions
    {
        private static int timeout = 30000;
        private static int interval = 100;
        private static int counter = 0;

        public static Element FindById(this IE browser, string id)
        {
            return FindById<Element>(browser, id, 0);
        }

        public static T FindById<T>(this IE browser, string id) where T : Element
        {
            return FindById<T>(browser, id, 0);
        }

        public static T FindById<T>(this IE browser, string idFormat, params object[] args) where T : Element
        {
            AttributeConstraint constraint = Find.ById(string.Format(idFormat, args));
            return browser.FindInContext<T>(constraint);
        }

        public static T FindByClass<T>(this IE browser, string className) where T : Element
        {
            return FindByClass<T>(browser, className, e => true);
        }

        public static T FindByClass<T>(this IE browser, string className, Predicate<T> predicate) where T : Element
        {
            return FindAllByClass<T>(browser, className, predicate).First();
        }

        public static ElementCollection<T> FindAllByClass<T>(this IE browser, string className)
            where T : Element
        {
            return FindAllByClass<T>(browser, className, e => true);
        }

        public static ElementCollection<T> FindAllByClass<T>(this IE browser, string className, Predicate<T> predicate)
            where T : Element
        {
            browser.WaitForAsyncPostBackToComplete();

            Predicate<T> constraint = e => e.ClassName != null && e.ClassName.Contains(className) && predicate.Invoke(e);

            while (!browser.ElementsOfType<T>().Exists(constraint) && counter < timeout)
            {
                Thread.Sleep(interval);
                counter = counter + interval;
            }

            if (browser.ElementsOfType<T>().Exists(constraint))
                return browser.ElementsOfType<T>().Filter(constraint);

            throw new Exception(string.Format("Could not locate element with class name containing {0}", className));
        }

        public static T FindInContext<T>(this IE browser, Constraint constraint)
            where T : Element
        {
            browser.WaitForAsyncPostBackToComplete();

            while (!browser.ElementsOfType<T>().Exists(constraint) && counter < timeout)
            {
                Thread.Sleep(interval);
                counter = counter + interval;
            }

            if (browser.ElementsOfType<T>().Exists(constraint))
                return browser.ElementOfType<T>(constraint);

            throw new Exception(string.Format("Could not locate {0}", constraint));
        }

        public static void VerifyExistsAndEnabled<T>(this IE browser, string id) where T : Element
        {
            browser.WaitForAsyncPostBackToComplete();

            counter = 0;

            Constraint constraint = Find.ById(id);

            while (!browser.ElementOfType<T>(constraint).Exists && counter < timeout)
            {
                Thread.Sleep(interval);
                counter = counter + interval;
            }

            if (!browser.ElementOfType<T>(constraint).Exists)
                Assert.Fail("Element of type " + typeof(T).FullName + " with ID of " + id + " could not be found!");

            if (!browser.ElementOfType<T>(constraint).Enabled)
                Assert.Fail("Element of type " + typeof(T).FullName + " with ID of " + id + " was found but was not enabled!");
        }

        public static void VerifyExists<T>(this IE browser, string id) where T : Element
        {
            browser.WaitForAsyncPostBackToComplete();

            counter = 0;
            Constraint constraint = Find.ById(id);

            while (!browser.ElementOfType<T>(constraint).Exists && counter < timeout)
            {
                Thread.Sleep(interval);
                counter = counter + interval;
            }

            if (!browser.ElementOfType<T>(constraint).Exists)
                Assert.Fail("Element " + typeof(T).FullName + " with id " + id + " is not displayed!");
        }

        public static void VerifyFormDoesNotExist(this IE browser, string id)
        {
            browser.WaitForAsyncPostBackToComplete();

            counter = 0;
            Constraint constraint = Find.ById(id);

            while (browser.ElementOfType<Form>(constraint).Exists && counter < timeout)
            {
                Thread.Sleep(interval);
                counter = counter + interval;
            }

            if (browser.ElementOfType<Form>(constraint).Exists)
                Assert.Fail("Expected form with Id of " + id + " to not exist!");
        }

        public static void WaitForAsyncPostBackToComplete(this IE browser)
        {
            //bool isInPostback = true;
            //Constraint constraint = Find.ById(Divs.AjaxRunState);

            //while (isInPostback)
            //{
            //    isInPostback = browser.ElementOfType<Div>(constraint).Text.ToLower().Equals("loading");
            //    if (isInPostback)
            //    {
            //        System.Windows.Forms.Application.DoEvents();
            //        Thread.Sleep(200); //sleep for 200ms and query again 
            //    }
            //}
        }
    }
}