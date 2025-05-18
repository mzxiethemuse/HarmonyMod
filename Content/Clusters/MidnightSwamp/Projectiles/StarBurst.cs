using HarmonyMod.Content.Dust;
using HarmonyMod.Core.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.MidnightSwamp.Projectiles;

public class StarBurst : ModProjectile
{
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailingMode[Type] = 2;
        ProjectileID.Sets.TrailCacheLength[Type] = 21;
    }

    public override void SetDefaults()
    {
        Projectile.width = 22;
        Projectile.height = 22;
        Projectile.hostile = true;
        Projectile.damage = 10;
        Projectile.aiStyle = -1;
        base.SetDefaults();
    }

    public override void AI()
    {
        
        Projectile.rotation += 0.05f;
    }

    public override void OnSpawn(IEntitySource source)
    {
        SoundEngine.PlaySound(SoundID.Item8, Projectile.Center);
        base.OnSpawn(source);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        float[] rotations = new float[Projectile.oldPos.Length];
        for (int i = 0; i < rotations.Length; i++)
        {
            rotations[i] = Projectile.velocity.ToRotation();
            
        }
        MiscShaderData miscShaderData = GameShaders.Misc["HarmonyMod:BasicTrail"];
        miscShaderData.UseImage0(TextureAssets.Logo);
        Trails.DrawTrail(Projectile.oldPos, Projectile.oldRot, Projectile.Size, Trails.BasicColorLerp(Color.Blue, Color.Transparent), Trails.BasicWidthLerp(1, 10), miscShaderData);
        return base.PreDraw(ref lightColor);
    }

    public override void OnKill(int timeLeft)
    {
        // SoundEngine.PlaySound(SoundID.Item45.WithPitchOffset(1.5f), Projectile.Center);
        Burst.SpawnBurst(Asset.Assets.VFXCircle, Projectile.Center, Color.Azure * 0.4f, 30, 60);
        base.OnKill(timeLeft);
    }
}