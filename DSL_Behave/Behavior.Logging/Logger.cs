using System;
using System.IO;

namespace Behavior.Logging
{

    public sealed class Logger
    {
        private static string logFilePath;
        private static LogLevel logLevel;
        private static bool writeToConsole;
        private static readonly Logger instance = new Logger();

        private Logger() { }

        public static Logger Instance
        {
            get
            {
                return instance;
            }
        }

        public string LogFilePath
        {
            get
            {
                return logFilePath;
            }
            set
            {
                if (!object.Equals(logFilePath, value))
                    logFilePath = value;
            }
        }

        public LogLevel Level
        {
            get
            {
                return logLevel;
            }
            set
            {
                if (!object.Equals(logLevel, value))
                    logLevel = value;
            }
        }

        public bool WriteToConsole
        {
            get
            {
                return writeToConsole;
            }
            set
            {
                if (!object.Equals(writeToConsole, value))
                    writeToConsole = value;
            }
        }

        public void Print(LogLevel level, string message)
        {
            if (level >= logLevel)
            {
                DateTime stamp = DateTime.Now;
                StreamWriter logWriter;

                if (!File.Exists(logFilePath))
                    logWriter = File.CreateText(logFilePath);
                else
                    logWriter = File.AppendText(logFilePath);

                string levelText = "";
                if (level.Equals(LogLevel.Debug) || level.Equals(LogLevel.Info))
                    levelText = "[" + level.ToString() + "]\t\t";
                if (level.Equals(LogLevel.Exception))
                    levelText = "[" + level.ToString() + "]\t";

                string logOut = levelText + stamp.ToString("u") + "." + stamp.Millisecond.ToString("000") + " * " + message;
                logWriter.WriteLine(logOut);

                if (writeToConsole)
                {
                    if (level.Equals(LogLevel.Exception))
                    {
                        var color = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(logOut);
                        Console.ForegroundColor = color;
                    }

                    Console.WriteLine(logOut);
                }

                logWriter.Flush();
                logWriter.Close();
            }
        }

        public void DeleteLogFile()
        {
            if(File.Exists(logFilePath))
                File.Delete(logFilePath);
        }
    }
}
