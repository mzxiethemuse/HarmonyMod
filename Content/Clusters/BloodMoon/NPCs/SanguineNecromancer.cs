using System;
using System.Linq;
using HarmonyMod.Content.Clusters.BloodMoon.Items;
using HarmonyMod.Content.Clusters.BloodMoon.Projectiles;
using HarmonyMod.Content.Dusts;
using HarmonyMod.Content.Dusts.BurstDatas;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace HarmonyMod.Content.Clusters.BloodMoon.NPCs;

public class SanguineNecromancer : ComplexNPC
{
    public override long CoinValue => Item.buyPrice(0, 5);

    public override void SetDefaults()
    {
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;

        Acceleration = 0.12f;
        JumpHeight = 5f;
        
        NPC.Size = new Vector2(30, 50);
        NPC.friendly = false;
        NPC.damage = 0;
        NPC.defense = 7;
        NPC.lifeMax = 300;
        base.SetDefaults();
    }

    public override void AI()
    {
        NPC.TargetClosestUpgraded();
        Timer++;
        if (State == 0)
        {
            Walk(GoHere, 0.8f, true, 100);
            if (Timer > 300 || GoHere.Distance(GetTarget().Center) > 700)
            {
                GoHere = GetTarget().Center + (NPC.Center.DirectionTo(GetTarget().Center) *
                                               new Vector2(Main.rand.Next(300, 450), 0.2f));
                Timer = 0;
            }
            if (Timer != 0 && Timer % 30 == 0)
            {
                // Decide what attack to use?
                bool foundAttack = false;
                while (!foundAttack)
                {
                    int choice = Main.rand.Next(1, 8);
                    /*
                     * attacks:
                     * 1 : zombie arm attack
                     * 2 : summon skeletons
                     * 3 : bloodthorns
                     * 4 : 
                     */
                    switch (choice)
                    {
                        case 1:
                            if (GetTarget().Distance(NPC.Center) < 400)
                            {
                                SoundEngine.PlaySound(SoundID.Item81, NPC.position);

                                //floorarm
                                Timer = 0;
                                State = 1;
                                foundAttack = true;
                            }
                            break;
                        case 2:
                            // zomboes
                            if (GetTarget().Distance(NPC.Center) > 500 && GetTarget().Distance(NPC.Center) < 1200)
                            {
                                Burst.SpawnBurst(Assets.Assets.VFXCircleBlurred, NPC.Center, Color.Red * 0.8f, 40f, 60);
                                Timer = 0;
                                State = 2;
                                foundAttack = true;
                            }
                            break;
                        case 3:
                        case 4:
                            if (GetTarget().Distance(NPC.Center) < 130)
                            {
                                State = 3;
                                Timer = 0;
                                foundAttack = true;
                            }
                            break;
                        default:
                            foundAttack = true;
                            break;
                    }
                }
            }
        }
        else if (State == 1)
        {
            if (Timer <= 60)
            {
                SoundEngine.PlaySound(SoundID.Item77.WithPitchOffset(1.1f), NPC.position);
                SoundEngine.PlaySound(SoundID.DD2_SkeletonSummoned, NPC.position);

                Decelerate();
                // 
                if (Timer != 0 && Timer % 12 == 0)
                {
                    Vector2 chosenPos = NPC.Center + new Vector2(65 * (Timer / 12) * NPC.direction, 0);
                    Projectile.NewProjectileDirect(
                        NPC.GetSource_FromAI(),
                        GetFloorAtX(chosenPos, 100f) + new Vector2(0, 25),
                        Vector2.Zero,
                        ModContent.ProjectileType<FloorArm>(),
                        15,
                        2f
                    );
                }
            }
            else if (Timer >= 80)
            {
                if (Timer == 80)
                {
                    GoHere = NPC.Center + (NPC.Center.DirectionTo(GetTarget().Center) * new Vector2(-100, 0f));
                }
                Walk(GoHere, 1.33f, true, 40);
                if (Timer > 160)
                {
                    GoHere = GetTarget().Center + (NPC.Center.DirectionTo(GetTarget().Center) *
                                                   new Vector2(Main.rand.Next(300, 450), 0.2f));
                    Timer = 0;
                    State = 0;
                }
            }
            

        }
        else if (State == 2) {
            Decelerate();
            if (Timer == 60)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 chosenPos = NPC.Center + new Vector2(40 * (i - 2), 0);
                    
                    NPC.NewNPCDirect(NPC.GetSource_FromAI(), GetFloorAtX(chosenPos, 100f) + new Vector2(0, -40), NPCID.DD2SkeletonT1).velocity.X = Main.rand.NextFloat(-4f, 4f);
                }
            }
            if (Timer > 180)
            {
                GoHere = GetTarget().Center + (NPC.Center.DirectionTo(GetTarget().Center) *
                                               new Vector2(Main.rand.Next(300, 450), 0.2f));
                Timer = 0;
                State = 0;
            }
        }
        else if (State == 3) {
            Decelerate(0.4f);
            if (Timer == 1)
            {
                SoundEngine.PlaySound(SoundID.NPCDeath1.WithPitchOffset(1.5f), NPC.position);

                Burst.SpawnBurst(NPC.Center, Color.Red * 0.5f, new MagicBurst(Assets.Assets.VFXCircleBlurred, 90, 60));

            };
            if (Timer == 90)
            {
                SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal, NPC.position);
                DustEmitter.Emit(DustID.Blood, NPC.position, NPC.width, NPC.height, 40);
                for (int i = 0; i < 6; i++)
                {

                    var p = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center + new Vector2(MathUtility.VaguelyNormalDist(-90, 90), NPC.height / 2), new Vector2(Main.rand.NextFloat(-0.7f, 0.7f), -1),
                        ProjectileID.SharpTears, 30, 8f);
                    p.friendly = false;
                    p.hostile = true;
                    p.penetrate = -1;
                    p.ai[1] = 1f;
                }
            }
            if (Timer > 120)
            {
                GoHere = GetTarget().Center + (NPC.Center.DirectionTo(GetTarget().Center) *
                                               new Vector2(Main.rand.Next(300, 450), 0.2f));
                Timer = 0;
                State = 0;
            }
        }

        if (MathF.Abs(NPC.velocity.X) > 0.05)
        {
            NPC.spriteDirection = NPC.velocity.X < 0 ? -1 : 1;
            NPC.direction = NPC.spriteDirection;
        }
        
        
        // Lighting.AddLight(NPC.Center, Color.Red.ToVector3());
    }


    public Vector2 GetFloorAtX(Vector2 position, float range)
    {
        position.Y -= range;
        for (int i = 0; i < range / 10; i++)
        {
            
            var tile = Framing.GetTileSafely(position.ToTileCoordinates());
            if (tile.HasUnactuatedTile && tile.BlockType == BlockType.Solid &&
                (Main.tileSolidTop[tile.TileType] || Main.tileSolid[tile.TileType]))
            {
                return position.ToTileCoordinates().ToWorldCoordinates(Vector2.Zero);
            }
            position.Y += range / 5;
        }

        return position;
    }

    public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        Timer += 10;
    }

    public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
    {
        Timer += 10;
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BloodPathogen>(), 5));
        base.ModifyNPCLoot(npcLoot);
    }
}