using HarmonyMod.Content.Items.Accessories.Ranger;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Core;

public class MobLoot : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.EyeofCthulhu)
        {
            npcLoot.Add(new CommonDrop(ModContent.ItemType<CthulhuEyeAttachment>(), 1));
        }
    }
}