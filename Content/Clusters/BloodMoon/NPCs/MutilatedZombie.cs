using System;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.BloodMoon.NPCs;

public class MutilatedZombie : ComplexNPC
{
    public override long CoinValue => Item.buyPrice(0, 0, 5, 1);

    public override void SetStaticDefaults()
    {
        NPCID.Sets.Zombies[Type] = true;
        base.SetStaticDefaults();
    }

    public override void SetDefaults()
    {
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.Zombie2;
        
        NPC.aiStyle = NPCAIStyleID.Fighter;
        NPC.friendly = false;
        NPC.lifeMax = 90;
        NPC.damage = 35;
        
        NPC.Size = new Vector2(40, 43);
        base.SetDefaults();
    }

    public override void PostAI()
    {
        NPC.spriteDirection = NPC.velocity.X < 0 ? -1 : 1;
        base.PostAI();
    }

    public override void DrawEffects(ref Color drawColor)
    {
        if (Main.rand.NextBool(4))
        {
            Terraria.Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
        }

        base.DrawEffects(ref drawColor);
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        for (int i = 0; i < 8; i++)
        {
            Terraria.Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.BloodWater);

        }

        if (hit.Damage > 15)
        {
            SoundEngine.PlaySound(SoundID.NPCHit9);
            NPC.NewNPCDirect(NPC.GetSource_OnHurt(null), NPC.Center, ModContent.NPCType<FleshBall>());
        }
    }
}

public class FleshBall : ModNPC
{
    public override void SetDefaults()
    {
        
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.noGravity = true;
        NPC.lifeMax = 20;
        NPC.damage = 35;
        NPC.aiStyle = -1;
        NPC.Size = new Vector2(16, 16);
        base.SetDefaults();
    }

    public override void OnSpawn(IEntitySource source)
    {
        NPC.velocity = Main.rand.NextVector2CircularEdge(8f, 3f);
        NPC.velocity.Y = -MathF.Abs(NPC.velocity.Y);
        NPC.immuneTime = 30;
    }

    public override void AI()
    {
        NPC.velocity *= 0.95f;
        if (NPC.collideX || NPC.collideY)
        {
            NPC.velocity = Main.rand.NextVector2CircularEdge(6f, 6f);
        }
        base.AI();
    }
    
    public override void DrawEffects(ref Color drawColor)
    {
        if (Main.rand.NextBool(3))
        {
            Terraria.Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.BloodWater);
        }

        base.DrawEffects(ref drawColor);
    }
}