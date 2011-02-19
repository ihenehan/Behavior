using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FixtureLauncher
{
    public class ProcessMonitor
    {
        public static int TimeoutCounter { get; set; }

        public void Monitor()
        {
            try
            {
                while (FixtureLauncher.fixtureProc == null)
                {
                    Console.WriteLine("ProcessMonitor.Monitor: Waiting for fixture process to start.");
                    Thread.Sleep(5000);
                }

                int timeout = 600000; //default timeout of 10 minutes.

                int interval = 5000; //Poll every 5 seconds.

                TimeoutCounter = 0;

                while (FixtureLauncher.fixtureProc != null)
                {
                    if (!FixtureLauncher.fixtureProc.HasExited)
                    {
                        Thread.Sleep(interval);
                        
                        TimeoutCounter = TimeoutCounter + interval;

                        if (TimeoutCounter >= timeout)
                        {
                            if (FixtureLauncher.fixtureProc != null)
                            {
                                FixtureLauncher.fixtureProc.Kill();
                                
                                FixtureLauncher.fixtureProc = null;
                            }

                            FixtureLauncher.procFinished = "Fail: Process may have stalled and timed out due to inactivity after " + (timeout / 1000).ToString() + " seconds!";
                            
                            return;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("ProcessMonitor.Monitor: + " + e.Message + "\r\n" + e.ToString());
            }
        }
    }
}
