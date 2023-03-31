using Source.Shared;
using Source.Trigger;
using System;
using WCSharp.Events;
using WCSharp.Sync;
using static War3Api.Common;

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
                try
                {
                    // Initialize core property (because they will be shared for other scripts and they don't require any other than themselves)

                }
                catch (Exception ex)
                {
                    Logger.Error("Init Engine", ex.Message);
                }

                try
                {
                    // Modules Init (these will be used though-out other scripts)

                    //Damage.Init()
                }
                catch (Exception ex)
                {
                    Logger.Error("Init Engine", ex.Message);
                }

                try
                {
                    // Library Init (these will be used for trigger like line-segment, etc.
                }
                catch (Exception ex)
                {
                    Logger.Error("Init Engine", ex.Message);
                }

                try
                {
                    // Trigger Init

                    DamageTags.Init();
                }
                catch (Exception ex)
                {
                    Logger.Error("Init Engine", ex.Message);
                }

                Logger.Debug("Init Engine", "Init completed");
            }
            catch (Exception ex)
            {
                DisplayTextToPlayer(GetLocalPlayer(), 0, 0, ex.Message);
            }
        }
    }
}
