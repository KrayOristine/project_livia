using War3Api;

/*
 * Ozzzzymaniac Custom Save/Load
 *
 * This is the core function of the maps
 */

namespace Source.GameSystem.Save
{
    public abstract class Saveable
    {
        protected readonly Common.player boundedPlayer;
        protected readonly int boundedSlot;

        protected readonly string hash;

        protected Saveable(Common.player boundPlayer, int boundSlot)
        {
            boundedPlayer = boundPlayer;
            boundedSlot = boundSlot;
            hash = Checksum.Serialize(Common.GetPlayerName(boundPlayer) + boundSlot.ToString());
        }
    }
}
