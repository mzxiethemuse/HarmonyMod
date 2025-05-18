using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace HarmonyMod.Core.Util;

//name is misleading.
public static class MathUtility
{
    public static float VaguelyNormalDist(float min, float max)
    {
        return MathHelper.Lerp(min, max, LerpUtils.RectSin(Main.rand.NextFloat()));
    }


    public static Vector2 Abs(this Vector2 v)
    {
        return new Vector2(Math.Abs(v.X), Math.Abs(v.Y));
    }

    public static float AttenuateKindaMaybeIdkWhatToCallThisFunction(float x, float bound)
    {
        return bound * MathF.Tanh(x * (MathF.PI / bound));
    }
    
}