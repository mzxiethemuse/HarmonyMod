using HarmonyMod.Assets;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.Forest.Weapons;

public class RootStaff : ModItem
{
    public override string Texture => AssetDirectory.Placeholders + "GenericItem";

    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 24;
        Item.damage = 9;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.autoReuse = true;
        Item.mana = 10;
        Item.channel = true; 
        Item.InterruptChannelOnHurt = true;
    }
}