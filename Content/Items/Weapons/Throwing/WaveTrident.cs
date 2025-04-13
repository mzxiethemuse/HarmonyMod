using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Items.Weapons.Throwing;

public class WaveTrident : ModItem
{
    public override void SetDefaults()
    {
        Item.useTime = 25;
        Item.useAnimation = 25;

        Item.damage = 22;
        Item.DamageType = DamageClass.Throwing;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.autoReuse = true;
        
        
        Item.shootSpeed = 9.5f;
        Item.shoot = ModContent.ProjectileType<WaveTridentProj>();

    }
}