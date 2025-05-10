using System.Collections.Generic;
using System.Linq;
using HarmonyMod.Content.Clusters.BloodMoon.Items;
using HarmonyMod.Content.Clusters.BloodMoon.NPCs;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.BloodMoon;

public class BloodMoonGlobalNPC : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) =>
        Main.BestiaryDB.FindEntryByNPCID(entity.type).Info.Contains(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.BloodMoon);

    public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
    {
        if (Main.bloodMoon)
        {
            pool[ModContent.NPCType<MutilatedZombie>()] = 0.33f;
            if (Main.npc.Count(npc => npc.type == ModContent.NPCType<SanguineNecromancer>() && npc.active) == 0)
            {
                pool[ModContent.NPCType<SanguineNecromancer>()] = 0.1f;
            }

        }
        base.EditSpawnPool(pool, spawnInfo);
    }

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodPathogen>(), 6));
        base.ModifyNPCLoot(npc, npcLoot);
    }
}