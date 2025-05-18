using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace HarmonyMod.Core.Graphics;

public class AnimationHandler
{
    public AnimationHandler(Asset<Texture2D> texture)
    {
        this.texture = texture;
    }
    
    
    
    public Asset<Texture2D> texture;
    protected int selectedAnim = 0;
    protected int frame = 0;
    protected int frameTime = 0;
    
    protected List<AnimationData> animations = new();

    public void Draw(Vector2 centerposition, float rotation, Color color, Vector2 scale)
    {
        
        AnimationData animation = animations[selectedAnim];

        
        Vector2 origin = new Vector2(GetFrameRect().Width, GetFrameRect().Height) / 2;
        Main.NewText(GetFrameRect().ToString());
        Main.spriteBatch.Draw(texture.Value, centerposition, GetFrameRect(), color, rotation, origin, scale, SpriteEffects.None, 0);
        frameTime++;
        if (frameTime > animation.frameLengths[frame])
        {
            frameTime = 0;
            frame++;
            if (frame >= animation.frames)
            {
                frame = 0;
            }
        }
    }

    public Rectangle GetFrameRect()
    {
        AnimationData animation = animations[selectedAnim];
        Point finPos = animation.position + new Point(animation.bounds.X * frame, 0);
        return new Rectangle(animation.position.X + animation.bounds.X * frame, animation.position.Y,animation.bounds.X, animation.bounds.Y);
    }


    public void PlayAnimation(int id)
    {
        frame = 0;
        frameTime = 0;
        selectedAnim = id;
    }
    public AnimationHandler AddAnimation(AnimationData animation)
    {
        animations.Add(animation);
        return this;
    }
    
    public AnimationHandler AddAnimation(Rectangle bounds, int frames, int frameLength = 1)
    {
        var framelengths = new int[frames];
        Array.Fill(framelengths, frameLength);
        animations.Add(new AnimationData(bounds.Location, new Point(bounds.Width, bounds.Height), framelengths));
        return this;
    }
    
    public AnimationHandler AddAnimation(Rectangle bounds, int[] frames)
    {
        animations.Add(new AnimationData(bounds.Location, new Point(bounds.Width, bounds.Height), frames));
        return this;
    }
    
}

public struct AnimationData
{
    public Point position;
    /// <summary>
    /// despite being "bounds", the topleft of this should be 0, 0, unless you have padding.
    /// </summary>
    public Point bounds;

    public int[] frameLengths;
    
    public int frames => frameLengths.Length;

    public AnimationData(Point position, Point bounds, int frames)
    {
        this.position = position;
        this.bounds = bounds;
        this.frameLengths = new int[frames];
    }
    
    public AnimationData(Point position, Point bounds, int[] frameLengths)
    {
        this.position = position;
        this.bounds = bounds;
        this.frameLengths = frameLengths;
    }
}