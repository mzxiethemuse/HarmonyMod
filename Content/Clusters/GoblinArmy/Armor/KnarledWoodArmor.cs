using System.Linq;
using HarmonyMod.Assets;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.GoblinArmy.Armor;

[AutoloadEquip(EquipType.Head)]
public class KnarledWoodHelmet : ModItem
{
    public static LocalizedText SetBonusText { get; private set; }

    public static int summonDamageBonus = 5;

    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(summonDamageBonus);
    
    public override void SetStaticDefaults() {
        // If your head equipment should draw hair while drawn, use one of the following:
        // ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
        // ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
        // ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
        // ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true;
    }
    
    public override string Texture => AssetDirectory.Placeholders + "GenericHelmet";
    
    public override void SetDefaults() {
        Item.width = 18; // Width of the item
        Item.height = 18; // Height of the item
        Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
        Item.rare = ItemRarityID.Green; // The rarity of the item
        Item.defense = 5; // The amount of defense the item will give when equipped
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return body.type == ModContent.ItemType<KnarledWoodChestplate>() && legs.type == ModContent.ItemType<KnarledWoodLegs>();
    }

    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Summon) *= 1 + summonDamageBonus / 100f;
    }

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = "Releases Shadowflame tendrils at nearby enemies";


        base.UpdateArmorSet(player);
    }
}

[AutoloadEquip(EquipType.Body)]
public class KnarledWoodChestplate : ModItem
{
    public override string Texture => AssetDirectory.Placeholders + "GenericChestplate";
    
    public static int summonDamageBonus = 10;

    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(summonDamageBonus);
    public override void SetDefaults() {
        Item.width = 18; // Width of the item
        Item.height = 18; // Height of the item
        Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
        Item.rare = ItemRarityID.Green; // The rarity of the item
        Item.defense = 8; // The amount of defense the item will give when equipped
    }
    
    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Summon) *= 1 + summonDamageBonus / 100f;
    }
}

[AutoloadEquip(EquipType.Legs)]
public class KnarledWoodLegs : ModItem
{
    public override string Texture => AssetDirectory.Placeholders + "GenericLeggings";
    
    public override void SetDefaults() {
        Item.width = 18; // Width of the item
        Item.height = 18; // Height of the item
        Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
        Item.rare = ItemRarityID.Green; // The rarity of the item
        Item.defense = 7; // The amount of defense the item will give when equipped
    }
}