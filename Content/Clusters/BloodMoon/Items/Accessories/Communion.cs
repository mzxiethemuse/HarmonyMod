using HarmonyMod.Asset;
using HarmonyMod.Content.Dust;
using HarmonyMod.Content.Dust.BurstDatas;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.BloodMoon.Items.Accessories;

public class Communion : ModItem
{
    public override string Texture => AssetDirectory.Placeholders + "GenericItem";
    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 26;
        Item.height = 26;
        Item.rare = ItemRarityID.LightRed;

    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.HarmonyPlayer().communionEquipped = true;
        if (player.HarmonyPlayer().TimeSinceLastHurt == (60 * 5))
        {
            SoundEngine.PlaySound(SoundID.Item111, player.Center);
            Burst.SpawnBurst(player.Center, Color.Red * 0.4f, new FollowPlayerBurst(Assets.VFXScorch[2], 20, 60, player.whoAmI));
            DustEmitter.Emit(DustID.LifeDrain, player.position, player.width, player.height, 20);
            player.Heal(60);
        }
    }
    
    public override Color? GetAlpha(Color lightColor)
    {
        return Color.Red * 0.8f;
    }
    
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient<BloodPathogen>(6);
        recipe.AddIngredient(ItemID.GoldBar, 8);
        recipe.Register();
        
        recipe = CreateRecipe();
        recipe.AddIngredient<BloodPathogen>(6);
        recipe.AddIngredient(ItemID.PlatinumBar, 8);
        recipe.Register();
    }
}