using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Timers;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Lib.Controllers
{
    public sealed class Log : IDisposable
    {
        int lastLogIndex;

        System.Timers.Timer t;

        static Log _instance;

        private IImmutableList<string> entries;

        private List<BasicPair<LogType, string>> _entries2;

        private enum LogType { E, I, W, D }

        private Log()
        {
            entries = ImmutableList.Create<string>();
            _entries2 = new List<BasicPair<LogType, string>>();
            t = new System.Timers.Timer(1000);
            t.Elapsed += Timer_Callback;
        }

        public static void Init(ref IImmutableList<string> log)
        {
            _instance = new Log();

            _instance.entries = log;
            _instance.t.Start();
        }

        private void Timer_Callback(object sender, ElapsedEventArgs e)
        {
            WriteToFile(_entries2);
        }

        private void WriteToFile(List<BasicPair<LogType, string>> messages)
        {
            ThreadPool.QueueUserWorkItem(WriteFileCallBack, messages);
        }

        private void WriteFileCallBack(object state)
        {
            var msgs = state as List<BasicPair<LogType, string>>;
            for (int i = lastLogIndex; i < msgs.Count; i++)
            {
                switch (msgs[i].Key)
                {
                    case LogType.I: LogManager.GetLogger(typeof(Object)).Info(msgs[i].Value); break;
                    case LogType.E: LogManager.GetLogger(typeof(Object)).Error(msgs[i].Value); break;
                    case LogType.W: LogManager.GetLogger(typeof(Object)).Warn(msgs[i].Value); break;
                    case LogType.D: LogManager.GetLogger(typeof(Object)).Debug(msgs[i].Value); break;
                }
            }
            lastLogIndex = msgs.Count;
        }

        public static void I(string message, Object instance)
        {

            _instance._entries2.Add(new BasicPair<LogType, string>(LogType.I, message));
            _instance.entries.Add(message);
        }

        public static void E(string message, Object instance)
        {
            _instance._entries2.Add(new BasicPair<LogType, string>(LogType.E, message));
            _instance.entries.Add(message);
        }
        public static void D(string message, Object instance)
        {
            _instance._entries2.Add(new BasicPair<LogType, string>(LogType.D, message));
            _instance.entries.Add(message);
        }
        public static void W(string message, Object instance)
        {
            _instance._entries2.Add(new BasicPair<LogType, string>(LogType.W, message));
            _instance.entries.Add(message);
        }

        public void Dispose()
        {
            t.Stop();
            t.Dispose();
        }
    }
}
