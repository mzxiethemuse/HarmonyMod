using System;
using System.Collections.Generic;
using HarmonyMod.Asset;
using HarmonyMod.Content.Dust;
using HarmonyMod.Core.Graphics;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.BloodMoon.Projectiles;

public class FloorArm : ModProjectile
{
    public override string Texture => $"Terraria/Images/Item_1304";

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
    }

    public override void SetDefaults()
    {
        Projectile.aiStyle = -1;
        Projectile.hide = true;
        Projectile.Size = new Vector2(17, 40);
        Projectile.hostile = true;
        Projectile.damage = 15;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 40 + 30; // just for readability
        Projectile.rotation = -MathF.PI / 8f;
        base.SetDefaults();
    }

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.rotation = -MathF.PI / 5f;
        Projectile.ai[0] = Projectile.position.Y;
        base.OnSpawn(source);
    }

    public override void AI()
    {
        if (Projectile.timeLeft < 40)
        {
            Projectile.position.Y = Projectile.ai[0] - LerpUtils.RectSin(Projectile.timeLeft / 40f).Bend(-0.3f) * 50f;
            if (Projectile.timeLeft / 40f > 0.45f && Projectile.ai[1] != 255)
            {
                Projectile.ai[1] = 255;
                SoundEngine.PlaySound(SoundID.WormDig.WithPitchOffset(0.5f), Projectile.position);
                Burst.SpawnBurst(Assets.VFXCircleBlurred, Projectile.Center, Color.Red * 0.2f, 20, 20);
                DustEmitter.Emit(ModContent.DustType<FancySmoke>(), Projectile.Center, 1, 1, 8,
                    new Vector2(0.1f, 0.5f), Color.DarkKhaki * 0.2f, Color.SaddleBrown * 0.5f);
            }
        }
        else if (Projectile.timeLeft > 50)
        {
            Terraria.Dust.NewDustDirect(Projectile.Center + new Vector2(0, -75), 0, 30, DustID.LifeDrain, 0f, 0f, 200,
                Color.Red).velocity *= 0f;
        }
        // Projectile.position += Projectile.velocity;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Trails.DrawTrail(Projectile.oldPos, Projectile.oldRot, Projectile.Size + new Vector2(10, -12),
            Trails.BasicColorLerp(Color.Red * 0.4f, Color.Transparent), Trails.BasicWidthLerp(1f, 4f), -2.8f, 1.7f,
            "LightDisc");

        return true;
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs,
        List<int> behindProjectiles, List<int> overPlayers,
        List<int> overWiresUI)
    {
        if (Projectile.timeLeft < 40)
        {
            behindNPCsAndTiles.Add(index);
        }
    }
}