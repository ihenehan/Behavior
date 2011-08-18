using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Behavior.Agent
{
    public class ProcessMonitor
    {
        public static int TimeoutCounter { get; set; }

        public void Monitor()
        {
            try
            {
                while (RestAgent.xmlRpcServerProc == null)
                {
                    Console.WriteLine("ProcessMonitor.Monitor: Waiting for fixture process to start.");
                    Thread.Sleep(5000);
                }

                int timeout = 600000; //default timeout of 10 minutes.

                int interval = 5000; //Poll every 5 seconds.

                TimeoutCounter = 0;

                while (RestAgent.xmlRpcServerProc != null)
                {
                    if (!RestAgent.xmlRpcServerProc.HasExited)
                    {
                        Thread.Sleep(interval);
                        
                        TimeoutCounter = TimeoutCounter + interval;

                        if (TimeoutCounter >= timeout)
                        {
                            if (RestAgent.xmlRpcServerProc != null)
                            {
                                RestAgent.xmlRpcServerProc.Kill();
                                
                                RestAgent.xmlRpcServerProc = null;
                            }

                            RestAgent.procFinished = "Fail: Process may have stalled and timed out due to inactivity after " + (timeout / 1000).ToString() + " seconds!";
                            
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
