using System;
using NUnitLite;
using System.Reflection;

namespace KNXNet.Test.Unit.CMD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new AutoRun().Execute(typeof(Program).GetTypeInfo().Assembly, Console.Out, Console.In, args);
        }
    }
}
