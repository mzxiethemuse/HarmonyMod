using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace HarmonyMod.Content.Dust.BurstDatas;

public class MagicBurst : BurstData
{
    public MagicBurst(Asset<Texture2D> texture, float duration, float radius) : base(texture, duration, radius)
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