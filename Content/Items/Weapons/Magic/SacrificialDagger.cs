using System;
using System.Collections.Generic;
using System.Numerics;
using HarmonyMod.Assets;
using HarmonyMod.Content.Clusters.GoblinArmy.Weapons;
using HarmonyMod.Content.Dusts;
using HarmonyMod.Content.Dusts.BurstDatas;
using HarmonyMod.Content.Projectiles;
using HarmonyMod.Core.Graphics;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace HarmonyMod.Content.Items.Weapons.Magic;

public class SacrificialDagger : ModItem
{
    public override string Texture => AssetDirectory.Placeholders + "GenericItem";


    public override void SetDefaults()
    {
        Item.useTime = 25;
        Item.useAnimation = 25;
        Item.useStyle = ItemUseStyleID.Rapier;

        Item.shoot = ModContent.ProjectileType<SacrificialDaggerProjectile>();
        Item.shootSpeed = 1f;
        
        Item.DamageType = DamageClass.Magic;
        Item.damage = 2;
        Item.knockBack = 4f;
        
        Item.autoReuse = false;
        Item.noMelee = true; 
        Item.noUseGraphic = true;

        Item.reuseDelay = 24;
        
        Item.UseSound = SoundID.Item1.WithPitchOffset(1.2f);
    }

    
}

public class SacrificialDaggerProjectile : ModProjectile
{
    public override void SetStaticDefaults()
    {

        ProjectileID.Sets.TrailingMode[this.Type] = 2;
        ProjectileID.Sets.TrailCacheLength[this.Type] = 14;

        base.SetStaticDefaults();
    }

    public override void SetDefaults()

    {
        Projectile.timeLeft = 20;
        Projectile.width = 34;
        Projectile.height = 32;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.penetrate = -1;
        Projectile.friendly = true;
    }

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
        Projectile.velocity *= 0;
    }

    public override void AI()
    {
        Projectile.ai[0] += 1;
        var n = Projectile.ai[0] / 20;
        if (Projectile.direction == -1) n = 1 - n;
        Player owner = Main.player[Projectile.owner];
        var a = MathF.Sin(n * MathF.PI);
        var rotation = Projectile.rotation - MathHelper.ToRadians(45);
        Projectile.Center = owner.Center + (new Vector2 (0, MathHelper.Lerp(-5, 5,
            MathF.Sin(n * MathF.PI * 2))) + Vector2.Lerp(Vector2.Zero, new Vector2(60, 0), MathF.Sin(n * MathF.PI))).RotatedBy(rotation);
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        bool excised = false;
        for (int i = 0; i < target.buffType.Length; i++)
        {
            var time = target.buffTime[i];
            var type = target.buffType[i];
            // Main.NewText($"{type}, {time}");
            if (type != 0 && time > 0)
            {
                excised = true;
                // Main.NewText($"{type}, {time}");

                target.buffTime[i] = 0;
                modifiers.FinalDamage.Flat += (time / 30) * 2;
                if (Projectile.owner == Main.myPlayer)
                {
                    var player = Main.player[Projectile.owner];
                    player.statMana -= (time / 30) * 2;
                }

                if (ExciseActions.actions.ContainsKey(type))
                {
                    // Main.NewText(modifiers.FinalDamage.);
                    modifiers = ExciseActions.actions[type](target, modifiers);
                    // Main.NewText(modifiers.FinalDamage);

                }

            }

        }

        if (excised)
        {
            // DustEmitter.Emit(DustID.MagicMirror, target.position, target.width, target.height, 30, new Vector2(0.5f, 1.2f));
            // if (target.ModNPC is ComplexNPC complexNpc)
            // {
            //     complexNpc.OnParried();
            // }
            SoundEngine.PlaySound(SoundID.Item45, Projectile.position);
            SoundEngine.PlaySound(SoundID.Item74, Projectile.position);

        }

        Projectile.damage = 0;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Trails.DrawTrail(Projectile.oldPos, Projectile.oldRot, Projectile.Size, Trails.BasicColorLerp(Color.Black * 0.6f, Color.Black * 0.6f), Trails.BasicWidthLerp(8f, 18f), -2.8f, 1.7f, "FlameLash");

        Trails.DrawTrail(Projectile.oldPos, Projectile.oldRot, Projectile.Size, Trails.BasicColorLerp(Color.Red * 0.6f, Color.DarkRed * 0.6f), Trails.BasicWidthLerp(0f, 32f), -2.8f, 1.8f, "FlameLash");
        // Trails.DrawTrailPixelated(Projectile.oldPos, Projectile.oldRot, Color.Red, Projectile.Size, 2f, 12f, -2.8f, 2f, "RainbowRod");
        return true;
    }
}

public static class ExciseActions {
    
    //IDEAS
    // Blood Pact; debuff that heals player when npc is excised (this requires a special player check)
    // 
    public static Dictionary<int, Func<NPC, NPC.HitModifiers, NPC.HitModifiers>> actions = new()
    {
        {BuffID.OnFire, FireExcision},
        {BuffID.OnFire3, FireExcision},
        {BuffID.Frostburn, FrostExcision},
        {BuffID.Frostburn2, FrostExcision},
        {BuffID.Venom, VenomExcision},
        {BuffID.Poisoned, PoisonExcision},
        {BuffID.ShadowFlame, ShadowflameExcision},
        {BuffID.Ichor, IchorExcision},

    };

    public static NPC.HitModifiers FireExcision(NPC npc, NPC.HitModifiers hit)
    {
        DustEmitter.Emit(DustID.Torch, npc.position, npc.width, npc.height, 30, new Vector2(3f, 3f), default, default, 3f, 6f, null, true);
        hit.FinalDamage.Flat += 20f;
        // hit.FinalDamage *= 200f;
        
        Burst.SpawnBurst(npc.Center, Color.OrangeRed, new MagicBurst(Assets.Assets.VFXStar[2], 88, 182));
        for (int i = 0; i < Main.rand.Next(8, 14); i++)
        {
            var proj = Projectile.NewProjectileDirect(npc.GetSource_OnHurt(null),
                npc.position + new Vector2(npc.width / 2, -4),
                Main.rand.NextVector2Circular(6f, 6f), ProjectileID.MolotovFire, 7, 2f);
        }

        // Explosion.SpawnExplosion<Explosion>(npc.Center, 15, 0f, Main.myPlayer, 186f, 84, Color.OrangeRed, 30, true, 0.3f);
        DustEmitter.Emit(ModContent.DustType<FancySmoke>(), npc.position, npc.width, npc.height, 25, new Vector2(2f, 2f), Color.Black * 0.6f, Color.Black * 0.8f, 8f, 10f, 3);

        return hit;
    }
    
    public static NPC.HitModifiers FrostExcision(NPC npc, NPC.HitModifiers hit)
    {
        DustEmitter.Emit(DustID.Frost, npc.position, npc.width, npc.height, 35, new Vector2(5f, 7f), default, default, 1f, 2f, null, true);
        hit.FinalDamage.Flat += 25f;
        for (int i = 0; i < Main.rand.Next(2,8); i++)
        {
            var proj = Projectile.NewProjectileDirect(npc.GetSource_OnHurt(null), npc.position + new Vector2(npc.width / 2, -4),
                Main.rand.NextVector2Circular(6f, 6f), ProjectileID.FrostShard, 7, 2f);
            proj.hostile = false;
            proj.friendly = true;
            proj.penetrate = 3;
            
        }
        Burst.SpawnBurst(npc.Center, Color.CornflowerBlue * 0.5f, new MagicBurst(Assets.Assets.VFXStar[2], 94, 176));

        // Explosion.SpawnExplosion<MagicExplosion>(npc.Center, 0, 0f, Main.myPlayer, 176f, 94, Color.CornflowerBlue * 0.5f, 30, true, 0.3f);
        return hit;
    }
    
    public static NPC.HitModifiers VenomExcision(NPC npc, NPC.HitModifiers hit)
    {
        DustEmitter.Emit(DustID.VenomStaff, npc.position, npc.width, npc.height, 35, new Vector2(5f, 7f), default, default, 1f, 3f, null, true);
        DustEmitter.Emit(DustID.Venom, npc.position, npc.width, npc.height, 35, new Vector2(5f, 7f), default, default, 0.5f, 1f, null, true);
        hit.FinalDamage.Flat *= 0.5f;
        hit.FinalDamage.Flat += 20f;

        hit.Defense *= 0.5f;
        Burst.SpawnBurst(npc.Center, Color.Violet * 0.5f, new MagicBurst(Assets.Assets.VFXStar[2], 64, 196));

        // Explosion.SpawnExplosion<MagicExplosion>(npc.Center, 0, 0f, Main.myPlayer, 196f, 64, Color.Violet * 0.5f, 30, true, 0.3f);

        return hit;
    }
    
    public static NPC.HitModifiers ShadowflameExcision(NPC npc, NPC.HitModifiers hit)
    {
        DustEmitter.Emit(DustID.Shadowflame, npc.position, npc.width, npc.height, 45, new Vector2(2f, 2f), default, default, 1f, 3f, null, true);
        hit.FinalDamage.Flat += 10f;

        hit.Defense *= 0.5f;
        Burst.SpawnBurst(npc.Center, Color.DarkViolet * 0.5f, new BurstData(Assets.Assets.VFXCircle, 74, 136));
        Burst.SpawnBurst(npc.Center, Color.Purple * 0.5f, new MagicBurst(Assets.Assets.VFXStar[2], 64, 196));

        // Explosion.SpawnExplosion<Explosion>(npc.Center, 0, 0f, Main.myPlayer, 136f, 74, Color.DarkViolet * 0.5f, 30, true, 0.3f);
        // Explosion.SpawnExplosion<MagicExplosion>(npc.Center, 20, 0f, Main.myPlayer, 196f, 64, Color.Purple * 0.5f, 30, true, 0.3f);

        return hit;
    }
    
    public static NPC.HitModifiers PoisonExcision(NPC npc, NPC.HitModifiers hit)
    {
        DustEmitter.Emit(DustID.PoisonStaff, npc.position, npc.width, npc.height, 15, new Vector2(5f, 7f), default, default, 0.5f, 2f, null, true);
        hit.FinalDamage.Flat *= 0.86f;

        hit.FinalDamage.Flat += 4;
        Burst.SpawnBurst(npc.Center, Color.ForestGreen * 0.5f, new BurstData(Assets.Assets.VFXCircle, 44, 186));

        return hit;
    }
    
    public static NPC.HitModifiers IchorExcision(NPC npc, NPC.HitModifiers hit)
    {
        DustEmitter.Emit(DustID.Ichor, npc.position, npc.width, npc.height, 15, new Vector2(5f, 7f), default, default, 0.5f, 2f, null, true);
        // if (npc.ModNPC is ComplexNPC complexNpc)
        // {
        //     complexNpc.OnParried();
        // }
        Burst.SpawnBurst(npc.Center, Color.LightGoldenrodYellow, new BurstData(Assets.Assets.VFXCircle, 44, 186));
        SoundEngine.PlaySound(SoundID.Research);

        return hit;
    }
}