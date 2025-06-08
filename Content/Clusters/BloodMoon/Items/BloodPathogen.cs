using HarmonyMod.Assets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.BloodMoon.Items;

public class BloodPathogen : ModItem
{
    public override string Texture => AssetDirectory.Placeholders + "GenericItem";

    public override void SetDefaults()
    {
        Item.maxStack = 99;
        Item.width = 26;
        Item.height = 26;
        Item.rare = ItemRarityID.LightRed;
        Item.material = true;
        Item.value = Item.buyPrice(0, 0, 15);  

    }

    public override Color? GetAlpha(Color lightColor)
    {
        return Color.Red * 0.6f;
    }
}