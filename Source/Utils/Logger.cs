using System;
using System.Collections.Generic;
using static War3Api.Common;

namespace Source.Utils
{
    public static class Logger
    {
        public static void Debug(string className, string message)
        {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0.25f, String.Format("[|c000000ffDEBUG|r - {0}]: {1}", className, message));
        }

        public static void Verbose(string className, string message) {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0.25f, String.Format("[|c00ffff00VERBOSE|r - {0}]: {1}", className, message));
        }

        public static void Error(string className, string message)
        {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0.25f, String.Format("[|c00ff0000ERROR|r - {0}]: {1}", className, message));
        }

        public static void Warning(string className, string message)
        {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0.25f, String.Format("[|c00ffff00WARN|r - {0}]: {1}", className, message));
        }

        public static void Log(string className, string message)
        {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0.25f, String.Format("[{0}]: {1}", className, message));
        }
    }
}
