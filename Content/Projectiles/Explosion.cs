using HarmonyMod.Assets;
using HarmonyMod.Core.Graphics;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Projectiles;

public class Explosion : ModProjectile
{
    public override void SetDefaults()
    {
        base.SetDefaults();
    }

    public override string Texture => AssetDirectory.Glow + "Explosion";

    public static Texture2D explosionRing =
        ModContent.Request<Texture2D>(AssetDirectory.Glow + "Explosion").Value;
    public static Texture2D explosionRingBlurred =
        ModContent.Request<Texture2D>(AssetDirectory.Glow + "ExplosionBlurred").Value;

    public virtual Texture2D ExplosionTexture => ModContent.Request<Texture2D>(Texture).Value;
    

    public Color color = Color.White;
    public float hitBoxLife = 1f;
    
    public override void AI()
    {
        var time = (Projectile.ai[1] - Projectile.timeLeft);
        var g = Easing.OutSine(time / Projectile.ai[1]);
        var a = (Projectile.timeLeft / Projectile.ai[1]);
        // Main.NewText(a + " " + g );
        var size = Projectile.ai[0] * g * 0.6;
        // Main.NewText(time);
        if (time / Projectile.ai[1] < hitBoxLife)
        {
            if (Projectile.friendly && Projectile.ai[2] > 0)
            {
                foreach (var NPC in Main.npc)
                {
                    if (NPC.active)
                    {
                        if (NPC.Center.Distance(Projectile.Center) < size && NPC.friendly == false &&
                            NPC.immune[Projectile.owner] <= 0)
                        {
                            NPC.StrikeNPC(NPC.CalculateHitInfo((int)Projectile.ai[2], 0, false, Projectile.knockBack));
                            NPC.immune[Projectile.owner] = Projectile.penetrate;
                        }
                    }
                }
            }
            else if (Projectile.hostile && Projectile.ai[2] > 0)
            {
                foreach (var Player in Main.player)
                {
                    if (Player.active)
                    {
                        if (Player.Center.Distance(Projectile.Center) < size &&
                            !Player.immune)
                        {
                            Player.Hurt(PlayerDeathReason.ByProjectile(255, Projectile.whoAmI),
                                (int)Projectile.ai[2], 0);
                            Player.AddImmuneTime(ImmunityCooldownID.General, Projectile.penetrate);
                        }
                    }
                }
            }
        }
        // Projectile.Size = Vector2(Projectile.ai[0] * (Projectile.ai[1] - Projectile.timeLeft))
    }

    protected virtual float ScaleLerpMod(float n)
    {
        return n;
    }
    
    protected virtual float AlphaLerpMod(float n)
    {
        return n;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        var time = (Projectile.ai[1] - Projectile.timeLeft);
        var g = Easing.OutSine(ScaleLerpMod(time / Projectile.ai[1]));
        var a = Easing.OutCirc(AlphaLerpMod(Projectile.timeLeft / Projectile.ai[1]));
        // Main.NewText(a + " " + g );
        var size = Projectile.ai[0] * g;
        
        // Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, Projectile.Center - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, size, SpriteEffects.None, 0f);
        var scaleFactor = size / ExplosionTexture.Size().X;
        // // 432
        // Main.spriteBatch.End();
        var color2 = color;
        
        // Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        PixelationCanvas.AddAdditiveDrawAction(() =>
        {
            var FUCK = (Projectile.Center - Main.screenPosition) / 2;
            // Lines.Rectangle(new Rectangle(Main.mouseX / 2, Main.mouseY / 2, 100, 100), Color.Red);
            Main.spriteBatch.Draw(ExplosionTexture, (Projectile.Center - Main.screenPosition) / 2, null, color * a, Projectile.rotation,
                ExplosionTexture.Size() / 2, scaleFactor,
                SpriteEffects.None, 0f);
            // Main.spriteBatch.Draw(explosionRingBlurred, (Projectile.Center - Main.screenPosition) / 2, null, color * a * 0.4f,
            //     0f, explosionRing.Size() / 2, scaleFactor,
            //     SpriteEffects.None, 0f);
            // Main.NewText(new Vector2(Main.mouseX, Main.mouseY));
        });
        
        //
        // Main.spriteBatch.DrawString(FontAssets.MouseText.Value, time.ToString(), Projectile.position - Main.screenPosition, Color.White);
        // Main.spriteBatch.DrawString(FontAssets.MouseText.Value, hitBoxLife.ToString(), Projectile.position - Main.screenPosition + new Vector2(0, 30), Color.White);
        return false;
    }

    public static void SpawnExplosion<T>(Vector2 position, int damage, float knockback, int owner, float radius, int length, Color color, int iframes = 10, bool friendly = true, float lifeTimeHitbox = 1f, bool setAlpha = true) where T : Explosion
    {
        var proj = Projectile.NewProjectileDirect(new EntitySource_Misc(""), position, Vector2.Zero,
            ModContent.ProjectileType<T>(), 0, knockback, owner, radius, length, damage);
        proj.timeLeft = length + 1;
        proj.penetrate = iframes;
        proj.hostile = !friendly;
        proj.friendly = friendly;
        // color *= (setAlpha ? 0 : 1);
        color.A = (byte)(setAlpha ? 1 : 0);
        ((T)proj.ModProjectile).color = color;
        ((T)proj.ModProjectile).hitBoxLife = lifeTimeHitbox;
        ((T)proj.ModProjectile).SetThings();
        
        
    }
    
    public static void SpawnExplosion<T>(IEntitySource source, Vector2 position, int damage, float knockback, int owner, float radius, int length, Color color, int iframes = 10, bool friendly = true, float lifeTimeHitbox = 1f, bool setAlpha = true) where T : Explosion
    {
        var proj = Projectile.NewProjectileDirect(source, position, Vector2.Zero,
            ModContent.ProjectileType<T>(), 0, knockback, owner, radius, length, damage);
        proj.timeLeft = length + 1;
        proj.penetrate = iframes;
        proj.hostile = !friendly;
        proj.friendly = friendly;

            // color *= (setAlpha ? 0 : 1);
        color.A = (byte)(setAlpha ? 1 : 0);
        ((T)proj.ModProjectile).color = color;
        ((T)proj.ModProjectile).hitBoxLife = lifeTimeHitbox;
        ((T)proj.ModProjectile).SetThings();
        
        
    }

    public void SetThings()
    {
        
    }
    

    /*Main.spriteBatch.Draw(explosionRing, Projectile.Center - Main.screenPosition, null, color * a, 0f,
    explosionRing.Size() / 2, scaleFactor * 2,
    SpriteEffects.None, 0f);
    Main.spriteBatch.Draw(explosionRingBlurred, Projectile.Center - Main.screenPosition, null, color * a * 0.4f,
    0f, explosionRing.Size() / 2, scaleFactor * 2,
    SpriteEffects.None, 0f);*/
}
    
    
