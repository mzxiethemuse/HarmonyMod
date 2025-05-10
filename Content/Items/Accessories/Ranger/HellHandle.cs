using HarmonyMod.Content.Reworks.Ranger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyMod.Asset;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Items.Accessories.Ranger
{
    public class HellHandle : ModItem
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
            player.GetModPlayer<RangerPlayer>().globalRecoilBonus -= 0.2f;
            player.GetModPlayer<RangerPlayer>().recoilDecayBonus += 0.2f;
            player.GetModPlayer<RangerPlayer>().flameBullets = true;

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<GunGrip>());
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.AddIngredient(ItemID.MagmaStone);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
