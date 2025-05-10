using HarmonyMod.Asset;
using Microsoft.Xna.Framework;
using HarmonyMod.Content.Reworks.Ranger;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Items.Accessories.Ranger
{
    public class CompoundEye : ModItem
    {
        public override string Texture => AssetDirectory.Placeholders + "GenericItem";

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(0, 2, 30, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.velocity.Distance(Vector2.Zero) > RangerPlayer.eyeattachmentspeedreq)
            {
                player.GetModPlayer<RangerPlayer>().horizontalRecoilMod -= 0.25f;
                player.GetModPlayer<RangerPlayer>().globalRecoilBonus -= 0.2f;
                player.GetModPlayer<RangerPlayer>().drawNerveSpeed = true;
            }
        }
        
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CthulhuEyeAttachment>());
            recipe.AddIngredient(ItemID.HoneyComb);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
