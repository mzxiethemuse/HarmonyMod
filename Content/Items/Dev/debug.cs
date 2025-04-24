using HarmonyMod.Content.Clusters.Forest;
using HarmonyMod.Content.Clusters.MidnightSwamp;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Items.Dev
{ 
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class debug : ModItem
	{
		
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.HarmonyMod.hjson' file.
		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 28;
			Item.useAnimation = 28;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			// Item.DefaultToWhip(ModContent.ProjectileType<debugwhipproj>(), 20, 2, 6);

		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

		public override bool? UseItem(Player player)
		{
			Point mouseWorld = new Point((int)(Main.MouseWorld.X / 16), (int)(Main.MouseWorld.Y / 16));
			(new ForestGen("penis", 1)).Apply();
			// SwampGen.GenerateMidnightSwampAt(mouseWorld);
			return true;
		}
	}
}
