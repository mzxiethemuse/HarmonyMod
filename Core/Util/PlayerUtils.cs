using Terraria;

namespace HarmonyMod.Core.Util;

public static class PlayerUtils
{
    // public static WarriorParry ParryPlayer(this Player p)
    // {
    //     return p.GetModPlayer<WarriorParry>();
    // }

    public static HarmonyPlayer HarmonyPlayer(this Player p)
    {
        return p.GetModPlayer<HarmonyPlayer>();
    }
}