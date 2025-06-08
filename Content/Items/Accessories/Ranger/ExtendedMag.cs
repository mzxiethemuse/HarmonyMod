using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyMod.Assets;
using HarmonyMod.Core.Reworks.Ranger;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Items.Accessories.Ranger
{
    public class ExtendedMag : ModItem
    {
        public override string Texture => AssetDirectory.Placeholders + "GenericItem";

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Green;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            player.GetModPlayer<RangerPlayer>().ammoConsumeChance += 0.2f;


        }
    }
}
