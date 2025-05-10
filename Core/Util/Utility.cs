using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace HarmonyMod.Core.Util;

public static class Utility
{
    public static float VaguelyNormalDist(float min, float max)
    {
        return MathHelper.Lerp(min, max, LerpUtils.RectSin(Main.rand.NextFloat()));
    }
}