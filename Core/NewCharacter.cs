using System;
using System.Collections.Generic;
using HarmonyMod.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Core;

public class NewCharacter : ModPlayer
{

    public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
    {
        if (!mediumCoreDeath)  return new List<Item> { new(ModContent.ItemType<SynergyGuide1>()) };
        return new Item[ItemID.None];
    }
}