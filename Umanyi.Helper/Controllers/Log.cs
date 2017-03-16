using log4net;
using System;
using System.Collections.Immutable;

namespace UmanyiSMS.Lib.Controllers
{
    public static class Log
    {
        private static IImmutableList<string> entries;
        
        public enum LogType { E, I, W, D }

        public static void Init(ref IImmutableList<string> log)
        {
            entries = log;
        }
        public static void I(string message,Object instance)
        {
            entries.Add(message);
            LogManager.GetLogger(instance!=null?instance.GetType():typeof(Object)).Info(message); 

        }
        public static void E(string message, Object instance)
        {
            entries.Add(message);
            LogManager.GetLogger(instance != null ? instance.GetType() : typeof(Object)).Error(message); 
        }
        public static void D(string message, Object instance)
        {
            entries.Add(message);
            LogManager.GetLogger(instance!=null?instance.GetType():typeof(Object)).Debug(message); 
        }
        public static void W(string message, Object instance)
        {
            entries.Add(message);
            LogManager.GetLogger(instance != null ? instance.GetType() : typeof(Object)).Warn(message); 
        }
    }
}
