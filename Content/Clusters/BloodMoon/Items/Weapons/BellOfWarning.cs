using System.Linq;
using HarmonyMod.Content.Clusters.BloodMoon.Projectiles;
using HarmonyMod.Content.Dusts;
using HarmonyMod.Content.Dusts.BurstDatas;
using HarmonyMod.Content.Projectiles;
using Microsoft.Build.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.BloodMoon.Items.Weapons;

public class BellOfWarning : ModItem
{
    public override string Texture => $"Terraria/Images/Item_{ItemID.Bell}";
    
    public override void SetDefaults()
    {
        Item.Size = new Vector2(21, 21);
        
        Item.DamageType = DamageClass.Magic;
        Item.damage = 21;
        Item.useTime = 46;
        Item.useAnimation = 46;
        
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.holdStyle = ItemHoldStyleID.None;

        Item.noMelee = true;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<WarningResonance>(); //goofy Aah. Projectilest.
        Item.shootSpeed = 7f;
        Item.mana = 15;
        
        Item.UseSound = SoundID.Item35.WithPitchOffset(0.8f).WithVolumeScale(2f);
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type,
        int damage, float knockback)
    {
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);
        return false;
    }
}

public class WarningResonance : ModProjectile
{
    public override string Texture => $"Terraria/Images/Item_{ItemID.Bell}";

    public override void SetDefaults()
    {
        Projectile.hide = true;
        Projectile.width = 16;
        Projectile.height = 16;
        Projectile.timeLeft = 60;
        Projectile.tileCollide = false;
    }

    public override bool PreAI()
    {

            Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(10, 10), DustID.GemRuby, Vector2.Zero).noGravity = true;
            Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(10, 10), ModContent.DustType<Lightspot>(), Vector2.Zero, 50, Color.Red).noGravity = true;

        

        if (Projectile.Center.Distance(new Vector2(Projectile.ai[0], Projectile.ai[1])) < 5)
        {
            Projectile.Kill();
        }
        
        return true;
    }

    public override void OnKill(int timeLeft)
    {
        Hitbox.SpawnHitbox(Projectile.GetSource_Death(), Projectile.Center, 110, 110, Projectile.damage, 10, -1, true);
        SoundEngine.PlaySound(SoundID.Item74);
        for (int i = 0; i < 120; i++)
        {
            Vector2 pos = new Vector2(100, 0).RotatedBy(MathHelper.ToRadians((360f / 60) * i));

                Dust.NewDustPerfect(Projectile.Center + pos + Main.rand.NextVector2Circular(10, 10), DustID.GemRuby, Vector2.Zero).noGravity = true;
                Dust.NewDustPerfect(Projectile.Center, DustID.GemRuby, Main.rand.NextVector2Circular(16, 16)).noGravity = true;
            
        }
        
        Burst.SpawnBurst(Projectile.Center, Color.Red * 0.4f, new InBurst(Assets.Assets.VFXCircle, 60, 110));

        // foreach (var flesh in Main.projectile.Where(proj => proj.type == ModContent.ProjectileType<LeperFlesh>() && proj.active && proj.Center.Distance(Projectile.Center) < 100))
        // {
        //     SoundEngine.PlaySound(SoundID.NPCDeath1);
        //
        //     Burst.SpawnBurst(flesh.Center, Color.DarkRed * 0.8f, new BurstData(Assets.Assets.VFXCircle2, 40, 50));
        //     Hitbox.SpawnHitbox(flesh.GetSource_Death(), flesh.Center, 50, 50, Projectile.damage, 12, -1, true);
        //     flesh.Kill();
        // }
        
        base.OnKill(timeLeft);
    }
}