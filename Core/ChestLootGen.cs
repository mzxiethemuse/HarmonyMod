using System.Collections.Generic;
using HarmonyMod.Content.Items.Accessories.Ranger;
using HarmonyMod.Content.Items.Weapons.Throwing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Core
{
	// W TAKEN FROM EXAMPLE MOD!!
	// This class showcases adding additional items to vanilla chests.
	// This example simply adds additional items. More complex logic would likely be required for other scenarios.
	// If this code is confusing, please learn about "for loops" and the "continue" and "break" keywords: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/jump-statements
	public class ChestLootGen : ModSystem
	{
		public enum ChestType
		{
			Wood,
			Gold,
			GoldLocked,
			Shadow,
			ShadowLocked,
			Barrel,
			TrashCan,
			CorruptPurpleLookinThing,
			Mahogany,
			Idunno,
			JungleShrine,
			Frozen,
			Mossy,
			Sky,
			CorruptIThink,
			Webby,
			Lihzahrd,
			Ocean
		}
		// We use PostWorldGen for this because we want to ensure that all chests have been placed before adding items.
		public override void PostWorldGen() {
			AddItemsToChests([ModContent.ItemType<GunGecko>()], ChestType.JungleShrine, 6, 3);

		}


		public void AddItemsToChests(int[] ids, ChestType chestType, int count, int chance)
		{
			// I LOVE STEALING FROM EXAMPLE MOD!!
			// I LOVE STEALING FROM EXAMPLE MOD!!
			// I LOVE STEALING FROM EXAMPLE MOD!!
			// I LOVE STEALING FROM EXAMPLE MOD!!
			// I LOVE STEALING FROM EXAMPLE MOD!!
			// I LOVE STEALING FROM EXAMPLE MOD!!
			// I LOVE STEALING FROM EXAMPLE MOD!!// I LOVE STEALING FROM EXAMPLE MOD!!
			// I LOVE STEALING FROM EXAMPLE MOD!!
						// Place some additional items in Frozen Chests:
			// These are the 3 new items we will place.
			
			// This variable will help cycle through the items so that different Frozen Chests get different items
			int choice = 0;
			// Rather than place items in each chest, we'll place up to 6 items (2 of each). 
			int itemsPlaced = 0;
			int maxItems = count;
			// Loop over all the chests
			for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++) {
				Chest chest = Main.chest[chestIndex];
				if (chest == null) {
					continue;
				}
				Tile chestTile = Main.tile[chest.x, chest.y];
				// We need to check if the current chest is the Frozen Chest. We need to check that it exists and has the TileType and TileFrameX values corresponding to the Frozen Chest.
				// If you look at the sprite for Chests by extracting Tiles_21.xnb, you'll see that the 12th chest is the Frozen Chest. Since we are counting from 0, this is where 11 comes from. 36 comes from the width of each tile including padding. An alternate approach is to check the wiki and looking for the "Internal Tile ID" section in the infobox: https://terraria.wiki.gg/wiki/Frozen_Chest
				if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == (int)chestType * 36) {
					// We have found a Frozen Chest
					// If we don't want to add one of the items to every Frozen Chest, we can randomly skip this chest with a 33% chance.
					if (WorldGen.genRand.NextBool(chance))
						continue;
					// Next we need to find the first empty slot for our item
					for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++) {
						if (chest.item[inventoryIndex].type == ItemID.None) {
							// Place the item
							chest.item[inventoryIndex].SetDefaults(ids[choice]);
							// Decide on the next item that will be placed.
							choice = (choice + 1) % ids.Length;
							// Alternate approach: Random instead of cyclical: chest.item[inventoryIndex].SetDefaults(WorldGen.genRand.Next(itemsToPlaceInFrozenChests));
							itemsPlaced++;
							break;
						}
					}
				}
				// Once we've placed as many items as we wanted, break out of the loop
				if (itemsPlaced >= maxItems) {
					break;
				}
			}
		}
	}
}