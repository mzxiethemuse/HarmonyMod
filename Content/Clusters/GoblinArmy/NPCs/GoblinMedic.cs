using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyMod.Asset;
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

namespace HarmonyMod.Content.Clusters.GoblinArmy.NPCs;

public class GoblinMedic : ComplexNPC
{
    public override long CoinValue => Item.buyPrice(0, 0, 14, 1);

    public override void SetStaticDefaults()
    {
        NPCID.Sets.BelongsToInvasionGoblinArmy[Type] = true;
    }

    public override void SetDefaults()
    {
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;

        NPC.lifeMax = 90;
        NPC.defense = 10;
        NPC.width = 28;
        NPC.height = 44;
        NPC.damage = 0;
        base.SetDefaults();
    }

    public override void AI()
    {
        Timer++;
        var deadGoblins = GlobalGoblin.deadGoblins;
        if (GoHere == Vector2.Zero)
        {
            LookForDeaduns();
        }

        if (deadGoblins.Count == 0 && State != 3)
        {
            State = 3;
            Timer = 0;
        }

        if (State == 0)
        {
            Walk(GoHere, 2f, true, 20f);
            if (NPC.Distance(GoHere) < 30f)
            {
                Timer = 0;
                State = 1;
            }

            if (Timer > 60 * 10)
            {
                LookForDeaduns();
                // i cant fucking reach ts cro
            }

            NPC.direction = NPC.velocity.X > 0 ? 1 : -1;
            NPC.spriteDirection = NPC.direction;
        }
        else if (State == 1)
        {
            if (Timer % 10 == 0)
            {
                Terraria.Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.HealingPlus).velocity *= 0.3f;
            }

            Decelerate();
            if (Timer > 120)
            {
                for (int i = 0; i < 30; i++)
                {
                    Terraria.Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.HealingPlus).velocity.Y *=
                        0.3f;
                }

                Timer = 0;
                State = 0;
                deadGoblins.RemoveAt((int)AI2);
                NPC.NewNPCDirect(NPC.GetSource_FromAI(), GoHere, (int)AI3).life = 30;

                LookForDeaduns();
            }
        }
        else if (State == 3)
        {
            Walk(GoHere, 2f, true, 20f);

            if (Timer > (60 * 10) || GoHere == Vector2.Zero || NPC.Distance(GoHere) < 30f || deadGoblins.Count != 0)
            {
                GoHere = NPC.Center + Main.rand.NextVector2Circular(800, 100);
                Timer = 0;
                LookForDeaduns();
            }
        }
    }

    // public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    // {
    //     Lines.Line(GoHere, NPC.Center, Color.Red, 2);
    // }

    private void LookForDeaduns()
    {
        var deadGoblins = GlobalGoblin.deadGoblins;

        foreach (var goblin in deadGoblins)
        {
            if (goblin.position.Distance(NPC.Center) < 900f)
            {
                Timer = 0;
                State = 0;
                GoHere = goblin.position;
                AI3 = goblin.type;
                AI2 = deadGoblins.IndexOf(goblin);
            }
        }
    }
}

public class DeadGoblinSystem : ModSystem
{
    public override void PostUpdateInvasions()
    {
        List<DeadGoblin> removeMe = new List<DeadGoblin>();
        if (Main.invasionType == 1)
        {
            foreach (var goblin in GlobalGoblin.deadGoblins)
            {
                goblin.time++;
                if (goblin.time > 780)
                {
                    removeMe.Add(goblin);
                }

                PixelationCanvas.AddAdditiveDrawAction((() =>
                {
                    Main.spriteBatch.Draw(Assets.PlaceholderCube.Value,
                        (goblin.position - Main.screenPosition) / 2 +
                        new Vector2(0, (float)Math.Sin(Main.timeForVisualEffects / 8)), null, Color.White * 0.5f, 0f,
                        Assets.PlaceholderCube.Size() / 2, 0.3f, SpriteEffects.None, 0f);
                }));
            }

            foreach (var remove in removeMe)
            {
                GlobalGoblin.deadGoblins.Remove(remove);
            }
        }

        base.PostUpdateInvasions();
    }
}