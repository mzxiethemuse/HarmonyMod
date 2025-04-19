using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.MidnightSwamp.Materials;

public class Starglob : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 18;
        Item.material = true;
        Item.maxStack = 99;
    }
    
}