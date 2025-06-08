using HarmonyMod.Assets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Items.Accessories.Ranger
{
    public class GunGecko : ModItem
    {
        public override string Texture => AssetDirectory.Placeholders + "GenericItem";

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 3, 30, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Item ammo = player.ChooseAmmo(player.HeldItem);
            if (ammo == null) return;
            if (ammo.type != ItemID.MusketBall && ammo.type != ItemID.ChlorophyteBullet && ammo.type != ItemID.WoodenArrow && ammo.type != ItemID.FlamingArrow)
            {
                player.GetDamage(DamageClass.Ranged) += 0.1f;
            }
        }
    }
}