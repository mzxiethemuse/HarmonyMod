using HarmonyMod.Asset;
using HarmonyMod.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;

using Assets = HarmonyMod.Asset;
namespace HarmonyMod.Content.Dust;


public abstract class FancyDust : ModDust
{
    public virtual Asset<Texture2D> texture => Assets.Assets.VFXCircle;
    public virtual int size => 16;
    
    public virtual int alphaDecayRate => 0;
    public virtual float scaleDecayRate => 5f;
    
    public override string Texture => AssetDirectory.Glow + "smoke_01";

    public override bool Update(Terraria.Dust dust)
    {
        dust.position += dust.velocity;
        dust.scale *= (1) - scaleDecayRate / 100;
        dust.alpha += alphaDecayRate;
        
        dust.rotation += dust.velocity.X * 0.1f;
        if (dust.scale < 0.1f || dust.alpha < 1)
        {
            dust.active = false;
        }
        return false;
    }

    public override bool PreDraw(Terraria.Dust dust)
    {
        var scaleFactor = (float)size / texture.Value.Width;
        PixelationCanvas.AddAdditiveDrawAction(() =>
        {
            var color = dust.color with {A = (byte)(255 - dust.alpha)};
            // color.A *= 0;
            // Main.spriteBatch.Draw(tex, Vector2.Zero, tex.Bounds, color, dust.rotation, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture.Value, (dust.position - Main.screenPosition) / 2, texture.Value.Bounds, color, dust.rotation, texture.Size() / 2, scaleFactor * dust.scale, SpriteEffects.None, 0f);
        });
        return false;
        
    }
        
}
