using HarmonyMod.Assets;
using HarmonyMod.Core.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.Forest.Projectiles;

// herb orb...
public class Horb : ModProjectile
{
    public override string Texture => AssetDirectory.Content + "Clusters/Forest/Weapons/HarpNut";

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailingMode[Type] = 2;
        ProjectileID.Sets.TrailCacheLength[Type] = 7;
    }

    public override void SetDefaults()
    {
        Projectile.width = 12;
        Projectile.height = 12;
        Projectile.damage = 2;
        Projectile.timeLeft = 120;
        Projectile.hostile = true;
        
    }

    public override void AI()
    {
        Projectile.velocity.Y += 0.1f;
        Projectile.rotation = Projectile.velocity.ToRotation();
        
    }

    public override Color? GetAlpha(Color lightColor)
    {
        return Color.White * 0.7f;
    }

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
    {
        if (Projectile.timeLeft < 100)
        {
            target.Heal(Projectile.damage);
            Projectile.Kill();

        }
        modifiers.Cancel();

    }

    public override bool PreDraw(ref Color lightColor)
    {
        Trails.DrawTrail(
            Projectile.oldPos,
            Projectile.oldRot,
            Projectile.Size / 2,
            Trails.BasicColorLerp(Color.LimeGreen * 0.2f, Color.Transparent),
             Trails.BasicWidthLerp(1, 6),
            2f,
            2f,
            "LightDisc"
            );
        
        return true;
    }
}