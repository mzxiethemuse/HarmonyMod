using HarmonyMod.Core.Util;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.MidnightSwamp;

public class MidnightSwampSystem : ModSystem
{
    public override void SetStaticDefaults()
    {
        WorldGenSystem.AddTask(new SwampGen("Making swampy", 100));
    }
}