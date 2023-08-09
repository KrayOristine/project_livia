/*
    Copyright (C) 2023  Kray Oristine

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using Source.GameSystem.WCObject;
using Source.Shared;
using Source.MapScript;
using System;
using WCSharp.Events;
using WCSharp.Missiles;
using WCSharp.Sync;
using static Source.Shared.Lua;
using static War3Api.Common;
using Source.GameSystem.Damage;

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
                TryInit(InitModules);
                TryInit(InitShared);
                TryInit(InitLibrary);
                TryInit(InitTrigger);

                Logger.Debug("Initialization", "Init completed");

                // hash test
                Console.WriteLine("Hashing testing in progress, there should be no different hash on the same input and seed");
                var original = "Lorem ipsum dolor sit amet, consectetur adipiscing elit viverra.";
                Lua.Assert(Hash.MurMur.HashV2(original, 6969) == Hash.MurMur.HashV2(original, 6969), "MurMur HashV2 failed, different hash detected");
                Lua.Assert(Hash.MurMur.HashV3(original, 9696) == Hash.MurMur.HashV3(original, 9696), "MurMur HashV3 failed, different hash detected");
                Lua.Assert(Hash.MD5.Hash(original) == Hash.MD5.Hash(original), "MD5 Hash failed, different hash detected");
                Console.WriteLine("Hashing passed, no different hash on same input and seed");


            }
            catch (Exception ex)
            {
                DisplayTextToPlayer(GetLocalPlayer(), 0, 0, ex.Message);
            }
        }
    }
}
