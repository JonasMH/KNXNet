using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KNXNet
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
