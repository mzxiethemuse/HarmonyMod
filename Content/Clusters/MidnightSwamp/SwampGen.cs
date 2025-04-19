using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.WorldBuilding;
using WorldGen = Terraria.WorldGen;

namespace HarmonyMod.Content.Clusters.MidnightSwamp;

public class SwampGen : WorldGenTask
{
    public override string PlaceToInsert => "Stalac";

    public SwampGen(string name, double loadWeight) : base(name, loadWeight)
    {
    }


    public override void Apply(GenerationProgress progress = null, GameConfiguration gameConfiguration = null)
    {
        WorldUtils.Gen(new Point(100, 100), new Shapes.Circle(8, 8), Actions.Chain(new GenAction[]
        {
            new Actions.SetTile(TileID.AmberGemspark),
            new Actions.Smooth(),
            new Actions.Custom(((i, j, args) =>
            {
                WorldUtils.TileFrame(i, j, true);
                return true;
            }))
        }));
        
    }
}