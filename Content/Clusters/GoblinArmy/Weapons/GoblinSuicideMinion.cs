using System;
using System.Numerics;
using HarmonyMod.Asset;
using HarmonyMod.Content.Clusters.GoblinArmy.NPCs;
using HarmonyMod.Content.Dust;
using HarmonyMod.Content.Projectiles;
using HarmonyMod.Core.Graphics;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace HarmonyMod.Content.Clusters.GoblinArmy.Weapons;


public class GoblinSuicideStaff : ModItem
{
    public override string Texture => AssetDirectory.GoblinArmy + "Weapons/GoblinSuicideStaff";
    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.Swing;

        Item.damage = 22;
        Item.useTime = 40;
        Item.useAnimation = 40;
        Item.noMelee = true;
        Item.DamageType = DamageClass.Summon;
        Item.shoot = ModContent.ProjectileType<GoblinSuicideMinion>();
    }
    
    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
        position = Main.MouseWorld;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {

        var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
        projectile.originalDamage = Item.damage;
        return false;
    }
}

public class GoblinSuicideMinion : Minion
{
    // public override string Texture => AssetDirectory.Placeholders + "GenericItem";
    public static Asset<Texture2D> texture;
    protected override bool DealContactDamage => true;

    public override void Load()
    {
        texture = ModContent.Request<Texture2D>(AssetDirectory.GoblinArmy + "Weapons/GoblinSuicideMinion");
    }

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
    }
    public override void SetDefaults()
    {
        Projectile.width = 28;
        Projectile.height = 34;
        
        base.SetDefaults();
        State = 1;
    }

    public override bool PreAI()
    {
        if (State != 1) Projectile.spriteDirection = Owner.direction;
        Projectile.rotation = MathHelper.Lerp(Projectile.rotation, Projectile.velocity.X * 0.05f, 0.5f);
        return true;
    }

    public override void AttackNPC()
    {
        Projectile.spriteDirection = Projectile.velocity.X > 0 ? -1 : 1;
        switch (State)
        {
            case 0:
                Projectile.friendly = true;
                Fly(NpcTarget.Center, 0.1f, 8, 2);
                break;
            case 1:
            {
                Decelerate(0.2f);
                if (Timer > 600)
                {
                    Timer = 0;
                    State = 0;
                }

                break;
            }
            case 2:
            {
                GoToIdlePosition();
                if (Projectile.Center.Distance(GetIdlePosition()) < 20f)
                {
                    Timer = 0;
                    State = 0;
                }
                break;
            }
        }

    }

    public override void IdleUpdate()
    {
        if (State == 1)
        {
            Decelerate(0.2f);
            // Projectile.position.Y += MathF.Sin((float)Main.timeForVisualEffects) * 0.2f;
            if (Timer > 600)
            {
                Timer = 0;
                State = 0;
            }
        }
        else
        {
            GoToIdlePosition();
        }
    }

    public override Vector2 GetIdlePosition()
    {
        return Projectile.IdlePositionCircle(Owner);
    }

    public override void OnWhipped(Projectile whip)
    {

        if (State != 1) return;
        DustEmitter.Emit(DustID.SpectreStaff, Projectile.position, Projectile.width, Projectile.height, 32, Vector2.Zero, default, default, 2f, 3f);

        Timer = 0;
        State = 2;
    }

    // public override bool ShouldPerformTargetingLogic()
    // {
    //     return State == 1;
    // }

    public override bool? CanHitNPC(NPC target)
    {
        return (State == 0 && target.whoAmI == this.target);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (State == 0)
        {
            foreach (Terraria.Dust dust in GoblinSuicideBomber.smokeEmitter.Emit(Projectile.position, Projectile.width,
                         Projectile.height, 30))
            {
                dust.customData = Main.rand.Next(1, 6);
            }

            Burst.SpawnBurst(Assets.VFXCircle, Projectile.Center, Color.Red, 60, 50);
            CameraUtils.AddScreenshakeModifier(Projectile.Center, Vector2.UnitY, 3f, 2, 40);
            SoundEngine.PlaySound(SoundID.NPCDeath1);
            DustEmitter.Emit(DustID.Blood, Projectile.position, Projectile.width, Projectile.height, 32, Vector2.Zero,
                default, default, 2f, 3f);
            SoundEngine.PlaySound(SoundID.Item62);
            SoundEngine.PlaySound(SoundID.Item14);
            Burst.SpawnBurst(Assets.VFXSmoke[3], Projectile.Center, Color.OrangeRed * 0.4f, 30, 50);
            Hitbox.SpawnHitbox(Projectile.GetSource_OnHit(target), Projectile.Center, 80, 80, 40, 5, Projectile.owner,
                true);
            Projectile.velocity += Main.rand.NextVector2CircularEdge(10f, 10f);
            Timer = 0;
            State = 1;
        }
        base.OnHitNPC(target, hit, damageDone);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Trails.DrawTrail(Projectile.oldPos, Projectile.oldRot, Projectile.Size, Trails.BasicColorLerp(Color.Indigo * 0.4f, Color.Transparent), Trails.BasicWidthLerp(1f, 12f), -2.8f, 1f, "LightDisc");

        Vector2 offset = new Vector2(0, MathF.Sin((float)Main.timeForVisualEffects / 8) * 5f);
        var isDead = ((State == 1) ? 1 : 0);
        if (isDead == 0)
        {
            if (Main.rand.NextBool(3))
            {
                var dust = Terraria.Dust.NewDustPerfect(Projectile.Center + new Vector2(11 * -Projectile.spriteDirection, 10) + offset, DustID.Shadowflame, null, 220, default, 0.77f);
                dust.velocity *= 0.3f;
                dust.velocity.Y = 1.45f;
            }
        }
        else
        {
            if (Main.rand.NextBool(4)) Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0, 0, 245);
        }
        Rectangle frame = new Rectangle(0, Projectile.height * isDead, Projectile.width, Projectile.height);
        SpriteEffects effects = (Projectile.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        Main.spriteBatch.Draw(texture.Value, Projectile.position - Main.screenPosition + offset + Projectile.Size / 2, frame, lightColor * ((State == 1) ? 0.7f : 1f), Projectile.rotation, Projectile.Size / 2, Vector2.One, effects, 0f);
        return false;
    }

    public override Color? GetAlpha(Color lightColor)
    {
        return Color.White * ((State == 1) ? 0.6f : 1f);
    }
}