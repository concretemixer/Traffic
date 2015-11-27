using UnityEngine;

namespace Commons.Utils
{
    public class Logger
    {
        static Logger instance;
        static Logger()
        {
            instance = new Logger();
        }

        public static void Log(params string[] _messages)
        {
            instance.Print(_messages);
        }

        void Print(string[] messages)
        {
            var output = string.Join(" ", messages);
            Debug.Log(output);
        }
    }
}