﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyMod.Assets;
using HarmonyMod.Core.Reworks.Ranger;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Items.Accessories.Ranger
{
    public class ChameleonFireCharm : ModItem
    {
        public override string Texture => AssetDirectory.Placeholders + "GenericItem";

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 3, 10);

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RangerPlayer>().recoilDecayBonus += 0.2f;
            Item ammo = player.ChooseAmmo(player.HeldItem);
            if (ammo == null) return;
            if (ammo.type != ItemID.MusketBall && ammo.type != ItemID.ChlorophyteBullet && ammo.type != ItemID.WoodenArrow && ammo.type != ItemID.FlamingArrow)
            {
                player.GetModPlayer<RangerPlayer>().globalRecoilBonus -= 0.2f;
                player.GetModPlayer<RangerPlayer>().flameBullets = true;
                player.GetDamage(DamageClass.Ranged) += 0.15f;
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<HellHandle>());
            recipe.AddIngredient(ModContent.ItemType<GunGecko>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
