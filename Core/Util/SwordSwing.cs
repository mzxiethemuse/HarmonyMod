using System;
using System.Collections.Generic;
using HarmonyMod.Core.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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

    protected float a => GetLerpValue((Lifetime - Projectile.timeLeft) / (float)Lifetime);

    protected Player? owner => Main.player[Projectile.owner];
    public virtual float Distance => 10f;

    public override void SetDefaults()
    {
        Projectile.usesIDStaticNPCImmunity = true;
        Projectile.idStaticNPCHitCooldown = Lifetime;
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
        else
        {
            float a = GetLerpValue((Lifetime - Projectile.timeLeft) / (float)Lifetime);
            
            Projectile.direction = owner.direction;
            Projectile.spriteDirection = owner.direction;
            Projectile.Center = owner.Center;
            Projectile.Center += Projectile.velocity * Distance * a;
            float r = (new Vector2(MathF.Abs(Projectile.velocity.X), Projectile.velocity.Y)).ToRotation();

            Projectile.rotation = r * Projectile.spriteDirection;
            Projectile.Center -= Projectile.velocity;
            
        }
    }

    public virtual float GetLerpValue(float n)
    {
        return defaultLerpValue(n);
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs,
        List<int> behindProjectiles, List<int> overPlayers,
        List<int> overWiresUI)
    {
        overPlayers.Add(index);
        base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
    }

    public override bool PreDraw(ref Color lightColor)
    {

        if (DrawTrail)
        {
            Trails.DrawTrail(Projectile.oldPos, Projectile.oldRot, Projectile.Size,
                (progress => Trail.ColorFunction(progress) * LerpUtils.RectSin(a)), Trail.WidthFunction,
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
            swings.comboSwing++;
            swings.swing++;
            if (SwingType == SwordSwingType.Basic || SwingType == SwordSwingType.Dueling)
            {
                if (swings.swing > 1)
                {
                    swings.swing = 0;
                }
                else if (swings.swing == 0)
                {
                    (startRotation, endRotation) = (endRotation, startRotation);
                }
            } else if (SwingType == SwordSwingType.Thrusting)
            {
                if (Main.myPlayer == owner.whoAmI)
                {
                    Projectile.velocity = owner.Center.DirectionTo(Main.MouseWorld);
                }
            }
            
            ComboLogic(ref swings.comboSwing);
        }

        base.OnSpawn(source);
    }

    public virtual void ComboLogic(ref int combo)
    {
        if (combo > 0)
        {
            combo = 0;
        }
    }

    public override void ModifyDamageHitbox(ref Rectangle hitbox)
    {
        if (SwingType == SwordSwingType.Thrusting)
        {
            float a = GetLerpValue((Lifetime - Projectile.timeLeft) / (float)Lifetime);
            
            //bandaid solution to a self-inflicted problem!
            Vector2 pos = owner.Center + Projectile.velocity * (Distance * a + Projectile.width) * 0.5f;
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, 8, 8);
            var rectangle = hitbox;
            // PixelationCanvas.AddAdditiveDrawAction((() =>
            // {
            //     Lines.Rectangle(new Rectangle(rectangle.X / 2, rectangle.Y / 2, rectangle.Width / 2, rectangle.Height / 2), Color.Red);
            // }));
        }
    }

    protected Vector2 CalculateOffsetBasicSwing(float progress)
    {
        Player owner = Main.player[Projectile.owner];
        return ((new Vector2(Width / 2, Height / -2) + HoldOffset) * new Vector2(owner.direction, 1)).RotatedBy(MathHelper.Lerp(startRotation * Projectile.direction, endRotation * Projectile.direction, progress));
    }

    float defaultLerpValue(float n) => Easing.OutQuint(n);
}

public class SwingPlayer : ModPlayer
{

    public int comboSwing = 0;
    public int swing = 0;
    public int oldHeldItemID = -1;
    public override void UpdateEquips()
    {
        if (oldHeldItemID != Player.HeldItem.type)
        {
            comboSwing = 0;
            swing = 0;
        }
        oldHeldItemID = Player.HeldItem.type;
    }
}

public class SwordSwingItem : GlobalItem
{

    public override bool? UseItem(Item item, Player player)
    {
        if (item.shoot != 0)
        {
            var tempProj = Projectile.NewProjectileDirect(player.GetSource_Misc("riposte"), Vector2.Zero, Vector2.Zero,
                item.shoot, 0, 0);
            if (tempProj.ModProjectile is SwordSwing swing)
            {
                if (swing.SwingType == SwordSwingType.Thrusting && player.HarmonyPlayer().TimeSinceLastHurt < 10)
                {
                    player.AddBuff(BuffID.ParryDamageBuff, 12);
                    SoundEngine.PlaySound(SoundID.Item10, player.Center);
                }
            }

            tempProj.Kill();
        }

        return null;
    }

}