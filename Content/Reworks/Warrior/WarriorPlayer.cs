using System;
using System.Collections.Generic;
using HarmonyMod.Content.Clusters.GoblinArmy.Weapons;
using HarmonyMod.Content.Dust;
using HarmonyMod.Content.Dust.BurstDatas;
using HarmonyMod.Content.Projectiles;
using HarmonyMod.Content.Projectiles.Explosions;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Reworks.Warrior;

public class WarriorPlayer : ModPlayer
{
    public int parryCooldown;
    public int parryCooldownTimer;
    public int parryTime;
    public int parryWindow;
    public int parryRange;
    public const int perfectParryWindow = 3;
    

    public static Dictionary<int, Action<Player, Projectile>> parryActions = new();
    public override void PostUpdate()
    {
        parryCooldownTimer--;
        parryTime--;
        if (parryCooldownTimer == 0)
        {
            SoundEngine.PlaySound(SoundID.MaxMana.WithVolumeScale(0.5f).WithPitchOffset(0.8f));
            for (int i = 0; i < 4; i++)
            {
                Terraria.Dust.NewDust(Player.position, Player.width, Player.height, DustID.Copper, 0f, 0f, 0,
                    Color.WhiteSmoke, 0.2f);
            }
        }

    }

    public override void ResetEffects()
    {
        parryCooldown = 10; //60 * 3;
        parryWindow = 18; // 2-tenths of a second
        // parryTime = 0;
        // parryCooldownTimer = 0;
        parryRange = 80;
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (WarriorParry.parryKeybind.JustPressed)
        {
            if (parryCooldownTimer < 0)
            {
                OnParryKeyPressed();
            }
        }
    }

    public override void ModifyHurt(ref Player.HurtModifiers info)
    {
        if (parryTime > 0)
        {
            if (parryTime > parryWindow - perfectParryWindow)
            {
                SoundEngine.PlaySound(SoundID.Item88.WithPitchOffset(1.4f));
                info.FinalDamage /= 2;
                Burst.SpawnBurst("Explosion", Player.Center, Color.Orange, 40f, parryWindow * 2);

                // Explosion.SpawnExplosion<Explosion>(Player.Center, 0, 3f,
                //     Player.whoAmI, 64f, 34, Color.Orange, 20);
                
            }
            if (info.DamageSource.SourceNPCIndex != -1)
            {
                var npc = Main.npc[info.DamageSource.SourceNPCIndex];
                if (npc.ModNPC is ComplexNPC complexNpc)
                {
                    if (complexNpc.OnParried())
                    {
                        info.FinalDamage *= 0;
   
                    }
                }
                else
                {
                    npc.GetGlobalNPC<ParryNPC>().OnParried(npc);
                }
            }
            SoundEngine.PlaySound(SoundID.Research);
            info.FinalDamage /= 2;
            ParryVFX(Player);
        }
    }

    public override void OnHurt(Player.HurtInfo info)
    {
       
    }



    public void OnParryKeyPressed()
    {
        SoundEngine.PlaySound(SoundID.Item37.WithPitchOffset(0.25f));

        Burst.SpawnBurst(Player.Center, Color.Yellow, new FollowPlayerBurst("Explosion", parryWindow * 2, 30f, Player.whoAmI));

        // Explosion.SpawnExplosion<ParryExplosion>(Player.Center + Player.velocity * 10, 0, 0f, Player.whoAmI, 30f,
        //     Player.GetModPlayer<WarriorPlayer>().parryWindow * 2, Color.Yellow * 0.8f);
        parryCooldownTimer = parryCooldown;
        parryTime = parryWindow;
        foreach (Projectile proj in Main.projectile)
        {
            if (proj.Center.Distance(Player.Center) < parryRange)
            {
                Parry(proj);
            }
        }
    }

    public void Parry(Projectile projectile)
    {
        SoundEngine.PlaySound(SoundID.Research.WithPitchOffset(1.7f));
        // Burst.SpawnBurst(Player.Center, Color.Orange, new FollowPlayerBurst("Explosion", parryWindow * 2, 40f, Player.whoAmI));

        for (int i = 0; i < 7; i++)
        {
            // Explosion.SpawnExplosion<Explosion>(projectile.Center, projectile.damage / 10, 0f,
            //     Player.whoAmI, 24f, 32, Color.Yellow, 0);

            Terraria.Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.TreasureSparkle, 0f, 0f, 0, Color.White);
        }
        if (parryActions.ContainsKey(projectile.type) && Main.netMode != NetmodeID.MultiplayerClient)
        {

            parryActions[projectile.type](Player, projectile);
        }
        else
        {
            // if (Main.myPlayer == Player.whoAmI)
            // {
            //     projectile.velocity = Player.DirectionTo(Main.MouseWorld) * 20;
            //
            // }
        }
    }
    
    public static void ParryVFX(Player player)
    {
        Burst.SpawnBurst(player.Center, Color.Lerp(Color.Yellow, Color.Orange, 0.5f), new FollowPlayerBurst("Explosion", player.GetModPlayer<WarriorPlayer>().parryWindow * 2, 50f, player.whoAmI));

        // Burst.SpawnBurst("Explosion", player.Center, Color.Yellow, 50f, player.GetModPlayer<WarriorPlayer>().parryWindow * 2);
        // Explosion.SpawnExplosion<ParryExplosion>(player.Center + player.velocity * 10, 0, 0f, player.whoAmI, 40f,
        //     player.GetModPlayer<WarriorPlayer>().parryWindow * 2, Color.Yellow);
    }

public override void Unload()
    {
        parryActions.Clear();
    }

    // adding all lof the parryable projectile things
    public override void SetStaticDefaults()
    {
        foreach (int boomerang in ThrowingItem.throwingProjectiles)//(int[])[ProjectileID.WoodenBoomerang, ProjectileID.IceBoomerang, ProjectileID.Shroomerang, ProjectileID.Trimarang])
        {
            parryActions.Add(boomerang, ParryActions.PBoost);
            /*(player, projectile) =>
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    projectile.velocity = player.DirectionTo(Main.MouseWorld) * 20 + (player.velocity * 0.7f);
                    projectile.damage += 2;
                }
            }
        );*/
        }
        
        parryActions.Add(ProjectileID.Xenopopper,
            (player, projectile) =>
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    // Explosion.SpawnExplosion<Explosion>(projectile.Center, 0, 0f, projectile.owner, 32f, 17, Color.MediumPurple, 30, true);
                    
                    projectile.Kill();
                }
            }
        );
        parryActions.Add(ModContent.ProjectileType<CrudlockProjectile>(),
            (player, projectile) =>
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    projectile.velocity += player.DirectionTo(Main.MouseWorld) * 2 + (player.velocity * 0.7f);
                    projectile.CritChance += 20;

                }
            }
        );
    }
}