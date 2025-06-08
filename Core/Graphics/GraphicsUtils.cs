using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace HarmonyMod.Core.Graphics;

public static class GraphicsUtils
{


    public static void StartDefaultSpriteBatch(Effect? effect = null)
    {
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, default,
            Main.Rasterizer, effect, Main.GameViewMatrix.TransformationMatrix);
    }

    /// <summary>
    /// Assumes sprite is facing left
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static SpriteEffects DirectionToSpriteEffect(int n)
    {
        if (n == 1)
        {
            return SpriteEffects.FlipHorizontally;
        } else return SpriteEffects.None;
    }
}