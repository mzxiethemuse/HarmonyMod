using System;
using HarmonyMod.Core.Graphics;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.GoblinArmy.Weapons;

public class KnarledSaber : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 48;
        Item.height = 60;
        Item.damage = 23;
        Item.useTime = 38;
        Item.useAnimation = 38;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.autoReuse = true;
        Item.rare = ItemRarityID.Blue;
        Item.crit = 6;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.UseSound = SoundID.Item1;
        Item.DamageType = DamageClass.Melee;
        
        Item.shoot = ModContent.ProjectileType<KnarledSaberSwing>();
        Item.shootSpeed = 2f;
        
        base.SetDefaults();
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type,
        int damage, float knockback)
    {
        player.GetModPlayer<SwingPlayer>().swing *= -1;
        var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
        // if (proj.ModProjectile is KnarledSaberSwing projectile && player.GetModPlayer<SwingPlayer>().swing == 1)
        // {
        //     (projectile.startRotation, projectile.endRotation) = (projectile.endRotation, projectile.startRotation);
        // }
        return false;
    }



    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<KnarledWood>(), 25);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}

public class KnarledSaberSwing : SwordSwing
{

    public override string Texture => "HarmonyMod/Content/Clusters/GoblinArmy/Weapons/KnarledSaber";
    public override int Width => 54;
    public override int Height => 54;
    public override int Lifetime => 31;

    public override bool TwoHanded => true;
    public override float StartRotation => -MathF.PI / 2;
    public override float EndRotation => MathF.PI / 2;

    public override bool Friendly => true;
    

    public override float GetLerpValue(float n)
    {
        return Easing.InOutCubic(n);
    }
    
    public override void SetStaticDefaults()
    {

        ProjectileID.Sets.TrailingMode[this.Type] = 4;
        ProjectileID.Sets.TrailCacheLength[this.Type] = 34;

        base.SetStaticDefaults();
    }


    public override void PostDraw(Color lightColor)
    {
        if (Main.rand.NextBool(5)) Terraria.Dust.NewDust(Projectile.position, Width, Height, DustID.Shadowflame);

        
        
        base.PostDraw(lightColor);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Main.rand.NextBool(3)) target.AddBuff(BuffID.ShadowFlame, 30);
    }
}
