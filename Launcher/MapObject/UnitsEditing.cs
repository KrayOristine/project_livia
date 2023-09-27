using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using War3Net.Build;
using War3Net.Build.Object;
using War3Net.Build.Widget;

namespace Launcher.MapObject
{
    public static class UnitsEditing
    {
        public static void Run(ref Map map)
        {


        }

        /// <summary>
        /// Abstraction for creating a unit, you dont need to fuck around with values
        /// </summary>
        /// <param name="baseId"></param>
        /// <param name="newId"></param>
        /// <returns></returns>
        public static SimpleObjectModification CreateUnit(int baseId, int newId)
        {
            var unit = new SimpleObjectModification
            {
                OldId = baseId,
                NewId = newId
            };
            CreateUnitModsField(ref unit);

            return unit;
        }


        public static void CreateUnitModsField(ref SimpleObjectModification unit)
        {

        }
    }
}
