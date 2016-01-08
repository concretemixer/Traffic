using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Commons.Utils
{
    public class Loggr
    {
        static readonly string COMMON_PREFIX = "Loggr:";
        static readonly string ERROR_PREFIX = "Error:";
        static readonly string MESSAGE_PREFIX = "Message:";
        static readonly string WARNING_PREFIX = "Warning:";
        static Loggr instance;
        static Loggr()
        {
            instance = new Loggr();
        }

        public static void Log(params string[] _messages)
        {
            instance.Print(_messages, MESSAGE_PREFIX);
        }

        public static void Error(params string[] _messages)
        {
            instance.Print(_messages, ERROR_PREFIX);
        }

        int stackLength = 1000;
        Stack<string> _logDump = new Stack<string>();

        string[] addPrefix(string[] _messages, string prefix)
        {
            var msgs = _messages.ToList();
            msgs.Insert(0, prefix);
            msgs.Insert(0, COMMON_PREFIX);
            return msgs.ToArray();
        }

        void Print(string[] _messages, string prefix = "")
        {
            _messages = addPrefix(_messages, prefix);
            var output = string.Join(" ", _messages);

            addToDump(output);
            Debug.Log(output);
        }

        void addToDump(string _message)
        {
            while (_logDump.Count > stackLength)
                _logDump.Pop();

            _logDump.Push(_message);
        }
    }
}