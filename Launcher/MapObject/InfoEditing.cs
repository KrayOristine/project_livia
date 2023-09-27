using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War3Net.Build;
using War3Net.Build.Info;

namespace Launcher.MapObject
{
    public static class InfoEditing
    {
        public const float PLAYER_START_X = 0;
        public const float PLAYER_START_Y = 0;

        public const float NPC_START_X = 0;
        public const float NPC_START_Y = 0;

        public const int MAP_LIVIA_VERSION = 1; // INCREMENT by 1 every update!

        public static void Run(ref Map map)
        {
            map.Info.GameDataSet = GameDataSet.Melee;
            map.Info.GameDataVersion = GameDataVersion.TFT;
            map.Info.MapVersion = 1;
            map.Info.EditorVersion = EditorVersion.v6115;
            // should not modify this
            //map.Info.GameVersion = new Version(1, 36, 0, 20257)
            map.Info.MapName = "World of Livia";
            map.Info.MapDescription = "Too lazy to describe, play and check it out yourself";
            map.Info.MapAuthor = "Ozzzzymaniac";
            map.Info.MapFlags = MapFlags.RequiresExpansion |
                                MapFlags.UseCustomAbilities |
                                MapFlags.UseCustomUpgrades |
                                MapFlags.UseCustomForces |
                                MapFlags.UseCustomTechtree |
                                MapFlags.ShowWaterWavesOnCliffShores |
                                MapFlags.HasTerrainFog |
                                MapFlags.UseItemClassificationSystem |
                                MapFlags.CustomAbilitySkin |
                                MapFlags.HasMapPropertiesMenuBeenOpened |
                                MapFlags.FixedPlayerSettingsForCustomForces;
            map.Info.LoadingScreenBackgroundNumber = -1;
            map.Info.LoadingScreenPath = "loadingscreen.dds";
            map.Info.LoadingScreenTitle = "Ozzzzymaniac present";
            map.Info.LoadingScreenSubtitle = "World of Livia";
            map.Info.LoadingScreenText = "A RPG map just like every other. For more|ninformation please visit our discord server|nInvite Code: 77tjkREYyp";
            map.Info.SupportedModes = SupportedModes.SD;
            map.Info.RecommendedPlayers = "1-5";
            map.Info.ScriptLanguage = ScriptLanguage.Lua;
            CreateMapPlayer(ref map);
            CreateMapForce(ref map);
        }

        public static void CreateMapForce(ref Map map)
        {

            var fPlayer = new ForceData
            {
                Name = "Livia Saviour",
                Flags = ForceFlags.Allied | ForceFlags.AlliedVictory,
                Players = new(0b1111) // p1 - 4
            };

            var fNpc = new ForceData
            {
                Name = "Livia Alliance",
                Flags = ForceFlags.Allied | ForceFlags.ShareVision | ForceFlags.AlliedVictory | ForceFlags.ShareAdvancedUnitControl,
                Players = new(0b111_0000) // p5 - 7
            };

            var fEnemy = new ForceData
            {
                Name = "The Villain",
                Flags = ForceFlags.Allied | ForceFlags.ShareVision | ForceFlags.AlliedVictory | ForceFlags.ShareAdvancedUnitControl,
                Players = new(0b11_000_0000) // p8-9
            };

            var fNeutral = new ForceData
            {
                Name = "Livia Nature",
                Flags = ForceFlags.Allied | ForceFlags.ShareVision | ForceFlags.AlliedVictory | ForceFlags.ShareAdvancedUnitControl,
                Players = new(0b11_00_000_0000) // p10-11
            };

            var fGod = new ForceData
            {
                Name = "God",
                Flags = ForceFlags.Allied | ForceFlags.ShareVision | ForceFlags.AlliedVictory | ForceFlags.ShareAdvancedUnitControl,
                Players = new(1 << 12) // p12
            };

            map.Info.Forces[0] = fPlayer;
            map.Info.Forces[1] = fNpc;
            map.Info.Forces[2] = fEnemy;
            map.Info.Forces[3] = fNeutral;
            map.Info.Forces[4] = fGod;

        }

        public static void CreateMapPlayer(ref Map map)
        {
            map.Info.Players[0].Controller = PlayerController.User;
            map.Info.Players[0].Race = PlayerRace.Human;
            map.Info.Players[0].Name = $"Fantasy Player 1";
            map.Info.Players[0].StartPosition = new(PLAYER_START_X, PLAYER_START_Y);
            map.Info.Players[0].Flags = PlayerFlags.FixedStartPosition;
            map.Info.Players[0].AllyHighPriorityFlags = new(0);
            map.Info.Players[0].AllyLowPriorityFlags = new(0);
            map.Info.Players[0].EnemyHighPriorityFlags = new(0);
            map.Info.Players[0].EnemyLowPriorityFlags = new(0);

            for (int i = 1; i < 5; i++)
            {
                var player = new PlayerData(i)
                {
                    Controller = PlayerController.User,
                    Race = PlayerRace.Human,
                    Name = $"Fantasy Player {i + 1}",
                    StartPosition = new(PLAYER_START_X, PLAYER_START_Y),
                    Flags = PlayerFlags.FixedStartPosition,
                    AllyHighPriorityFlags = new(0),
                    AllyLowPriorityFlags = new(0),
                    EnemyHighPriorityFlags = new(0),
                    EnemyLowPriorityFlags = new(0)
                };

                map.Info.Players.Add(player);
            }

            var npc = new PlayerData(5)
            {
                Controller = PlayerController.Computer,
                Race = PlayerRace.Human,
                Name = "Livia Citizen",
                StartPosition = new(NPC_START_X, NPC_START_Y),
                Flags = PlayerFlags.FixedStartPosition,
                AllyHighPriorityFlags = new(0),
                AllyLowPriorityFlags = new(0),
                EnemyHighPriorityFlags = new(0),
                EnemyLowPriorityFlags = new(0)
            };
            var npc2 = new PlayerData(6)
            {
                Controller = PlayerController.Computer,
                Race = PlayerRace.Human,
                Name = "Livia Noble",
                StartPosition = new(NPC_START_X, NPC_START_Y),
                Flags = PlayerFlags.FixedStartPosition,
                AllyHighPriorityFlags = new(0),
                AllyLowPriorityFlags = new(0),
                EnemyHighPriorityFlags = new(0),
                EnemyLowPriorityFlags = new(0)
            };
            var npc3 = new PlayerData(7)
            {
                Controller = PlayerController.Computer,
                Race = PlayerRace.Human,
                Name = "Livia Army",
                StartPosition = new(NPC_START_X, NPC_START_Y),
                Flags = PlayerFlags.FixedStartPosition,
                AllyHighPriorityFlags = new(0),
                AllyLowPriorityFlags = new(0),
                EnemyHighPriorityFlags = new(0),
                EnemyLowPriorityFlags = new(0)
            };

            var enemy = new PlayerData(8)
            {
                Controller = PlayerController.Computer,
                Race = PlayerRace.Human,
                Name = "Villain",
                StartPosition = new(NPC_START_X, NPC_START_Y),
                Flags = PlayerFlags.FixedStartPosition,
                AllyHighPriorityFlags = new(0),
                AllyLowPriorityFlags = new(0),
                EnemyHighPriorityFlags = new(0),
                EnemyLowPriorityFlags = new(0)
            };
            var enemy2 = new PlayerData(9)
            {
                Controller = PlayerController.Computer,
                Race = PlayerRace.Human,
                Name = "Cosmic Villain",
                StartPosition = new(NPC_START_X, NPC_START_Y),
                Flags = PlayerFlags.FixedStartPosition,
                AllyHighPriorityFlags = new(0),
                AllyLowPriorityFlags = new(0),
                EnemyHighPriorityFlags = new(0),
                EnemyLowPriorityFlags = new(0)
            };

            var neutral = new PlayerData(10)
            {
                Controller = PlayerController.Neutral,
                Race = PlayerRace.Human,
                Name = "Livia Nature",
                StartPosition = new(NPC_START_X, NPC_START_Y),
                Flags = PlayerFlags.FixedStartPosition,
                AllyHighPriorityFlags = new(0),
                AllyLowPriorityFlags = new(0),
                EnemyHighPriorityFlags = new(0),
                EnemyLowPriorityFlags = new(0)
            };
            var neutral2 = new PlayerData(11)
            {
                Controller = PlayerController.Neutral,
                Race = PlayerRace.Human,
                Name = "Livia Creature",
                StartPosition = new(NPC_START_X, NPC_START_Y),
                Flags = PlayerFlags.FixedStartPosition,
                AllyHighPriorityFlags = new(0),
                AllyLowPriorityFlags = new(0),
                EnemyHighPriorityFlags = new(0),
                EnemyLowPriorityFlags = new(0)
            };
            var neutral3 = new PlayerData(12)
            {
                Controller = PlayerController.Neutral,
                Race = PlayerRace.Human,
                Name = "Gods",
                StartPosition = new(NPC_START_X, NPC_START_Y),
                Flags = PlayerFlags.FixedStartPosition,
                AllyHighPriorityFlags = new(0),
                AllyLowPriorityFlags = new(0),
                EnemyHighPriorityFlags = new(0),
                EnemyLowPriorityFlags = new(0)
            };

            map.Info.Players.Add(npc);
            map.Info.Players.Add(npc2);
            map.Info.Players.Add(npc3);
            map.Info.Players.Add(enemy);
            map.Info.Players.Add(enemy2);
            map.Info.Players.Add(neutral);
            map.Info.Players.Add(neutral2);
            map.Info.Players.Add(neutral3);
        }
    }
}
