using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyMod.Content.Dusts;
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

namespace HarmonyMod.Content.Clusters.GoblinArmy.NPCs;

public class GoblinEngineer : ComplexNPC
{
    public override long CoinValue => Item.buyPrice(0, 0, 36, 1);

    private const float SeekGoblinState = 0;
    private const float BuildState = 1;
    private const float WalkToPointState = 2;
    public static DustEmitter buildSmoke = new DustEmitter(ModContent.DustType<FancySmoke>());
    
    public static bool dontFleeForDebug = true;
    private static int PalisadeID = ModContent.NPCType<Palisade>();
    private float distanceBeforeWalk;
    private bool shouldMakePalisade = true;

    public override void SetStaticDefaults()
    {
        buildSmoke.SetScaleRange(0.5f, 1.5f);
        buildSmoke.SetVelocitySpread(new Vector2(0.5f, 0.2f));
        buildSmoke.SetColors(new Color(104, 71, 46, 200) * 0.8f, new Color(104, 61, 36, 220) * 0.7f);
        buildSmoke.customData = 2;
        NPCID.Sets.BelongsToInvasionGoblinArmy[Type] = true;
        base.SetStaticDefaults();

    }

    public override void SetDefaults()
    {
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.lifeMax = 100;
        NPC.defense = 1;
        NPC.width = 28;
        NPC.height = 44;
        NPC.damage = 7;
        base.SetDefaults();
    }

    public override void AI()
    {
        NPC.TargetClosest(false);
        var playerPos = GetTarget().Center;

        Timer++;
        if ((Main.invasionType != 1 && !dontFleeForDebug) || playerPos.Distance(NPC.Center) < 80f)
        {
            var a = playerPos.X > NPC.position.X ? -1 : 1;
            NPC.direction = -a;

            Walk(
                playerPos + new Vector2(2000, 0) * a,
                4.7f, true, 40f
                );
            return;
            
        }
        
        if (State == SeekGoblinState)
        {
            var distanceTraveled = Diff(DistanceOnX(NPC.oldPosition + NPC.Size / 2, GoHere), DistanceOnX(NPC.Center, GoHere));
            if (distanceTraveled == 0)
            {
                Timer += 4;
            }
            
            /// make sure the target pos is set
            if (GoHere == Vector2.Zero) SetWalkTargetToGoblin();
            /// every ~8 seconds
            if (Timer > 60 * 4)
            {
                // update the position
                Timer = 0;
                SetWalkTargetToGoblin();
                //PickNewRandomWalkTarget();
            }

            if (DistanceOnX(NPC.Center, GoHere) < 30f)
            {
                if (shouldMakePalisade)
                {
                    Timer = 0;
                    State = 1;
                }
                else
                {
                    Timer = 0;
                    PickNewRandomWalkTarget();
                    State = 2;
                }
            }
            Walk(GoHere, 3f, false, 30f);
        } else if (State == BuildState) {
            Decelerate();
            if (Timer % 20 == 0) 
            {
                SoundEngine.PlaySound(SoundID.Item37);
                foreach (Terraria.Dust dust in buildSmoke.Emit(NPC.position, NPC.width, NPC.height, 5))
                {
                    dust.customData = Main.rand.Next(1, 6);
                }
            }
            if (Timer >= 60)
            {
                foreach (Terraria.Dust dust in buildSmoke.Emit(NPC.position, NPC.width, NPC.height, 20))
                {
                    dust.customData = Main.rand.Next(1, 6);
                }


                Timer = 0;
                PickNewRandomWalkTarget();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    var palisade = NPC.NewNPCDirect(NPC.GetSource_FromAI("GoblinEngineer"), (int)NPC.Center.X,
                        (int)NPC.Center.Y, PalisadeID);
                    palisade.direction = NPC.direction;
                    palisade.spriteDirection = NPC.direction;
                }
                State = 2;
            }
        } else if (State == WalkToPointState)
        {
            if (Timer > 60 * 2 || DistanceOnX(NPC.Center, GoHere) < 30f)
            {
                SetWalkTargetToGoblin();
                Timer = 0;
                State = 0;
            }
            Walk(GoHere, 3f, true, 30f);
        }
        NPC.direction = NPC.velocity.X > 0 ? 1 : -1;
        NPC.spriteDirection = NPC.direction;

    }

    private int SetWalkTargetToGoblin()
    {
        var MostPopularGoblinAward = LookForGoblinGroups(NPCID.GoblinArcher, 1000f, 300f);
        if (MostPopularGoblinAward != -255)
        {
            var popularGoblin = Main.npc[MostPopularGoblinAward];
            var popularGoblinTarget = Main.player[Main.npc[MostPopularGoblinAward].target];
            GoHere = popularGoblin.Center + new Vector2(Main.rand.NextFloat(50,150) * (popularGoblinTarget.Center.X > popularGoblin.Center.X ? 1 : -1), 0f);

        }
        else
        {
            PickNewRandomWalkTarget();
        }

        return MostPopularGoblinAward;
    }
    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        // Lines.Line(GoHere, NPC.Center, Color.Red, 2);
        // spriteBatch.DrawString(FontAssets.MouseText.Value, "gohere", GoHere - screenPos, Color.White);
        // spriteBatch.DrawString(FontAssets.MouseText.Value, NPC.velocity.ToString(), NPC.position - screenPos + new Vector2(0, 30), Color.White);

    }

    

    private void PickNewRandomWalkTarget()
    {
        var iJerkOffToFurryPorn = 700;
        GoHere = NPC.Center + Main.rand.NextVector2Circular(iJerkOffToFurryPorn, 100);
        while (DistanceOnX(NPC.Center, GoHere) < 200f)
        {
            GoHere = NPC.Center + Main.rand.NextVector2Circular(iJerkOffToFurryPorn, 100);
        }
    }

    /// <summary>
    /// finds the goblin within the specified radius that has the most amount of goblins around it, keeping in mind to exclude goblins that are near palisades. if there are no goblins not near palisades found, it returns a random goblin within the radius
    /// </summary>
    /// <param name="type"></param>
    /// <param name="radius"></param>
    /// <param name="groupRadius"></param>
    /// <returns></returns>
    private int LookForGoblinGroups(int type, float radius, float groupRadius)
    {
        /// setup
        shouldMakePalisade = false;
        List<int> gobbos = new List<int>();
        // Main.NewText("Looking for goblins");

        //loop thru every npc
        foreach (var npc in Main.npc)
        {
   
            if (npc.type == type && npc.Center.Distance(NPC.Center) < radius && npc.active)
            {
                // go thru every npc and check to make sure that if its a palisade, it's not near the gobbo 
                var serviced = false;
                foreach (var npc2 in Main.npc)
                {
                    if (npc2.type == PalisadeID && npc2.active && npc2.Center.Distance(npc.Center) < radius * 0.5f) 
                    {
                        serviced = true;
                    }
                }
                
                // if no palisades found near gobbo, add it to the list
                if (!serviced)
                {
                    // Main.NewText("Found an unserviced goblin! Let's make a palisade");
                    shouldMakePalisade = true;
                    gobbos.Add(npc.whoAmI);
                }
            }
        }

        // if no goblins far enough from palisades are found...
        
        if (gobbos.Count == 0)
        {
            // Main.NewText("No unserviced goblins found, choosing a random one" + shouldMakePalisade);

            shouldMakePalisade = false;
            // check again, without checking for palisades
            foreach (var npc in Main.npc)
            {
                if (npc.type == type && npc.Center.Distance(NPC.Center) < radius && npc.active)
                {
                    // Main.NewText("Goblin Found.");
                    gobbos.Add(npc.whoAmI);
                }
            }

            // if there are really no goblins nearby, return an impossible npc index (checked in WalkToGoblin or whatever)
            if (gobbos.Count == 0)
            {
                // Main.NewText("There are no goblins.");
                return -255;
            } 
            
            var thingy = gobbos[Main.rand.Next(0, gobbos.Count)];
            
            
            //return a random one
            return gobbos[Main.rand.Next(0, gobbos.Count)];
        }
        
        var gobboArray = gobbos.ToArray();
        gobbos = null;
        var groupSizes = new int[gobboArray.Length];
        for (int i = 0; i < gobboArray.Length - 1; i++)
        {
            var currentGob = gobboArray[i];
            foreach (var gob in gobboArray)
            {
                if (Main.npc[gob].Center.Distance(Main.npc[currentGob].Center) < groupRadius && Main.npc[gob].active)
                {
                    groupSizes[i]++;
                }
            }
        }

        return gobboArray[Array.IndexOf(groupSizes, groupSizes.Max())];


    }
    
}