using HarmonyMod.Content.Projectiles;
using HarmonyMod.Core.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Items.Weapons.Throwing;

public class WaveTridentProj : ModProjectile
{

    public override void SetDefaults()
    {
        Projectile.aiStyle = -1;
        Projectile.damage = 2;
        Projectile.DamageType = DamageClass.Throwing;
        Projectile.knockBack = 5f;
        Projectile.width = 24;
        Projectile.height = 32;
        Projectile.friendly = true;
    }

    public override void AI()
    {
        Projectile.velocity.Y += 0.2f;
        Projectile.rotation = Projectile.velocity.ToRotation();
    }

    public override void OnKill(int timeLeft)
    {
        if (Projectile.owner == Main.myPlayer)
        {
            for (int i = 0; i < 3; i++)
            {
                Terraria.Dust.NewDust(Projectile.position, 30, 30, DustID.Water);
                Terraria.Dust.NewDust(Projectile.position, 30, 30, DustID.GemSapphire);
                Terraria.Dust.NewDust(Projectile.position, 30, 30, DustID.TreasureSparkle);
            }
            SoundEngine.PlaySound(SoundID.Item30, Projectile.position);
            SoundEngine.PlaySound(SoundID.Item50, Projectile.position);
            Explosion.SpawnExplosion<Explosion>(Projectile.Center, Projectile.damage / 2, 0f, Projectile.owner, 80f, 17, Color.Blue, 30, true);
            // Explosion.SpawnExplosion<Explosion>(Projectile.Center, 0, 0f, Projectile.owner, 80f, 17, Color.LightBlue * 0.3f, 30);

        }
    }
    
    public override bool PreDraw(ref Color lightColor)
    {
        Trails.DrawTrail(Projectile.oldPos, Projectile.oldRot, Projectile.Size, Trails.BasicColorLerp(Color.CadetBlue, Color.RoyalBlue), Trails.BasicWidthLerp(4f, 28f), -2.8f, 1.7f, "RainbowRod");

        return true;
    }
}