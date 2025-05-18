using System;
using HarmonyMod.Asset;
using HarmonyMod.Content.Dust;
using HarmonyMod.Content.Projectiles;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.GoblinArmy.NPCs;

public class GoblinBrawler : ComplexNPC
{
    
    
    enum AIStates {
        Walk = 0,
        Dash = 3,
        Jump1 = 4,
        Jump2 = 5,
    }
    public override long CoinValue => Item.buyPrice(0, 0, 57, 1);

    public override void SetStaticDefaults()
    {
        NPCID.Sets.BelongsToInvasionGoblinArmy[Type] = true;
    }

    public override void SetDefaults()
    {
        NPC.width = 32;
        NPC.height = 46;
        

        NPC.lifeMax = 280;
        NPC.defense = 20;
        NPC.damage = 35;
        NPC.knockBackResist = 0.2f;
        Acceleration = 0.1f;
        NPC.HitSound = SoundID.NPCHit4;
        NPC.DeathSound = SoundID.NPCDeath1;

        Guarded = true;
    }

    public override void AI()
    {
        
        NPC.spriteDirection = NPC.direction;
        Timer++;
        AI2--;
        if (State == 0)
        {
            Guarded = true;
            NPC.TargetClosest(true);
            var playerPos = GetTarget().Center;
            Walk(playerPos, 1f, true, 50f);
            // if (NPC.Center.Distance(playerPos) < 50 && AI2 < 0)
            // {
            //     Timer = 0;
            //     State = 1;
            // }
            if (Main.rand.NextBool(2)) {
                if (Diff(NPC.Center.X, playerPos.X) < 500f && Diff(NPC.Center.Y, playerPos.Y) < 170f && Timer > 270 &&
                    Main.rand.NextBool(33))
                {
                    SoundEngine.PlaySound(SoundID.NPCHit40.WithPitchOffset(0.8f), NPC.Center);
                    NPC.TargetClosest();
                    Timer = 0;
                    State = (int)AIStates.Dash;
                    NPC.netUpdate = true;
                }

                if (Timer > 270 && Main.rand.NextBool(35))
                {
                    SoundEngine.PlaySound(SoundID.NPCHit40.WithPitchOffset(1.2f), NPC.Center);
                    NPC.TargetClosest();
                    TryJump(6f, 4f * (playerPos.X > NPC.Center.X ? 1 : -1));

                    Timer = 0;
                    State = (int)AIStates.Jump1;
                    NPC.netUpdate = true;
                }
            }
            
        } else if (State == 2)
        {
            if (Main.rand.NextBool(4) && !Guarded) Terraria.Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.TreasureSparkle).velocity *= 0.1f;
            
            Decelerate();
            if (Timer > 120)
            {
                Guarded = true;
                Timer = 0;
                State = 0;
            }
        } else if (State == (int)AIStates.Dash) // dash attack
        {

            Guarded = true;
            var playerPos = GetTarget().Center;
            Walk(playerPos, 6f, false, 50f);
            if (Timer > 50)
            {
                Timer = 0;
                Decelerate();
                if (Main.rand.NextBool(3))
                {
                    SoundEngine.PlaySound(SoundID.NPCHit40, NPC.Center);
                    TryJump(6f, 4f * (playerPos.X > NPC.Center.X ? 1 : -1));
                    
                    NPC.TargetClosest();
                    State = 4;
                }
                else
                {
                    State = 0;
                }
            }
        } else if (State == (int)AIStates.Jump1) // jump part 1
        {
            var playerPos = GetTarget().Center;

            if (Timer > 20)
            {
                //probably a better way to do this
                NPC.velocity.X *= 0.7f;
                NPC.velocity.Y *= 0.2f;
                if (Timer > 40)
                {
                    SoundEngine.PlaySound(SoundID.NPCHit40.WithPitchOffset(1.6f), NPC.Center);

                    NPC.velocity = new Vector2(4f * (playerPos.X > NPC.Center.X ? 1 : -1), 8f);
                    Timer = 0;
                    State = 5;
                }
            }
        } else if (State == (int)AIStates.Jump2) // jump part 2
        {
            if (NPC.collideY)
            {
                SoundEngine.PlaySound(SoundID.DeerclopsRubbleAttack.WithPitchOffset(1.2f), NPC.Center);
                Burst.SpawnBurst(Assets.VFXSmoke[3], NPC.Center, Color.DimGray * 0.4f, 100f, 30);

                Hitbox.SpawnHitbox(NPC.GetSource_FromAI(), NPC.Center, 90, 20, 25,20, NPC.whoAmI, false, true);
                for (int i = -9; i < 9; i++)
                {
                    Burst.SpawnBurst(Assets.VFXSmoke[Main.rand.Next(0, 4)], NPC.Center + new Vector2(10 * i, Main.rand.Next(-20, 20)), Color.DarkGray * 0.4f, 20f + Main.rand.Next(-10, 11), 30);

                }

                Timer = 70;
                State = 2;
                
            }
            if (Timer > 20)
            {
                Timer = 60;
                State = 2;
            }
        }
    }

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
    {
        modifiers.Knockback += 0.5f;
        base.ModifyHitPlayer(target, ref modifiers);
    }

    // public override bool OnParried()
    // {
    //     NPC.defense -= 5;
    //     Timer = 0;
    //     State = 2;
    //     if (State == 3)
    //     {
    //         NPC.life -= 30;
    //         NPC.checkDead();
    //         NPC.netUpdate = true;
    //         Timer -= 60;
    //     }
    //     return base.OnParried();
    // }

    // public override bool CanHitPlayer(Player target, ref int cooldownSlot)
    // {
    //     return Guarded;
    // }
    
}