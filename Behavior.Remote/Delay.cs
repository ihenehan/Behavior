using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Behavior.Remote
{
    public class Delay
    {
        private static Timer timer = new Timer();
        private static bool timedOut = false;

        public static void delay(int milliseconds)
        {
            timedOut = false;
            timer.Tick += new EventHandler(TimerEventProcessor);
            timer.Interval = milliseconds;
            timer.Start();

            while (timedOut == false)
            {
                Application.DoEvents();
            }
        }

        private static void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            timer.Stop();
            timedOut = true;
        }
    }
}
