// ------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="Kray Oristine">
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.
// </copyright>
// ------------------------------------------------------------------------------
using static War3Api.Common;

namespace Source.Shared
{
    public static class Logger
    {
        public static void Debug(string className, string message)
        {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0.25f, string.Format("[|c000000ffDEBUG|r - {0}]: {1}", className, message));
        }

        public static void Verbose(string className, string message)
        {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0.25f, string.Format("[|c00ffff00VERBOSE|r - {0}]: {1}", className, message));
        }

        public static void Error(string className, string message)
        {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0.25f, string.Format("[|c00ff0000ERROR|r - {0}]: {1}", className, message));
        }

        public static void Warning(string className, string message)
        {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0.25f, string.Format("[|c00ffff00WARN|r - {0}]: {1}", className, message));
        }

        public static void Log(string className, string message)
        {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0.25f, string.Format("[{0}]: {1}", className, message));
        }

        public static void DebugSingle(string message)
        {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0.25f, string.Format("[|c000000ffDEBUG|r]: {0}", message));
        }

        public static void VerboseSingle(string message)
        {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0.25f, string.Format("[|c00ffff00VERBOSE|r]: {0}", message));
        }

        public static void ErrorSingle(string message)
        {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0.25f, string.Format("[|c00ff0000ERROR|r]: {0}", message));
        }

        public static void WarningSingle(string message)
        {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0.25f, string.Format("[|c00ffff00WARN|r]: {0}", message));
        }

        public static void LogSingle(string message)
        {
            DisplayTextToPlayer(GetLocalPlayer(), 0, 0.25f, message);
        }
    }
}
