using System.Collections.Generic;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace HarmonyMod.Content.Clusters.MidnightSwamp;

public class MidnightSwampSystem : ModSystem
{
    public static bool DisableAllSwampStuffForDebug = false;
    public Point swampPos; 
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        if (MidnightSwampSystem.DisableAllSwampStuffForDebug) return;

        WorldHelper.AddTask(tasks, new SwampGen("Making swamp", 1));
    }

    public override void LoadWorldData(TagCompound tag)
    {
        swampPos = new Point(-1, -1);
        if (tag.ContainsKey("HarmonyMod:swampPos"))
        {
            swampPos = tag.Get<Point>("HarmonyMod:swampPos");
        }

    }

    public override void SaveWorldData(TagCompound tag)
    {
        tag.Add("HarmonyMod:swampPos", swampPos);
    }
}