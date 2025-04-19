using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.MidnightSwamp.Materials;

public class TransmutationEssence : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.ItemNoGravity[Item.type] = true; // Makes the item have no gravity
    }

    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 28;
        Item.material = true;
        Item.maxStack = 99;
    }

    public override Color? GetAlpha(Color lightColor)
    {
        return lightColor * 0.5f;
    }
    
}