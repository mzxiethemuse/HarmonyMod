using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace HarmonyMod.Content.Dusts;

//TODO: rework ts
public class DustEmitter
{
    private int dustType;
    private Color colorA = Color.White;
    private Color colorB = Color.White;
    private Vector2 velspread = Vector2.Zero;
    public object customData;

    private Vector2 scaleRange = new Vector2(1f, 1f);
    
    public DustEmitter(int type)
    {
        dustType = type;
    }
    
    public DustEmitter(int type, float minScale, float maxScale, Vector2 velRadius, Color color)
    {
        dustType = type;
        scaleRange = new Vector2(minScale, maxScale);
        velspread = velRadius;
        colorA = color;
    }
    
    public DustEmitter(int type, float minScale, float maxScale, Vector2 velRadius, Color color, object data)
    {
        dustType = type;
        scaleRange = new Vector2(minScale, maxScale);
        velspread = velRadius;
        colorA = color;
        customData = data;
    }

    public DustEmitter(int type, float minScale, float maxScale, Vector2 velRadius, Color colorA, Color colorB, object data)
    {
        dustType = type;
        scaleRange = new Vector2(minScale, maxScale);
        velspread = velRadius;
        this.colorA = colorA;
        this.colorB = colorB;

        customData = data;
    }
    public void SetScaleRange(float min, float max)
    {
        scaleRange = new Vector2(min, max);
    }
    
    public void SetVelocitySpread(Vector2 radius)
    {
        velspread = radius;
    }

    public void SetColors(Color a, Color b)
    {
        colorA = a;
        colorB = b;
    }
    public List<Terraria.Dust> Emit(Vector2 position, int width, int height, int n)
    {
        var list = new List<Terraria.Dust>();
        for (int i = 0; i < n; i++)
        {
            var velocity = Main.rand.NextVector2Circular(velspread.X, velspread.Y);
            var d = Terraria.Dust.NewDustDirect(
                position,
                width,
                height,
                dustType,
                velocity.X,
                velocity.Y,
                0,
                Color.Lerp(colorA, colorB, Main.rand.NextFloat()),
                MathHelper.Lerp(scaleRange.X, scaleRange.Y, Main.rand.NextFloat())
            );
            d.customData = customData;
            list.Add(d);
        }

        return list;
    }
    
    public List<Terraria.Dust> Emit(int id, Vector2 position, int width, int height, int n)
    {
        var list = new List<Terraria.Dust>();
        for (int i = 0; i < n; i++)
        {
            var velocity = Main.rand.NextVector2Circular(velspread.X, velspread.Y);
            var d = Terraria.Dust.NewDustDirect(
                position,
                width,
                height,
                id,
                velocity.X,
                velocity.Y,
                0,
                Color.Lerp(colorA, colorB, Main.rand.NextFloat()),
                MathHelper.Lerp(scaleRange.X, scaleRange.Y, Main.rand.NextFloat())
            );
            d.customData = customData;
            list.Add(d);
        }

        return list;
    }
    
    public static List<Terraria.Dust> Emit(int id, Vector2 position, int width, int height, int n, Vector2 velspread = default(Vector2), Color colorA = default(Color), Color colorB = default(Color), float minScale = 1f, float maxScale = 1f, object customData = null, bool noGravity = false)
    {
        var list = new List<Terraria.Dust>();
        for (int i = 0; i < n; i++)
        {
            var velocity = Main.rand.NextVector2Circular(velspread.X, velspread.Y);
            var d = Terraria.Dust.NewDustDirect(
                position,
                width,
                height,
                id,
                velocity.X,
                velocity.Y,
                0,
                Color.Lerp(colorA, colorB, Main.rand.NextFloat()),
                MathHelper.Lerp(minScale, maxScale, Main.rand.NextFloat())
            );
            d.customData = customData;
            if (noGravity)
            {
                d.noGravity = true;
            }
            list.Add(d);
            
        }

        return list;
    }
}