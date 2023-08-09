using War3Api;

/*
 * Ozzzzymaniac Custom Save/Load
 *
 * This is the core function of the maps
 */

namespace Source.GameSystem.Save
{
    public class HeroBank : Saveable
    {
        internal HeroBank(Common.player player, int slot) : base(player, slot)
        {
        }
    }
}
