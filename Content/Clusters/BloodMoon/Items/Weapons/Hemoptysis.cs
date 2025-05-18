using System.Collections.Generic;
using HarmonyMod.Asset;
using HarmonyMod.Content.Dust;
using HarmonyMod.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.BloodMoon.Items.Weapons;

public class Hemoptysis : ModItem
{
    public override string Texture => AssetDirectory.Placeholders + "GenericItem";
    
    public override void SetDefaults()
    {
        Item.Size = new Vector2(26, 26);
        Item.DamageType = DamageClass.Magic;
        Item.damage = 25;
        Item.useTime = 60;
        Item.useAnimation = 60;
        Item.channel = true;
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.shoot = ModContent.ProjectileType<HemoptysisSneeze>();
        Item.shootSpeed = 0.1f;
        Item.useStyle = ItemUseStyleID.HiddenAnimation;
        base.SetDefaults();
    }


    public override Color? GetAlpha(Color lightColor)
    {
        return Color.PaleVioletRed;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient<BloodPathogen>(12);
        recipe.AddTile(TileID.Bookcases);
        recipe.Register();
    }
}

public class HemoptysisSneeze : ModProjectile
{
    private AnimationHandler _animationHandler;

    private static Asset<Texture2D> burstsheet =
        ModContent.Request<Texture2D>("HarmonyMod/Content/Clusters/BloodMoon/Items/Weapons/HemoptysisBurst");
    public override string Texture => AssetDirectory.Placeholders + "GenericItem";

    public override void SetDefaults()
    {
        _animationHandler = new AnimationHandler(burstsheet).AddAnimation(new Rectangle(0, 0, 30, 30), 6, 2);
        Projectile.hide = true;
        Projectile.Size = new Vector2(60, 60);
        Projectile.timeLeft = 200;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        base.SetDefaults();
    }

    public override void AI()
    {
        Projectile.friendly = false;
            Player owner = Main.player[Projectile.owner];

        Projectile.rotation = Projectile.velocity.ToRotation();
        if (Main.myPlayer == Projectile.owner)
        {
            Projectile.rotation = owner.Center.DirectionTo(Main.MouseWorld).ToRotation();
        }
        Projectile.Center = owner.Center + new Vector2(Projectile.Size.X / 2 , 0).RotatedBy(Projectile.rotation) + new Vector2(0, -12f);
        if (owner.channel)
        {
            if (Main.rand.NextBool(1 + (int)(60 - Projectile.ai[0]) / 4))
            {
                Terraria.Dust.NewDustDirect(owner.position, owner.width, owner.height, ModContent.DustType<HemoptysisDust>(), 0,
                    0.7f, 100, Color.Red).velocity.X = 0.01f;
            }

            Projectile.ai[0]++;
            if (Projectile.ai[0] == 60 - 12)
            {
                owner.velocity.Y = 2.5f;
                _animationHandler.PlayAnimation(0);
            }
            
            if (Projectile.ai[0] == 55)
            {
                SoundEngine.PlaySound(Assets.BloodSneeze.WithPitchOffset(0.7f), Projectile.Center);
                

                DustEmitter.Emit(DustID.Blood, Projectile.position, Projectile.width, Projectile.height, 15).ForEach((
                    dust =>
                    {
                        dust.velocity = new Vector2(1 * owner.direction, -1f);
                    }));
                for (int i = 0; i < 6; i++)
                {
                    var d2 = Terraria.Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                        ModContent.DustType<HemoptysisDust>(), owner.direction * 1.5f, 0f, 100,
                        new Color(110, 10, 10, 255), 1.2f);
                    d2.velocity *= 0.3f;
                }
                
                for (int i = 0; i < 8; i++)
                {
                    var d2 = Terraria.Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                        ModContent.DustType<Lightspot>(), owner.direction, 0f, 100,
                        Color.Red * 1f, 1.2f);
                    d2.velocity *= 0.3f;
                }
                Projectile.friendly = true;

            } 
            else if (Projectile.ai[0] > 60)
            {
                Projectile.Kill();
            }
        } else Projectile.Kill();

        if (Projectile.ai[0] >= 55)
        {
            
            owner.eyeHelper.BlinkBecausePlayerGotHurt();
            
        }
        else if (Projectile.ai[0] % 8 == 0)
        {
            SoundEngine.PlaySound(SoundID.Item7.WithPitchOffset(2f + ((60f - Projectile.timeLeft) / 60f)).WithVolumeScale(4f), Projectile.Center);
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        if (Projectile.ai[0] > 60 - 10)
        {
            PixelationCanvas.AddAdditiveDrawAction(() =>
            {
                _animationHandler.Draw((Projectile.Center - Main.screenPosition) / 2, Projectile.rotation, Color.Red * 0.7f, Vector2.One);
            });
        }
        return false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(ModContent.BuffType<SanguineCurse>(), 60*6);
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers,
        List<int> overWiresUI)
    {
        overPlayers.Add(index);
    }
}