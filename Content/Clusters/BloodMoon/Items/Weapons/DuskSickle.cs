using System;
using HarmonyMod.Asset;
using HarmonyMod.Content.Dust;
using HarmonyMod.Core.Graphics;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.BloodMoon.Items.Weapons;

public class DuskSickle : ModItem
{
    public static int time = 28;
    private const float startRot = -MathF.PI / 3;
    private const float endRot = MathF.PI / 1.5f;
    
    
    public override void SetDefaults()
    {
        Item.Size = new Vector2(40, 40);
        Item.DamageType = DamageClass.Melee;
        Item.damage = 14;
        Item.UseSound = Assets.SickleSwing.WithPitchOffset(0.6f);
        
        Item.useTime = time / 2;
        Item.useAnimation = time;
        Item.useStyle = ItemUseStyleID.Swing;
        
        Item.noUseGraphic = true;
        Item.noMelee = true;
        
        
        Item.shoot = ModContent.ProjectileType<DuskSickleSwing>();
        Item.shootSpeed = 1f;
        base.SetDefaults();
    }
    
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity,
        int type,
        int damage, float knockback)
    {
        if (player.velocity.Y != 0)
        {
            player.velocity.X += 2f * player.direction;
            // player.velocity.X = MathHelper.Clamp(-30, 30, player.velocity.X);
        }
        //
        // player.GetModPlayer<SwingPlayer>().swing *= -1;
        // var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback,
        //     player.whoAmI);
        // // if (proj.ModProjectile is DuskSickleSwing projectile && player.GetModPlayer<SwingPlayer>().swing == -1)
        // // {
        // //     (projectile.startRotation, projectile.endRotation) = (projectile.endRotation, projectile.startRotation - 0.2f);
        // // }

        
        return true;

    }

    public override bool AltFunctionUse(Player player)
    {
        return true;
    }

    // public override void HoldItem(Player player)
    // {
    //     if (player.velocity.Y != 0 || player.HarmonyPlayer().TimeSinceYVelocityZero < 2)
    //     {
    //         player.velocity.X *= 1.005f;
    //         player.velocity.X = MathHelper.Clamp(-30, 30, player.velocity.X);
    //     }
    //     base.HoldItem(player);
    // }
}

public class DuskSickleSwing : SwordSwing
{
    
    public override string Texture => AssetDirectory.Content + "Clusters/BloodMoon/Items/Weapons/DuskSickle";

    public override int Width => 40;
    
    public override int Height => 40;
    public override bool Friendly => true;
    public override int Lifetime => 4 + DuskSickle.time / 2;

    public override float VisualRotationOffset => -0.2f;

    public override float StartRotation => 0;
    public override float EndRotation => MathF.PI / 2;
    

    
    

    public override Vector2 HoldOffset => Vector2.Lerp(new Vector2(0, 0), new Vector2(10, -10),
        LerpUtils.RectSin((Lifetime - Projectile.timeLeft) / Lifetime)
        );

    public override float GetLerpValue(float n)
    {
        return Easing.OutExpo(n);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        var owner = Main.player[Projectile.owner];
        if (MathF.Abs(owner.velocity.X) > 1)
        {
            Burst.SpawnBurst(Assets.VFXScorch[2], target.Center, Color.DarkRed * 0.3f, 70, 20);
        }
        base.OnHitNPC(target, hit, damageDone);
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        var owner = Main.player[Projectile.owner];

        modifiers.Knockback += 1f;
        modifiers.FlatBonusDamage += (int)owner.velocity.Length() * 2;
        
    }


}