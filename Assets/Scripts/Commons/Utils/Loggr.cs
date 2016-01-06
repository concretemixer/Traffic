using UnityEngine;

namespace Commons.Utils
{
    public class Loggr
    {
        static Loggr instance;
        static Loggr()
        {
            instance = new Loggr();
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