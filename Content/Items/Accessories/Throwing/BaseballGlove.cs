using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Items.Accessories.Throwing
{
    public class BaseballGlove : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Green;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            player.GetAttackSpeed(DamageClass.Throwing) += 0.2f;

        }
    }
}
