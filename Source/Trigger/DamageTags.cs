using System;
using static War3Api.Common;
using Source.Modules;
using Source.Utils;

namespace Source.Trigger
{
    public static class DamageTags
    {
        private static void onDamage() {
            try
            {
                var d = Damage.Current;
                Logger.Log("Damage Tags", "Damage taken: " + d.Damage + "Source: " + GetUnitName(d.Source));
            } catch (Exception ex)
            {
                Logger.Error("Damage Tags", ex.Message);
            }
        }
        public static void Init()
        {

            var ev = Damage.Register(DamageEvent.DAMAGED, 9999, onDamage);
            if (ev == null)
            {
                throw new Exception("Damage Tags - Unable to register damage event");
            }
        }
    }
}
