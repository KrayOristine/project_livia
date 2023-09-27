using System;
using System.IO;
using War3Net.Build;
using War3Net.Build.Info;

namespace Launcher.MapObject
{
    public static class ObjectEditing
    {
        public static void Run(ref Map map)
        {
            if (map == null) throw new ArgumentNullException("Map is null!!!! what the fuck happened");

            InfoEditing.Run(ref map);
        }
    }
}
