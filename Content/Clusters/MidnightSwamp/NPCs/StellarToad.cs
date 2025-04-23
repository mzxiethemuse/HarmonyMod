using System.Collections.Generic;
using System.Linq;
using HarmonyMod.Content.Clusters.MidnightSwamp.Accessories;
using HarmonyMod.Content.Clusters.MidnightSwamp.Materials;
using HarmonyMod.Content.Clusters.MidnightSwamp.Projectiles;
using HarmonyMod.Content.Dust;
using HarmonyMod.Core.Graphics;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.MidnightSwamp.NPCs;

public class StellarToad : ComplexNPC
{
    public static Dictionary<int, Transmutoadtion> TransmutoadtionTable = new Dictionary<int, Transmutoadtion>()
    {
        { ItemID.PhilosophersStone, new Transmutoadtion(ModContent.ItemType<FusionBezoar>())},
        {ModContent.ItemType<Starglob>(), new Transmutoadtion(ModContent.ItemType<TransmutationEssence>(), false,
            () => { return Main.hardMode;})}
    };
    
    public int StoredItem = -1;
    private bool wasJustInWater;
    
    public override void SetStaticDefaults()
    {
        NPCID.Sets.TrailingMode[Type] = 2;
        NPCID.Sets.TrailCacheLength[Type] = 24;
        base.SetStaticDefaults();
    }
    public override void SetDefaults()
    {
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.lifeMax = 140;
        NPC.friendly = false;
        NPC.width = 56;
        NPC.height = 56;
        NPC.damage = 7;
        NPC.noGravity = true;
        NPC.scale = 0.6f;
        Acceleration = 0.05f;
        JumpHeight = 4f;
        
        base.SetDefaults();
    }

    public override void AI()
    {
        if (NPC.Center.Distance(GetTarget().Center) > 2000f)
        {
            NPC.EncourageDespawn(60);

        }

        if (Main.rand.NextBool(180))
        {
            NPC.NewNPCDirect(NPC.GetSource_FromAI(), NPC.Center + Main.rand.NextVector2Circular(300f, 300f),
                ModContent.NPCType<MidnightFly>());
        }
        Timer++;
        if (GoHere == Vector2.Zero) GoHere = NPC.Center;
        
        switch (State)
        {
            case 0:
            {
                Walk(GoHere, 1.2f, false, 40f);
                // make it so that the frog will take slightly quicker to start moving again once it stops
                if (DistanceOnX(NPC.Center, GoHere) < 40f)
                {
                    Timer++;
                }
                // Yum! Flies
                if (Main.rand.NextBool(4) && Timer % 60 == 0 && Timer != 0 && AI3 == 0)
                {
                    if (Main.rand.NextBool(3))
                    {
                        foreach (var item in Main.item.Where((item => item.Distance(NPC.Center) < 80f && item.active && item.stack == 1)))
                        {
                            SoundEngine.PlaySound(SoundID.Zombie13, NPC.Center);
                            SoundEngine.PlaySound(SoundID.AbigailSummon, NPC.Center);

                            DustEmitter.Emit(DustID.ManaRegeneration, item.position, item.width, item.height, 8);
                            item.active = false;
                            AI3 = 1;
                            StoredItem = ModContent.ItemType<Starglob>();
                            if (TransmutoadtionTable.ContainsKey(item.type))
                            {
                                if (TransmutoadtionTable[item.type].condition() || TransmutoadtionTable[item.type].alwaysAvailable)
                                {
                                    StoredItem = TransmutoadtionTable[item.type].output;
                                }
                            }
                            break;
                        }
                    } else foreach (var fly in Main.npc.Where((npc => npc.type == ModContent.NPCType<MidnightFly>() && npc.active)))
                    {
                        if (fly.Distance(NPC.Center) < 120f)
                        {
                            SoundEngine.PlaySound(SoundID.Zombie13, NPC.Center);
                            SoundEngine.PlaySound(SoundID.GlommerBounce, NPC.Center);
                            DustEmitter.Emit(DustID.ManaRegeneration, fly.position, fly.width, fly.height, 8);

                            fly.StrikeInstantKill();
                            AI3 = 1;
                            break;
                        }
                    }
                }
                
                
                if (Timer > (60 * 5) + AI2)
                {
                    Timer = 0;
                    GoHere = NPC.Center + Main.rand.NextVector2CircularEdge(Main.rand.Next(100, 300), 20f);
                    AI2 = Main.rand.Next(-120, 180);
                    if (Main.rand.NextBool(2))
                    {
                        AIHop();
                        State = 1;
                        Timer = 0;
                    }
                }

                break;
            }
            case 1:
            {
                if (OnGround()) Decelerate(0.2f);
                if (NPC.velocity.Y > 0 && AI3 == 1)
                {
                    if (StoredItem != -1)
                    {
                        SoundEngine.PlaySound(SoundID.GlommerBounce, NPC.Center);

                        var i = Item.NewItem(NPC.GetSource_FromAI(), NPC.position, NPC.Size, StoredItem, 1);
                        Main.item[i].velocity = new Vector2(Main.rand.Next(-2,2), -2);
                        StoredItem = -1;
                    }
                    else
                    {
                        NPC.TargetClosest();
                        // SoundEngine.PlaySound(SoundID.Item3.WithPitchOffset(0.3f), NPC.Center);
                        var projectile = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center,
                            NPC.Center.DirectionTo(GetTarget().Center) * 9, ModContent.ProjectileType<StarBurst>(), 24,
                            2f);
                        // projectile.friendly = false;
                        // projectile.hostile = true;}
                    }
                    AI3 = 0;

                }
                if (Timer > (120))
                {
                    State = 0;
                    Timer = 0;
                    GoHere = NPC.Center + Main.rand.NextVector2CircularEdge(Main.rand.Next(100, 200), 20f);
                    AI2 = Main.rand.Next(-120, 180);
                    
                }
            }
                break;
        }

        if (NPC.wet)
        {
            NPC.velocity.Y -= 0.12f;
            wasJustInWater = true;
        }
        else
        {
            if (wasJustInWater)
            {
                NPC.velocity.Y = -0.5f;

                wasJustInWater = false;
            }
            NPC.velocity.Y += 0.15f;

        }
        NPC.direction = NPC.velocity.X > 0 ? 1 : -1;
        NPC.spriteDirection = -NPC.direction;
        base.AI();
    }

    public void AIHop()
    {
        SoundEngine.PlaySound(SoundID.Zombie13, NPC.Center);
        NPC.velocity.X = 3 * NPC.direction;
        TryJump(7f);
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        GoHere = NPC.Center + Main.rand.NextVector2CircularEdge(Main.rand.Next(100, 300), 20f);
        base.HitEffect(hit);
    }

    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (AI3 != 0 && Main.rand.NextBool(12))
        {
            Terraria.Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.ManaRegeneration).velocity *= 0.01f;
        }
        // Lines.Line(GoHere, NPC.Center, Color.Red, 2);
        // spriteBatch.DrawString(FontAssets.MouseText.Value, "gohere", GoHere - screenPos, Color.White);

    }
    
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (State == 1)
        {
            Trails.DrawTrail(NPC.oldPos, NPC.oldRot, Color.Azure, NPC.Size, 1, 32, -2.8f, 1.7f, "LightDisc");
        }
        return true;
    }
}