﻿using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class Log
    {
        public enum LogType { E, I, W, D }
        public static void I(string message,Object instance)
        {
            LogManager.GetLogger(instance!=null?instance.GetType():typeof(Object)).Info(message); 

        }
        public static void E(string message, Object instance)
        {
            LogManager.GetLogger(instance != null ? instance.GetType() : typeof(Object)).Error(message); 
        }
        public static void D(string message, Object instance)
        {
                LogManager.GetLogger(instance!=null?instance.GetType():typeof(Object)).Debug(message); 
        }
        public static void W(string message, Object instance)
        {
            LogManager.GetLogger(instance != null ? instance.GetType() : typeof(Object)).Warn(message); 
        }
    }
}