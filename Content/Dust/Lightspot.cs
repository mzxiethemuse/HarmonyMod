using HarmonyMod.Asset;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HarmonyMod.Content.Dust;

public class Lightspot : FancyDust
{
    public override int alphaDecayRate => 1;
    public override float scaleDecayRate => 8f;
    public override Asset<Texture2D> texture => Assets.Whiteball;
    public override int size => 6;
}