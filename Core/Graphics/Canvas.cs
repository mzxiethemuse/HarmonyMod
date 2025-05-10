using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace HarmonyMod.Core.Graphics;

/// <summary>
/// A canvas, Ill do this stuff later in the Like. Actual library thing I dont wanna modify this too much
/// </summary>
public class Canvas
{
    RenderTarget2D target;
    // why do we do this? Because I'm killing myself.
    public RenderTarget2D RenderTarget => target;
    public Color clearColor;
    
    
    Vector2 size;

    /// <summary>
    /// Creates a new Canvas, which manages its own RenderTarget. This should always be done on the Main Thread.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Canvas(int x, int y)
    {
        size = new Vector2(x, y);
        clearColor = Color.Transparent;
        // make sure we r running on the client (server doesnt have eyes to see our beautiful work)
        if (!Main.dedServ)
        {

            // rendertargets should be initialized on the main thread :D
            Main.RunOnMainThread(() =>
            {
                target = new RenderTarget2D(Main.instance.GraphicsDevice, x, y);
            });
        }
        // On_Main.CheckMonoliths += DrawContents;

    }

    /// <summary>
    /// Draws the Canvas at the specified position and color.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="color"></param>
    /// <param name="scale"></param>
    /// <param name="blendState2"></param>
    public void Draw(Vector2 position, Color color, Vector2 scale, BlendState? blendState2)
    {
        // make sure that the blendstate isnt null
        if (blendState2 == null) blendState2 = BlendState.AlphaBlend;
        
        //... or the RT itself
        if (target == null || target.IsDisposed)
        {
            return;
        }
        // this is how to draw things if you dont know
        Main.spriteBatch.Begin(SpriteSortMode.Texture, blendState2, Main.DefaultSamplerState, default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        Main.spriteBatch.Draw(target, new Rectangle((int)position.X, (int)position.Y, (int)(size.X * scale.X), (int)
            (size.Y * scale.Y)), color);//new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
        Main.spriteBatch.End();
    }
    
    /// <summary>
    /// this draws the rendertarget (assumes spriteBatch has begun). also automatically clears the target.
    /// </summary>
    /// <param name="targetRect"></param>
    /// <param name="color"></param>
    /// <param name="blendState2"></param>
    public void Draw(Rectangle targetRect, Color color, bool clear = true)
    {
        //... or the RT itself
        if (target == null || target.IsDisposed)
        {
            return;
        }
        // this is how to draw things if you dont know
        Main.spriteBatch.Draw(target, targetRect, color);//new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);

        if (clear) {
            var gd = Main.graphics.GraphicsDevice;
            var oldRTs = gd.GetRenderTargets();
            gd.SetRenderTarget(target);
            gd.Clear(clearColor);
            gd.SetRenderTargets(oldRTs);
        }
    }

    /// <summary>
    /// this function Assumes that the spritebatch is ended.
    /// </summary>
    /// <param name="drawCall"></param>
    public void DrawToCanvas(Action drawCall, BlendState blendState = null, Effect shader = null)
    {

        var gd = Main.graphics.GraphicsDevice;
        var oldRTs = gd.GetRenderTargets();
        gd.SetRenderTarget(target);
        
        Main.spriteBatch.Begin(SpriteSortMode.Texture, blendState, Main.DefaultSamplerState, default, Main.Rasterizer, shader, Matrix.Identity);
        drawCall();
        Main.spriteBatch.End();
    }
    
    


    // private void DrawContents(On_Main.orig_CheckMonoliths orig)
    // {
    //     //invoke original? idk i dont do On stuff 
    //     orig.Invoke();
    //     // get the old rendert rgaters
    //     var gd = Main.graphics.GraphicsDevice;
    //     var oldRTs = gd.GetRenderTargets();
    //     gd.SetRenderTarget(target);
    //     gd.Clear(clearColor);
    //     
    //     Main.spriteBatch.Begin(SpriteSortMode.Texture, blendState, Main.DefaultSamplerState, default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
    //
    //     Main.spriteBatch.End();
    //     
    //     
    //     // thgis is a dummy thing
    //     gd.SetRenderTargets(oldRTs);
    // }
    
    private void InitRT(Vector2 vec)
    {
        if (Main.dedServ)
        {
            return;
        }
        
        target?.Dispose();
        GraphicsDevice gd = Main.instance.GraphicsDevice;
        target = new RenderTarget2D(gd, (int)vec.X, (int)vec.Y);
    }
    
    //         Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

}