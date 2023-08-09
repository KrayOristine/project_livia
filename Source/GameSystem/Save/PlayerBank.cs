using Source.Shared;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using War3Api;

/*
 * Ozzzzymaniac Custom Save/Load
 *
 * This is the core function of the maps
 */

namespace Source.GameSystem.Save
{
    public sealed class PlayerBank
    {
        public string KEY;
        public int HASH;
        public List<HeroBank> saveSlots = new();
        private readonly Common.player boundedPlayer;
        private static readonly List<PlayerBank> banks = new();

        public static PlayerBank GetPlayerBank(int index)
        {
            if (index < 0 || index > banks.Count) Logger.Error("PlayerBank", "Illegal Access to player data banks!");
            return banks[index];
        }


        /// <summary>
        /// Create a new bank for player, this is a core bank that contain shared data between hero like achievement, donation skins, etc.
        /// </summary>
        /// <param name="player">A player to bound this bank</param>
        /// <param name="key">A player key to encrypt/decrypt the hero bank - empty for performance if needed</param>
        /// <param name="hash">A unique hash of the player to encode/decode the hero bank</param>
        public PlayerBank(Common.player player, string key, int hash)
        {
            boundedPlayer = player;
            KEY = key;
            HASH = hash;
            banks[Common.GetPlayerId(player)] = this;
        }

        /// <summary>
        /// Create a new slot for the save
        /// just be careful as too many slot will cause the loading of this meta data became unstable
        /// </summary>
        /// <returns>A new bank for you to modify</returns>
        public HeroBank CreateNewSlot()
        {
            var bank = new HeroBank(boundedPlayer, saveSlots.Count);
            saveSlots.Add(bank);

            return bank;
        }

        /// <summary>
        /// Get the save bank from specific slots
        ///
        /// If the slot is exceed current maximum slot, return the last slot
        /// If the slot is less than 0 return the first slot
        /// </summary>
        /// <param name="num">Slot index</param>
        /// <returns>A hero bank on given index</returns>
        public HeroBank GetSaveSlot(int num)
        {
            if (num > saveSlots.Count) return saveSlots[^1];
            if (num < 0) return saveSlots[0];

            return saveSlots[num];
        }
    }
}
