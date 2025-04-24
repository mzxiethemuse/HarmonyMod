using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace HarmonyMod.Content.Clusters.Forest;

public class ForestGen : WorldGenTask
{
    public override string PlaceToInsert => "Flowers";

    public override void Apply(GenerationProgress progress = null, GameConfiguration gameConfiguration = null)
    {
        WorldHelper.TryAtVariousPointsAlongX(80, 7, 14, 3, i =>
        {
            Point surface = WorldHelper.FindSurfaceTileAtX(i + WorldGen.genRand.Next(-10, 11), 0);
            if (surface != new Point(-1, -1) && Framing.GetTileSafely(surface).TileType == TileID.Grass)
            {
                WorldUtils.Gen(surface, new Shapes.Slime(WorldGen.genRand.Next(4, 7)), Actions.Chain([
                    new Modifiers.Blotches(3, 0.7D),
                    new Modifiers.Expand(1),
                    new Modifiers.Offset(0, 3),
                    new Modifiers.RadialDither(0.99D, 0.5D),
                    new Modifiers.IsEmpty(),
                    new Modifiers.Offset(0, 2),
                    new Actions.PlaceWall(WallID.FlowerUnsafe),
                    new Modifiers.Dither(0.95D),
                ]));
            }
            return true;
        });
    }


    public ForestGen(string name, double loadWeight) : base(name, loadWeight)
    {
    }
}