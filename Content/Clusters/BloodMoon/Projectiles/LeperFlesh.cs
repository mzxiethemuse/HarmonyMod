using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.BloodMoon.Projectiles;

public class LeperFlesh : ModProjectile
{
    public override string Texture => "HarmonyMod/Content/Clusters/BloodMoon/NPCs/FleshBall";
    
    public override void SetDefaults()
    {
        Projectile.usesIDStaticNPCImmunity = true;
        Projectile.idStaticNPCHitCooldown = 30;
        
        Projectile.penetrate = 4;
        
        Projectile.damage = 35;
        Projectile.aiStyle = -1;
        Projectile.Size = new Vector2(16, 16);
        Projectile.timeLeft = 600;
        base.SetDefaults();
    }
    
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.velocity = Main.rand.NextVector2CircularEdge(3.5f, 3.5f);
            Projectile.velocity.Y = -MathF.Abs(Projectile.velocity.Y);
        }
    
        public override void AI() 
        {
            if (Main.rand.NextBool(4))
            {
                Terraria.Dust.NewDust(Projectile.position, 16, 16, DustID.Blood);
            }

            Projectile.gfxOffY = MathF.Sin(Projectile.timeLeft / 35f);
            
            Projectile.velocity *= 0.95f;
            Projectile.rotation += Projectile.velocity.X * 0.2f;
            base.AI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.NPCHit1, Projectile.position);

            Projectile.velocity = Main.rand.NextVector2CircularEdge(6f, 6f);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 10; k++)
            {
                Terraria.Dust.NewDust(Projectile.position, 16, 16, DustID.Blood);
            }
            
            SoundEngine.PlaySound(SoundID.NPCDeath1.WithVolumeScale(0.5f).WithPitchOffset(1.5f), Projectile.position);
            base.OnKill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return (Projectile.timeLeft < 580) ? null : false;
        }
}