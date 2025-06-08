using System;
using System.Collections.Generic;
using HarmonyMod.Content.Dusts;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.GoblinArmy.Weapons;

public class YurgWhip : ModItem
{
    public override string Texture => $"HarmonyMod/Content/Clusters/GoblinArmy/Weapons/Yurg";

    public override void SetDefaults()
    {
        Item.damage = 21;
        Item.DamageType = DamageClass.Melee;
        // Item.width = 18;
        // Item.height = 18;
        Item.useTime = 44;
        Item.useAnimation = 44;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 6;
        Item.rare = ItemRarityID.Blue;
        Item.autoReuse = true;
        Item.DefaultToWhip(ModContent.ProjectileType<YurgWhipProjectile>(), 24, 2, 3.5f);
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity,
        int type,
        int damage, float knockback)
    {
        // Projectile.NewProjectile(source, position, velocity, type, damage / 2, knockback, player.whoAmI);

        Projectile.NewProjectile(source, position,
            velocity.RotatedByRandom(MathF.PI * 0.4f) * Main.rand.NextFloat(0.8f, 1f), type,
            (int)(damage * 0.75f), knockback, player.whoAmI);


        return true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<KnarledWood>(), 25);
        recipe.AddIngredient(ItemID.SpikyBall, 15);
        recipe.AddIngredient(ItemID.Rope, 30);

        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}

public class YurgWhipProjectile : BaseWhipProjectile
{
    public static BurstData spikyballburst = new BurstData(Assets.Assets.VFXCircleBlurred, 30, 20);
    public override string Texture => $"HarmonyMod/Content/Clusters/GoblinArmy/Weapons/YurgProjectile";

    public override bool PreAI()
    {
        if (Projectile.ai[0] == 30f && Main.rand.NextBool(2))
        {
            // GO MY MAGIC NUMBER
            if (Main.netMode != 2)
            {
                DustEmitter.Emit(DustID.WhiteTorch, Projectile.Center, 0, 0, 9);
                var p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center,
                    Projectile.velocity.RotatedByRandom(MathHelper.PiOver2) * 2,
                    ProjectileID.SpikyBall, 3, 2f, Projectile.owner);
                Burst.SpawnBurst(p.Center, Color.Gray, spikyballburst); 

            }
        }

        return true;
    }
}