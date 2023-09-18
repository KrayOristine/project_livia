// ------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Kray Oristine">
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
using Source.GameSystem.W3OOP;
using Source.Shared;
using Source.MapScript;
using System;
using WCSharp.Events;
using WCSharp.Missiles;
using WCSharp.Sync;
using static Source.Shared.Lua;
using static War3Api.Common;
using Source.GameSystem.Damage;
using Source.GameSystem.UI;

namespace Source
{
    public static class Program
    {
        public static bool Debug { get; private set; } = false;

        public static void Main()
        {
            // Delay a little since some stuff can break otherwise
            var timer = CreateTimer();
            TimerStart(timer, 0.01f, false, () =>
            {
                DestroyTimer(timer);
                Start();
            });
        }

        private static void TryInit(Action method)
        {
            try
            {
                method.Invoke();
            }
            catch (Exception ex)
            {
                Logger.Error("Initialization", ex.ToString());
            }
        }

        private static void InitShared()
        {
            MissileSystem.RegisterForOwnershipChanges();
        }

        private static void InitModules()
        {
            Engine.InitEngine();
        }

        private static void InitLibrary()
        {
            Mouse.Init();
        }

        private static void InitTrigger()
        {
            DamageTag.Init();
        }

        private static void InitFrame()
        {
            Frame.HideOriginFrames(true);
            Frame.GetByName("ConsoleUIBackdrop", 0).SetSize(0, 0.0001f);

            SaveSlot.Init();
        }

        private static void Start()
        {
            try
            {
#if DEBUG
                // This part of the code will only run if the map is compiled in Debug mode
                Debug = true;
                Console.WriteLine("This map is in debug mode. The map may not function as expected.");
                // By calling these methods, whenever these systems call external code (i.e. your code),
                // they will wrap the call in a try-catch and output any errors to the chat for easier debugging
                PeriodicEvents.EnableDebug();
                PlayerUnitEvents.EnableDebug();
                SyncSystem.EnableDebug();

                CreateUnit(Player(0), FourCC("hfoo"), 0, 0, 0);
                CreateUnit(Player(0), FourCC("hfoo"), 0, 0, 0);
#endif
                DisplayTimedTextToPlayer(GetLocalPlayer(), 0, 0, 100, "|c00ff0000WARNING|r: Map initialization phase begin, HUGE LAGE INCOMING");

                // Disable player mouse and changing shit
                ShowInterface(false, 0);
                EnableUserControl(false);
                BlzEnableCursor(false);
                EnableOcclusion(false);

                TryInit(InitModules);
                TryInit(InitShared);
                TryInit(InitLibrary);
                TryInit(InitTrigger);
                /// Frame initialization must always after everything
                TryInit(InitFrame);

                ShowInterface(true, 0);
                EnableUserControl(true);
                BlzEnableCursor(true);
                EnableOcclusion(true);


                Logger.Debug("Initialization", "Init completed");


            }
            catch (Exception ex)
            {
                DisplayTextToPlayer(GetLocalPlayer(), 0, 0, ex.ToString());
            }
        }
    }
}
