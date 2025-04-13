using System;
using System.Collections.Generic;
using HarmonyMod.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace HarmonyMod.Core.Graphics;

public class ScreenCanvas
{
    RenderTarget2D renderTarget;
    List<Action> actions;
    public Action PreBeginDrawToRT;
    public Action PreBeginDrawRT;
    public bool Enabled = true;
    public Vector2 SizeMultiplier = Vector2.One;
    public BlendState PreBlendState = BlendState.AlphaBlend;
    public BlendState PostBlendState = BlendState.AlphaBlend;
    public Color Color = Color.White;

    public static Texture2D fuck =
        ModContent.Request<Texture2D>(AssetDirectory.Placeholders + "Fuck").Value;
    /// <summary>
    /// Creates a new ScreenCanvas object, with a RenderTarget that is sized to the screen, affected by the `SizeMultiplier` property. This will render on its own unless `Enabled` is set to false.
    /// </summary>
    public ScreenCanvas()
    {
        actions = new List<Action>();
        // make sure we r running on the client (server doesnt have eyes to see our beautiful work)
        if (!Main.dedServ)
        {
            // adding this makes the rendertargets resize when the resolution is changed
            Main.OnResolutionChanged += InitRT;
            // rendertargets should be initialized on the main thread :D
            Main.RunOnMainThread(() =>
            {
                renderTarget = new RenderTarget2D(Main.instance.GraphicsDevice, (int)(Main.screenWidth * SizeMultiplier.X), (int)(Main.screenHeight * SizeMultiplier.Y));
            });
        }
        
        // i dont know why we use "CheckMonoliths", but that's where we hook into to draw things *to* the RT
        On_Main.CheckMonoliths += DrawToRT;
        On_Main.DrawProjectiles += On_Main_DrawProjectiles;
    }
    void DrawToRT(On_Main.orig_CheckMonoliths orig)
    {
        //invoke original? idk i dont do On stuff 
        orig.Invoke();
        // get the old rendert rgaters
        var gd = Main.graphics.GraphicsDevice;
        var oldRTs = gd.GetRenderTargets();
        gd.SetRenderTarget(renderTarget);
        gd.Clear(Color.Transparent);
        Main.spriteBatch.Begin(SpriteSortMode.Texture, PreBlendState, Main.DefaultSamplerState, default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        if (PreBeginDrawToRT != null)
        {
            PreBeginDrawToRT.Invoke();
        }

        foreach (var act in actions)
        {
            act.Invoke();
        }
        // Main.spriteBatch.Draw(fuck, Vector2.Zero, Color.White);

        // Main.spriteBatch.DrawString(FontAssets.MouseText.Value, Main.screenWidth.ToString(), Vector2.Zero, Color.White, 0f, Vector2.Zero, 100f, SpriteEffects.None, 0f);
        Main.spriteBatch.End();
        actions.Clear();
        // thgis is a dummy thing
        gd.SetRenderTargets(oldRTs);
    }

    private void DrawRT()
    {
        
        if (renderTarget == null || renderTarget.IsDisposed || !Enabled)
        {
            return;
        }

        Main.spriteBatch.Begin(SpriteSortMode.Texture, PostBlendState, Main.DefaultSamplerState, default, Main.Rasterizer
            );//Main.GameViewMatrix.TransformationMatrix);
        if (PreBeginDrawRT != null)
        {
            PreBeginDrawRT.Invoke();
        }
        // Lines.Rectangle(new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Red);
        // Main.spriteBatch.Draw(renderTarget, new Vector2(Main.screenWidth, Main.screenHeight) / 2, Color.White);
        // Main.spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(1 / SizeMultiplier.X, 1 / SizeMultiplier.Y), SpriteEffects.None, 0f);
        Main.spriteBatch.Draw(renderTarget, new Rectangle(0, 0,  Main.screenWidth, Main.screenHeight), Color);
        Main.spriteBatch.End();
    }
    
    private void On_Main_DrawProjectiles(On_Main.orig_DrawProjectiles orig, Main self)
    {
        orig.Invoke(self);
        DrawRT();
    }
    

    private void InitRT(Vector2 vec)
    {
        if (Main.dedServ)
        {
            return;
        }
        
        renderTarget?.Dispose();
        GraphicsDevice gd = Main.instance.GraphicsDevice;
        renderTarget = new RenderTarget2D(gd, (int)(vec.X * SizeMultiplier.X), (int)(vec.Y * SizeMultiplier.Y));
    }

    /// <summary>
    /// Adds an Action to the draw queue. The spriteBatch is already started here. Keep in mind the SizeMultiplier property.
    /// </summary>
    /// 
    public void AddAction(Action action)
    {
        actions.Add(action);
    }
}