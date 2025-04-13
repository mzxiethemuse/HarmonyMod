using HarmonyMod.Assets;
using HarmonyMod.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;

using Assets = HarmonyMod.Assets;
namespace HarmonyMod.Content.Dust;


public class FancySmoke : ModDust
{
    
    public const int size = 16;
    public override string Texture => AssetDirectory.Glow + "smoke_01";

    public override bool Update(Terraria.Dust dust)
    {
        dust.position += dust.velocity;
        dust.scale *= 0.95f;
        
        dust.rotation += dust.velocity.X * 0.1f;
        if (dust.scale < 0.1f)
        {
            dust.active = false;
        }
        return false;
    }

    public override bool PreDraw(Terraria.Dust dust)
    {
        var tex = Assets.Assets.Textures["smoke_0" + (dust.customData ?? 1)] ;
        var scaleFactor = (float)size / tex.Value.Width;
        PixelationCanvas.AddAdditiveDrawAction(() =>
        {
            var color = dust.color;
            // color.A *= 0;
            // Main.spriteBatch.Draw(tex, Vector2.Zero, tex.Bounds, color, dust.rotation, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(tex.Value, (dust.position - Main.screenPosition) / 2, tex.Value.Bounds, color, dust.rotation, tex.Size() / 2, scaleFactor * dust.scale, SpriteEffects.None, 0f);
        });
        return false;
    }
}
