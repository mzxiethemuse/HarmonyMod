using System;
using HarmonyMod.Assets;
using HarmonyMod.Core.Graphics;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Dust;

public class Burst : ModDust
{
    const bool _debug = false;
    public override string Texture => AssetDirectory.Glow + "Explosion";
    

    public override bool Update(Terraria.Dust dust)
    {
        if (dust.customData is BurstData)
        {
            BurstData burstData = (BurstData)dust.customData;
            burstData.time++;
            burstData.AI(ref dust);
            if (burstData.time >= burstData.duration)
            {
                dust.active = false;
            }
        }

        return false;
    }

    public override bool PreDraw(Terraria.Dust dust)
    {
        if (dust.customData is BurstData)
        {
            BurstData burstData = (BurstData)dust.customData;
            burstData.AI(ref dust);

            var texture = Assets.Assets.Textures[burstData.texture];
            // burstData.time is the amt of time passed, timeLeft is the amount of time until duration expires
            var timeLeft = burstData.duration - burstData.time;
            var progress = Easing.OutSine(burstData.ScaleLerpMod(burstData.time / burstData.duration));
            var alpha = Easing.OutCirc(burstData.AlphaLerpMod(1 - (burstData.time / burstData.duration)));
            // Main.NewText(a + " " + g );
            var size = burstData.radius * progress;

            var scaleFactor = size / texture.Size().X;
            // // 43
            // Main.spriteBatch.End();
            var color2 = dust.color;
            PixelationCanvas.AddAdditiveDrawAction(() =>
            {
                var FUCK = (dust.position - Main.screenPosition) / 2;
                // Lines.Rectangle(new Rectangle(Main.mouseX / 2, Main.mouseY / 2, 100, 100), Color.Red);
                Main.spriteBatch.Draw(texture.Value, (dust.position - Main.screenPosition) / 2, null, dust.color * alpha,
                    dust.rotation,
                    texture.Size() / 2, scaleFactor,
                    SpriteEffects.None, 0f);
                // Main.spriteBatch.Draw(texture.Value, (dust.position - Main.screenPosition) / 2, null, dust.color,
                //                     dust.rotation,
                //                     texture.Size() / 2, scaleFactor,
                //                     SpriteEffects.None, 0f);
            });
            
        }
        return false;
    }
    

    public static void SpawnBurst(string texture, Vector2 position, Color color, float radius, int duration)
    {
        
        var peepee = Terraria.Dust.NewDustPerfect(position, ModContent.DustType<Burst>(), null, 0, color with {A = 0}, 1f);
        // DEBUG
        if (_debug) Terraria.Dust.NewDustPerfect(position, DustID.TintableDust, null, 0, color with {A = 0}, 5f);
        peepee.customData = new BurstData(texture, duration, radius);
    }
    
    public static void SpawnBurst(Vector2 position, Color color, BurstData data)
    {
        var peepee = Terraria.Dust.NewDustPerfect(position, ModContent.DustType<Burst>(), null, 0, color with {A = 0}, 1f);
        if (_debug)  Terraria.Dust.NewDustPerfect(position, DustID.TintableDust, null, 0, color with {A = 0}, 5f);

        peepee.customData = data;
    }
}

public class BurstData
{
    public string texture;
    public float time;
    public float radius;
    public float duration;
    
    public BurstData(string texture, float duration, float radius)
    {
        time = 0;
        this.duration = duration;
        this.radius = radius;
        this.texture = texture;
        
    }
    
    public virtual float ScaleLerpMod(float n)
    {
        return n;
    }
    
    public virtual float AlphaLerpMod(float n)
    {
        return n;
    }

    public virtual void AI(ref Terraria.Dust dust)
    {
    }
}