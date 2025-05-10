using HarmonyMod.Asset;
using Microsoft.Xna.Framework;
using HarmonyMod.Content.Reworks.Ranger;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Items.Accessories.Ranger
{
    public class Gimbal : ModItem
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
            player.GetModPlayer<RangerPlayer>().globalRecoilBonus -= 0.1f;
            player.GetModPlayer<RangerPlayer>().horizontalRecoilMod -= 0.5f;
            player.GetModPlayer<RangerPlayer>().recoilPowerBonus -= 0.1f;

            if (player.velocity.Distance(Vector2.Zero) > RangerPlayer.eyeattachmentspeedreq)
            {
                player.GetModPlayer<RangerPlayer>().globalRecoilBonus -= 0.2f;
                player.GetModPlayer<RangerPlayer>().horizontalRecoilMod -= 0.2f;

                player.GetModPlayer<RangerPlayer>().drawNerveSpeed = true;

            }
        }
        
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<StabilizerGrip>());
            recipe.AddIngredient(ModContent.ItemType<CompoundEye>());
            recipe.AddIngredient(ModContent.ItemType<GunGrip>());
            recipe.AddIngredient(ItemID.Wire, 20);

            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
