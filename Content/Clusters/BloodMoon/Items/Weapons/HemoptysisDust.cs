using HarmonyMod.Asset;
using HarmonyMod.Content.Dust;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace HarmonyMod.Content.Clusters.BloodMoon.Items.Weapons;

public class HemoptysisDust : FancyDust
{
    public override int alphaDecayRate => 20;
    public override float scaleDecayRate => 8f;
    public override Asset<Texture2D> texture => Assets.VFXScorch[2];
    public override int size => 16;
}