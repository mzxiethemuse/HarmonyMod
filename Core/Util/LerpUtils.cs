using System;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace HarmonyMod.Core.Util;

/// <summary>
/// A class with various functions to process lerp values (i.e. values that ramp from 0 -> 1). most functions here are based off of sound synthesis. if you wanna know what they do, honestly just look up "Bitwig Grid phase modules"
/// </summary>
public static class LerpUtils
{
    public static float Clip(float a, float amp)
    {
        return MathHelper.Clamp(a + amp, 0, 1);
    }
    
    public static float Formant(float a, float amp) => MathHelper.Clamp((amp * a) + ((1 - a) / 2), 0, 1);
    
    public static float PhaseShift(float a, float shift) => MathHelper.Clamp((a + shift) % 1, 0, 1);

    public static float Flip(float a) => 1 - a;

    public static float Bend(float a, float bend) => MathHelper.Clamp(MathF.Pow(a,MathF.Pow(MathHelper.Clamp(bend,-0.99f,100),2)), 0, 1);

    /// <summary>
    /// this "windows" the phase (litreally a * LerpUtils.RectSin(a)
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static float Window(float a) => a * MathF.Sin(a * MathF.PI);
    
    public static float RectSin(float a, float sync = 1) => MathF.Sin(sync * a * MathF.PI);
}