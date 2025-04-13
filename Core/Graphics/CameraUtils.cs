using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.CameraModifiers;

namespace HarmonyMod.Core.Graphics;

public static class CameraUtils
{
    public static void AddCameraModifier(ICameraModifier modifier) => Main.instance.CameraModifiers.Add(modifier);
    
    public static void AddScreenshakeModifier(Vector2 startPosition, Vector2 direction, float strength, int vibrationCyclesPerSecond, int frames) => Main.instance.CameraModifiers.Add(new PunchCameraModifier(startPosition, direction, strength, vibrationCyclesPerSecond, frames));

}