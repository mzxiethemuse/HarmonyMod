using HarmonyMod.Asset;
using HarmonyMod.Content.Clusters.MidnightSwamp.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.MidnightSwamp.Accessories;

public class StellarGut : ModItem
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
        player.GetModPlayer<MidnightSwampPlayer>().StellarGut = true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient<Starglob>(8);
        recipe.AddTile(TileID.Bottles);
        recipe.Register();
    }
}
