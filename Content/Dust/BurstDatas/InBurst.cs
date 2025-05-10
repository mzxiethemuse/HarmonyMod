using System;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace HarmonyMod.Content.Dust.BurstDatas;

public class InBurst : BurstData
{
    
    public InBurst(Asset<Texture2D> texture, float duration, float radius) : base(texture, duration, radius)
    {
    }
    
    public override float ScaleLerpMod(float n)
    {
        return LerpUtils.Flip(n).Bend(1.5f);
    }
    
}