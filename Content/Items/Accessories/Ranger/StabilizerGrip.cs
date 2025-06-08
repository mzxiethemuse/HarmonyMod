using HarmonyMod.Assets;
using HarmonyMod.Core.Reworks.Ranger;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Items.Accessories.Ranger
{
    public class StabilizerGrip : ModItem
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
            player.GetModPlayer<RangerPlayer>().horizontalRecoilMod -= 0.5f;
        }
    }
}