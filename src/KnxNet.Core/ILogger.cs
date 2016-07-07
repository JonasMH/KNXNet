using System;

namespace KnxNet.Core
{
    public enum LogType
    {
        Info,
        Warn,
        Errr
    }
    
    public interface ILogger
    {
        void WriteLine(string line, LogType type = LogType.Info);
    }

    public class ConsoleLogger : ILogger
    {
        public void WriteLine(string line, LogType type = LogType.Info)
        {
            Console.WriteLine(type + ":" + line);
        }
    }
}
