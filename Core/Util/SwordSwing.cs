using System;
using System.Collections.Generic;
using HarmonyMod.Core.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Core.Util;

public enum SwordSwingType
{
    Basic = 1,
    Dueling = 2,
    Thrusting = 3,
    NoChangeSwingDirection = 4,
    
}

public abstract class SwordSwing : ModProjectile
{
    
    public virtual SwordSwingType SwingType => SwordSwingType.Basic;
    public virtual float Scale => 1f;
    public virtual int Lifetime => 20;
    public virtual int Width => 30;
    public virtual int Height => 40;
    public virtual Vector2 HoldOffset => Vector2.Zero;
    public virtual bool TwoHanded => false;
    public virtual bool Friendly => false;
    public virtual bool Hostile => false;
    
    public virtual float StartRotation => 0f;
    public virtual float EndRotation => MathF.PI / 2f;

    public virtual float VisualRotationOffset => 0f;
    
    public float startRotation = 0f;

    public float endRotation = MathF.PI / 2f;
    

    public virtual bool DrawTrail => false;
    public virtual TrailData Trail => new TrailData();
    
    private float a =>  GetLerpValue((Lifetime - Projectile.timeLeft) / (float)Lifetime);
    
    private Player? owner => Main.player[Projectile.owner];

    public override void SetDefaults()
    {
        Projectile.usesIDStaticNPCImmunity = true;
        Projectile.idStaticNPCHitCooldown = Lifetime - 2;
        startRotation = StartRotation;
        endRotation = EndRotation;
        Projectile.penetrate = -1;
        Projectile.friendly = Friendly;
        Projectile.hostile = Hostile;
        Projectile.tileCollide = false;
        
        Projectile.DamageType = DamageClass.Melee;
        
        Projectile.timeLeft = Lifetime;
        Projectile.width = Width;
        Projectile.height = Height;
        // Projectile.scale = Scale;
        
        base.SetDefaults();
    }
    
    

    public override void AI()
    {
        
        if (SwingType != SwordSwingType.Thrusting)
        {
            Player owner = Main.player[Projectile.owner];
            float a = GetLerpValue((Lifetime - Projectile.timeLeft) / (float)Lifetime);
            var flipVector = new Vector2(owner.direction, 1);

            Projectile.direction = owner.direction;
            Projectile.spriteDirection = owner.direction;
            //
            Projectile.rotation =
                MathHelper.Lerp(startRotation * Projectile.direction, endRotation * Projectile.direction, a);
            var origin = new Vector2(0, Projectile.height);
            Projectile.Center = owner.Center;
            Projectile.Center +=
                ((new Vector2(Width / 2, Height / -2) + HoldOffset) * flipVector).RotatedBy(Projectile.rotation);
            // Projectile.Center -= Projectile.Size / 2;
            owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full,
                Projectile.rotation - ((MathF.PI / 2) + (MathF.PI / 4)) * owner.direction);
            if (TwoHanded)
                owner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.ThreeQuarters,
                    Projectile.rotation - ((MathF.PI / 2) + (MathF.PI / 3)) * owner.direction);
            Projectile.rotation += VisualRotationOffset;

            base.AI();
        }
    }

    public virtual float GetLerpValue(float n)
    {
        return defaultLerpValue(n);
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers,
        List<int> overWiresUI)
    {
        overPlayers.Add(index);
        base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
    }
    
    public override bool PreDraw(ref Color lightColor)
    {
        
        if (this.DrawTrail)
        {
            float[] rotations = new float[Projectile.oldPos.Length];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                var a = (float)i / (float)Projectile.oldPos.Length;
                rotations[i] = MathHelper.Lerp(startRotation * Projectile.direction, endRotation * Projectile.direction, a);
            }
            Trails.DrawTrail(Projectile.oldPos, rotations, Projectile.Size,
                Trail.ColorFunction, Trail.WidthFunction,
                -2.8f, 1.7f, Trail.Shader);
        }

        return true;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (SwingType == SwordSwingType.Dueling)
        {
            (startRotation, endRotation) = (MathHelper.Lerp(startRotation, endRotation, a), startRotation);
            Projectile.damage = 0;
        }
    }

    public override void OnSpawn(IEntitySource source)
    {
        if (owner != null)
        {
            SwingPlayer swings = owner.GetModPlayer<SwingPlayer>();
            swings.swing++;
            if (SwingType == SwordSwingType.Basic || SwingType == SwordSwingType.Dueling)
            {
                if (swings.swing > 1)
                {
                    swings.swing = 0;
                } else if (swings.swing == 0)
                {
                    (startRotation, endRotation) = (endRotation, startRotation);
                }
            }
        }
        base.OnSpawn(source);
    }

    float defaultLerpValue(float a) => Easing.OutQuint(a);
}

public class SwingPlayer : ModPlayer
{
    public int swing = 0;
    public int oldHeldItemID = -1;
    public override void UpdateEquips()
    {
        if (oldHeldItemID != Player.HeldItem.type)
        {
            swing = 0;
        }
        oldHeldItemID = Player.HeldItem.type;
        base.UpdateEquips();
    }
}
