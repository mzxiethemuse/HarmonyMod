using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Core.Util;

public abstract class SwordSwing : ModProjectile
{
    public virtual int lifeTime => 20;
    public virtual int width => 30;
    public virtual int height => 40;
    public virtual Vector2 offset => Vector2.Zero;
    public virtual bool twoHanded => false;
    public virtual bool friendly => false;
    public virtual bool hostile => false;
    
    public virtual float StartRotation => 0f;
    public virtual float EndRotation => MathF.PI / 2f;

    
    public float startRotation = 0f;

    public float endRotation = MathF.PI / 2f;

    public override void SetDefaults()
    {
        
        startRotation = StartRotation;
        endRotation = EndRotation;
        Projectile.penetrate = -1;
        Projectile.friendly = friendly;
        Projectile.hostile = hostile;
        Projectile.tileCollide = false;
        
        Projectile.DamageType = DamageClass.Melee;
        
        Projectile.timeLeft = lifeTime;
        Projectile.width = width;
        Projectile.height = height;
        
        
        base.SetDefaults();
    }
    
    

    public override void AI()
    {
        Player owner = Main.player[Projectile.owner];
        float a = GetLerpValue((lifeTime - Projectile.timeLeft) / (float)lifeTime);
        var flipVector = new Vector2(Projectile.direction, 1);

        Projectile.direction = owner.direction;
        Projectile.spriteDirection = owner.direction;
        //
        Projectile.rotation = MathHelper.Lerp(startRotation * Projectile.direction, endRotation * Projectile.direction, a);
        var origin = new Vector2(0, Projectile.height);
        Projectile.Center = owner.Center;
        Projectile.Center += new Vector2(width / 2 * owner.direction, height / -2).RotatedBy(Projectile.rotation);
        // Projectile.Center -= Projectile.Size / 2;
        owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full,  Projectile.rotation - ((MathF.PI / 2) + (MathF.PI / 4)) * owner.direction);
        if (twoHanded) owner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.ThreeQuarters,  Projectile.rotation - ((MathF.PI / 2) + (MathF.PI / 3)) * owner.direction);


        // Projectile.rotation = MathHelper.Lerp(startRotation, endRotation, a + (Projectile.direction == -1 ? -1f : 0));// + (2 * MathF.PI * (Projectile.direction == -1 ? -1f : 0));
        // Projectile.Center = owner.Center;
        // Projectile.Center += (offset * flipVector).RotatedBy(Projectile.rotation + (2 * MathF.PI *
        //     (Projectile.direction == -1 ? -1f : 0)));
        // Projectile.Center += (new Vector2(width / 2f, -height / 2f) * flipVector).RotatedBy(Projectile.rotation);
        base.AI();
    }

    public virtual float GetLerpValue(float n)
    {
        return n;
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers,
        List<int> overWiresUI)
    {
        overPlayers.Add(index);
        base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
    }
}

public class SwingPlayer : ModPlayer
{
    public int swing = 1;
}