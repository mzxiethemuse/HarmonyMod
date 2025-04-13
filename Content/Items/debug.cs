using HarmonyMod.Content.Dust;
using HarmonyMod.Content.Projectiles;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Items
{ 
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class debug : ModItem
	{
		static DustEmitter buildSmoke = new DustEmitter(ModContent.DustType<FancySmoke>());
    
		
		public override void SetStaticDefaults()
		{
			buildSmoke.SetScaleRange(0.8f, 3f);
			buildSmoke.SetVelocitySpread(new Vector2(5, 2));
			base.SetStaticDefaults();
		}
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
			// Item.shoot = ModContent.ProjectileType<debugwhipproj>();
			// Item.shootSpeed = 12f;

		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type,
			int damage, float knockback)
		{
			// buildSmoke.Emit(Main.MouseWorld, 1, 1, 10);
			// Explosion.SpawnExplosion<Explosion>(Main.MouseWorld, 0, 0f, player.whoAmI, 22f, 17, Color.MediumPurple, 30);

			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
	}
}
