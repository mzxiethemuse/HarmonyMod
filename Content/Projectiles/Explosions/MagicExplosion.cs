using System;
using HarmonyMod.Assets;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace HarmonyMod.Content.Projectiles.Explosions;

public class MagicExplosion : Explosion
{
    public override string Texture => AssetDirectory.Glow + "Explosion";

    public override Texture2D ExplosionTexture
    {
        get => Assets.Assets.Textures["star_03"].Value;
    } //"light_0" + Main.rand.Next(1, 4)]; }

    protected override float ScaleLerpMod(float n)
    {
        return MathF.Sin(2.5f * n);
    }

    public override void PostAI()
    {
        Projectile.rotation += 0.02f;
    }
}