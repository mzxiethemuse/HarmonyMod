using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.GoblinArmy;

public class KnarledWood : ModItem
{
    public override void SetDefaults()
    {
        Item.material = true;
        Item.width = 20;
        Item.height = 18;
        Item.rare = ItemRarityID.Blue;
        
        Item.maxStack = 999;
        Item.useTime = 10;
        Item.useAnimation = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        //temporary fix this later
        Item.createTile = TileID.DynastyWood;
    }
}

