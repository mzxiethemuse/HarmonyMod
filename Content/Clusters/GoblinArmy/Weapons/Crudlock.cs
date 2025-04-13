using HarmonyMod.Content.Dust;
using HarmonyMod.Content.Items.Weapons.Throwing;
using HarmonyMod.Content.Projectiles;
using HarmonyMod.Core.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.GoblinArmy.Weapons;

public class Crudlock : ModItem
{
    public override string Texture => "HarmonyMod/Content/Clusters/GoblinArmy/Weapons/Crudlock";

    public override void SetDefaults()
    {
        Item.useTime = 25;
        Item.useAnimation = 25;

        Item.damage = 17;
        Item.DamageType = DamageClass.Throwing;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.autoReuse = true;
        Item.crit = 15;
        
        Item.shootSpeed = 6.5f;
        Item.shoot = ModContent.ProjectileType<CrudlockProjectile>();

    }

    public override bool CanUseItem(Player player)
    {
        foreach (var projectile in Main.projectile)
        {
            if (projectile.active && projectile.owner == player.whoAmI && projectile.type == ModContent.ProjectileType<CrudlockProjectile>()) return false;
        }

        return true;
    }
}
public class CrudlockProjectile : ModProjectile
{
    public override string Texture => "HarmonyMod/Content/Clusters/GoblinArmy/Weapons/Crudlock";
    
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        ProjectileID.Sets.TrailCacheLength[Projectile.type] = 34;
    }
    
    public override void SetDefaults()
    {
        Projectile.aiStyle = ProjAIStyleID.Boomerang;
        Projectile.DamageType = DamageClass.Throwing;
        Projectile.knockBack = 3f;
        Projectile.width = 36;
        Projectile.height = 22;
        Projectile.penetrate = -1;
        Projectile.friendly = true;
        
    }
    
    public override bool PreDraw(ref Color lightColor)
    {
        Trails.DrawTrail(Projectile.oldPos, Projectile.oldRot, Projectile.Size, Trails.BasicColorLerp(Color.Red * 0.3f, Color.Transparent), Trails.BasicWidthLerp(1f, 12f), -2.8f, 1.7f, "LightDisc");

        return true;
    }
    //
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Projectile.damage += 5;
        if (hit.Crit)
        {
            
            SoundEngine.PlaySound(SoundID.Item36.WithPitchOffset(0.5f));

            SoundEngine.PlaySound(SoundID.Item41.WithPitchOffset(0.6f));
            for (int i = 0; i < 18; i++)
            {
                Terraria.Dust.NewDust(target.position, target.width, target.height, DustID.Torch);
                Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);

                Terraria.Dust.NewDust(target.position, target.width, target.height, DustID.Flare);
                if (i % 4 == 0)
                {
                    var p = Projectile.NewProjectileDirect(target.GetSource_OnHurt(null), Projectile.Center,
                        Main.rand.NextVector2CircularEdge(4f, 4f), ProjectileID.MeteorShot, 8, 2f);
                    p.penetrate = 3;
                    p.timeLeft = 45;

                }
            }
            Burst.SpawnBurst("Explosion", Projectile.Center, Color.OrangeRed * 0.7f, 50f, 38);
            Projectile.CritChance += 5;
        }
    }
}