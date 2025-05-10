using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;

namespace HarmonyMod.Core.Graphics;

public static class Trails
{
    public static void DrawTrail(Vector2[] positions, float[] rotations, Color color, Vector2 size, float minWidth, float maxWidth, float saturation, float opacity, string shader)
    {
        color.A *= 0;
        GraphicsDevice graphicsDevice = Main.graphics.GraphicsDevice;
        RasterizerState save = graphicsDevice.RasterizerState;
        graphicsDevice.RasterizerState = RasterizerState.CullNone;
        VertexStrip vertexStrip = new();
        MiscShaderData miscShaderData = GameShaders.Misc[shader];
        miscShaderData.UseSaturation(saturation);
        miscShaderData.UseOpacity(opacity);
        miscShaderData.Apply();
        vertexStrip.PrepareStripWithProceduralPadding(positions, rotations, (progress) => color,
            (progressOnStrip) =>
            {
                float num = 1f;
                float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
                num *= 1f - (1f - lerpValue) * (1f - lerpValue);
                return MathHelper.Lerp(minWidth, maxWidth, num);;
            }, -Main.screenPosition + size / 2f);
        vertexStrip.DrawTrail();
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        graphicsDevice.RasterizerState = save;
    }
    
    public static void DrawTrail(Vector2[] positions, float[] rotations, Vector2 size, VertexStrip.StripColorFunction color, VertexStrip.StripHalfWidthFunction width, float saturation, float opacity, string shader)
    {
        GraphicsDevice graphicsDevice = Main.graphics.GraphicsDevice;
        RasterizerState save = graphicsDevice.RasterizerState;
        graphicsDevice.RasterizerState = RasterizerState.CullNone;
        VertexStrip vertexStrip = new();
        MiscShaderData miscShaderData = GameShaders.Misc[shader];
        miscShaderData.UseSaturation(saturation);
        miscShaderData.UseOpacity(opacity);
        miscShaderData.Apply();
        vertexStrip.PrepareStripWithProceduralPadding(positions, rotations, color,
            width, -Main.screenPosition + size / 2f);
        vertexStrip.DrawTrail();
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        graphicsDevice.RasterizerState = save;
    }
    
    public static void DrawTrailPixelated(Vector2[] positions, float[] rotations, Vector2 size, VertexStrip.StripColorFunction color, VertexStrip.StripHalfWidthFunction width, float saturation, float opacity, string shader)
    {
        PixelationCanvas.AddAdditiveDrawAction(() =>
        {
            GraphicsDevice graphicsDevice = Main.graphics.GraphicsDevice;
            RasterizerState save = graphicsDevice.RasterizerState;
            graphicsDevice.RasterizerState = RasterizerState.CullNone;
            VertexStrip vertexStrip = new();
            MiscShaderData miscShaderData = GameShaders.Misc[shader];
            miscShaderData.UseSaturation(saturation);
            miscShaderData.UseOpacity(opacity);
            miscShaderData.Apply();
            vertexStrip.PrepareStripWithProceduralPadding(divideEachInArrayUtil(positions, 4), rotations, color,
                width, -Main.screenPosition / 2 + size / 2f);
            vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            graphicsDevice.RasterizerState = save;
        });
        
    }

    private static Vector2[] divideEachInArrayUtil(Vector2[] positions, int denominator)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = positions[i] / denominator;
        }

        return positions;
    }

    public static VertexStrip.StripColorFunction BasicColorLerp(Color a, Color b)
    {
        // a.A *= 0;
        // b.A *= 0;
        return progress =>
        {
            return Color.Lerp(a, b, progress);
        };
    }
    
    public static VertexStrip.StripHalfWidthFunction BasicWidthLerp(float minWidth, float maxWidth)
    {
        return (progressOnStrip) =>
        {
            float num = 1f;
            float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
            num *= 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(minWidth, maxWidth, num);
        };
    }
    
}

public struct TrailData
{
    public string Shader;
    public VertexStrip.StripColorFunction ColorFunction;
    public VertexStrip.StripHalfWidthFunction WidthFunction;
    public TrailData(VertexStrip.StripColorFunction color, VertexStrip.StripHalfWidthFunction width, string effect)
    {
        this.Shader = effect;
        this.ColorFunction = color;
        this.WidthFunction = width;
    }
}