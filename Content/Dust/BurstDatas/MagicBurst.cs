using System;

namespace HarmonyMod.Content.Dust.BurstDatas;

public class MagicBurst : BurstData
{
    public MagicBurst(string texture, float duration, float radius) : base(texture, duration, radius)
    {
    }
    
    public override float ScaleLerpMod(float n)
    {
        return MathF.Sin(2.5f * n);
    }
    
    public override void AI(ref Terraria.Dust dust)
    {
        dust.rotation += 0.01f;
    }
    
}