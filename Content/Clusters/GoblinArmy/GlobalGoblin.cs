using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyMod.Content.Clusters.GoblinArmy.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace HarmonyMod.Content.Clusters.GoblinArmy;

public class GlobalGoblin : GlobalNPC
{
    public static List<DeadGoblin> deadGoblins = new();   
    
    public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
    {
        if (spawnInfo.Invasion && Main.invasionType == 1)
        {
            pool[ModContent.NPCType<GoblinBrawler>()] = 0.1f + (0.02f * ModContent.GetInstance<GoblinWarfareSystem>().GoblinWarsFought);
            pool[ModContent.NPCType<GoblinEngineer>()] = 0.25f + 0.02f * ModContent.GetInstance<GoblinWarfareSystem>().GoblinWarsFought;
            pool[ModContent.NPCType<GoblinSuicideBomber>()] = 0.15f;
            if (ModContent.GetInstance<GoblinWarfareSystem>().GoblinWarsFought > 1)
            {
                pool[ModContent.NPCType<GoblinMedic>()] = 0.15f;

            }

        }
        base.EditSpawnPool(pool, spawnInfo);
    }

    
    //entry.Info.Contains(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Goblins)
    public static int[] goblins =
    {
        NPCID.GoblinArcher,
        NPCID.GoblinPeon,
        NPCID.GoblinWarrior,
        NPCID.GoblinSummoner,
        NPCID.GoblinArcher,
        NPCID.GoblinThief,
        ModContent.NPCType<GoblinEngineer>(),
        ModContent.NPCType<GoblinSuicideBomber>(),
        ModContent.NPCType<GoblinBrawler>(),
        ModContent.NPCType<GoblinMedic>()

    };

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => goblins.Contains(entity.type);
        // Main.BestiaryDB.FindEntryByNPCID(entity.type).Info
        //     .Contains(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Invasions.Goblins);//goblins.Contains(entity.type);

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        npcLoot.Add(new CommonDrop(ItemID.TatteredCloth, 3));
        npcLoot.Add(new CommonDrop(ModContent.ItemType<KnarledWood>(), 4, 1, 3));
    }

    public override void OnSpawn(NPC npc, IEntitySource source)
    {
        if (npc.type == NPCID.GoblinArcher && source is EntitySource_SpawnNPC)
        {
            for (int i = 0; i < Main.rand.Next(1, 4); i++)
            {
                NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X + Main.rand.Next(-50, 51), (int)npc.Center.Y, NPCID.GoblinArcher);
            }
        }
    }

    // public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    // {
    //     GameShaders.Misc["HarmonyMod:GuardShader"].Apply();
    //     return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
    // }
    
    public override void OnKill(NPC npc)
    {
        if (npc.type != ModContent.NPCType<GoblinSuicideBomber>() && npc.type != ModContent.NPCType<GoblinMedic>()) deadGoblins.Add(new DeadGoblin(npc.type, npc.position));
    }
}

public class DeadGoblin(int type, Vector2 position)
{
    public int time = 0;
    public Vector2 position = position;
    public int type = type;
}