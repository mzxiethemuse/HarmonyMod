using System;
using HarmonyMod.Content.Dusts;
using HarmonyMod.Content.Dusts.BurstDatas;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.BloodMoon.Items.Weapons;

public class BayonetRifle : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.Size = new Vector2(76, 18);
        
        Item.DamageType = DamageClass.Ranged;
        Item.damage = 17;
        Item.useTime = 36;
        Item.useAnimation = 36;
        
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.holdStyle = ItemHoldStyleID.None;

        Item.noMelee = true;
        Item.autoReuse = true;
        Item.shoot = ProjectileID.PurificationPowder; //goofy Aah. Projectilest.
        Item.shootSpeed = 12f;
        Item.useAmmo = AmmoID.Bullet;
    }

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            if (player.altFunctionUse == 2)
            {
                SoundEngine.PlaySound(SoundID.Item1, player.position);
                Item.useTime = 20;
                Item.useAnimation = 20;

                Item.noUseGraphic = true;
            }
            else
            {
                
                SoundEngine.PlaySound(SoundID.Item11, player.position);
                Item.useTime = 36;
                Item.useAnimation = 36;
                Item.noUseGraphic = false;
            }
        }
        return null;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type,
        int damage, float knockback)
    {

        if (player.altFunctionUse == 2)
        {
            Projectile.NewProjectileDirect(source, position, Vector2.Zero, ModContent.ProjectileType<BayonetRifleStab>(), 10, 7f, player.whoAmI);
            return false;
        }
        return true;
    }
    public override bool AltFunctionUse(Player player) => true;
    
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<BloodPathogen>(), 6);
        recipe.AddIngredient(ItemID.TheUndertaker);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
        
        recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<BloodPathogen>(), 6);
        recipe.AddIngredient(ItemID.Musket);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
        base.AddRecipes();
    }
}

public class BayonetRifleStab : SwordSwing
{
    public override SwordSwingType SwingType => SwordSwingType.Thrusting;

    public override string Texture => "HarmonyMod/Content/Clusters/BloodMoon/Items/Weapons/BayonetRifle";
    public override int Width => 76;
    public override int Height => 18;
    public override bool Friendly => true;
    public override int Lifetime => 20;

    public override float Distance => 22f;
    public override bool TwoHanded => true;
    //
    // public override bool DrawTrail => true;
    //
    // public override TrailData Trail => new TrailData(Trails.BasicColorLerp(Color.WhiteSmoke * 0.4f, Color.Transparent), Trails.BasicWidthLerp(1, 36), "LightDisc");

    public override float GetLerpValue(float n)
    {
        return Easing.OutQuint(LerpUtils.RectSin(n));
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        for (int i = 0; i < Main.rand.Next(1,4); i++)
        {
            Burst.SpawnBurst(Main.rand.NextVector2FromRectangle(target.Hitbox), Color.WhiteSmoke * 0.5f,
                new InBurst(Assets.Assets.VFXStar[3], 16, 18));
        }
        
        base.OnHitNPC(target, hit, damageDone);
    }

    public override void PostAI()
    {
        if (owner != null && owner.whoAmI == Main.myPlayer)
        {
            float a = GetLerpValue((Lifetime - Projectile.timeLeft) / (float)Lifetime);
            Vector2 pos = owner.Center + Projectile.velocity * (Distance * a + Projectile.width) * 0.5f;
            if (Main.rand.NextBool(10))
            {
                Burst.SpawnBurst(pos, Color.WhiteSmoke * 0.4f,
                    new InBurst(Assets.Assets.VFXStar[3], 16, 28));
            }
        }
    }
}