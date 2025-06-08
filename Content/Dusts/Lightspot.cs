using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace HarmonyMod.Content.Dusts;

public class Lightspot : FancyDust
{
    public override int alphaDecayRate => 1;
    public override float scaleDecayRate => 8f;
    public override Asset<Texture2D> texture => Assets.Assets.Whiteball;
    public override int size => 6;
}