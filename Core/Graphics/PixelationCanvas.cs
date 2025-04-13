using System;
using HarmonyMod.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace HarmonyMod.Core.Graphics;

public class PixelationCanvas : ModSystem
{
    public static Texture2D fuck =
        ModContent.Request<Texture2D>(AssetDirectory.Placeholders + "Fuck").Value;
    public ScreenCanvas additiveCanvas;

    public override void Load()
    {
        Main.QueueMainThreadAction(() =>
        {
            additiveCanvas = new ScreenCanvas();
            additiveCanvas.SizeMultiplier = new Vector2(0.5f, 0.5f);
            additiveCanvas.PreBlendState = BlendState.AlphaBlend;
            additiveCanvas.PostBlendState = BlendState.AlphaBlend;
            additiveCanvas.Color = Color.White;
            additiveCanvas.PreBeginDrawToRT = () =>
            {
                GameShaders.Misc["HarmonyMod:Basic"].Apply(null);
                // Main.spriteBatch.Draw(fuck, new Rectangle(134, 0, Main.screenWidth / 2, Main.screenHeight / 2), Color.White * 0.5f);
            };
        });
        
        base.Load();
    }
    

    /// <summary>
    /// SPRITEBATCH!!! SIZE !! 2X!!!!
    /// </summary>
    /// <param name="action"></param>
    public static void AddAdditiveDrawAction(Action action)
    {
        PixelationCanvas canvas = ModContent.GetInstance<PixelationCanvas>();
        canvas.additiveCanvas.AddAction(action);
    }
}