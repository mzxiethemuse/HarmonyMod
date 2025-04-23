using HarmonyMod.Content.Clusters.MidnightSwamp.Tiles;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using WorldGen = Terraria.WorldGen;


namespace HarmonyMod.Content.Clusters.MidnightSwamp;

public class SwampGen : WorldGenTask
{
    private const float size = 2f;
    public override string PlaceToInsert => "Stalac";

    public SwampGen(string name, double loadWeight) : base(name, loadWeight)
    {
    }


    public override void Apply(GenerationProgress progress = null, GameConfiguration gameConfiguration = null)
    {
        if (MidnightSwampSystem.DisableAllSwampStuffForDebug) return;
        progress.Message = "Landing a fallen star in the jungle...";
        
        WorldHelper.TryAtVariousPointsAlongX(350, 10, 50, 50, (x =>
        {
            int y = Main.rand.Next((int)Main.worldSurface + 100, (int)Main.worldSurface + 300);
            Point point = new(x, y);
            if (Framing.GetTileSafely(point).TileType == TileID.JungleGrass)
            {
                if (GenerateMidnightSwampAt(point))
                {
                    ModContent.GetInstance<MidnightSwampSystem>().swampPos = point;
                    return true;
                }

            }
            return false;
        }));
        
    }

    public static bool GenerateMidnightSwampAt(Point pos)
    {
        ushort SwampGrassID = (ushort)ModContent.TileType<StarMud>();
        ShapeData biomeShape = new ShapeData();
        ShapeData grassMask = new ShapeData();
        ShapeData tileMask = new ShapeData();
        // generate the initial circular shape of the biome, replace jungle grass with swamp grass
        WorldUtils.Gen(pos, new Shapes.Circle(
            (int)(WorldGen.genRand.Next(40, 60) * size), (int)(WorldGen.genRand.Next(23, 28) * size)
        ).Output(biomeShape),  Actions.Chain([
            new Modifiers.Blotches(),
            new Modifiers.OnlyTiles(TileID.JungleGrass, TileID.Mud).Output(tileMask),
            new Modifiers.OnlyTiles(TileID.JungleGrass).Output(grassMask),
            new Actions.SetTileKeepWall(SwampGrassID),
            new Actions.SetFrames(true)
            
        ]));
        ShapeData holesShape = new ShapeData();
        // // adding random pockets, some with liquid
        WorldUtils.Gen(pos, new ModShapes.All(tileMask), Actions.Chain(
        [
            new Modifiers.Dither(0.99),
            new Actions.Custom(((x, y, args) =>
            {
                WorldUtils.Gen(new Point(x, y), new Shapes.Slime(1, WorldGen.genRand.Next(7, 11), (WorldGen.genRand.Next(3, 5))), Actions.Chain([
                    new Modifiers.Blotches(),
                    new Modifiers.InShape(biomeShape),
                    new Actions.ClearTile(true),
                    new Modifiers.RectangleMask(0, 7, 0, 3),
                    new Actions.SetLiquid()
                ]));
                return true;
            })).Output(holesShape),
        ]));;

        WorldUtils.Gen(pos, new ModShapes.All(biomeShape), Actions.Chain([
            new Modifiers.OnlyTiles(TileID.JungleGrass, TileID.Mud, SwampGrassID).Output(tileMask)
        ]));

        ShapeData airShape = new ShapeData();
        WorldUtils.Gen(pos,
            new ModShapes.All(biomeShape),
            Actions.Chain([
                new Modifiers.IsEmpty().Output(airShape),
            ])
        );

        WorldUtils.Gen(pos, new ModShapes.All(biomeShape), Actions.Chain([
            new Modifiers.OnlyTiles(),
            new Actions.Blank().Output(tileMask)
        ]));
        
        WorldUtils.Gen(pos, new ModShapes.All(tileMask), 
            Actions.Chain([
                // new Modifiers.InShape(tileMask)
                new Modifiers.Dither(0.98),
                new Modifiers.Expand(1),
                new Modifiers.Blotches(3),
                new Modifiers.InShape(tileMask),
                new Actions.SetTile(SwampGrassID, true)
            ]));

        WorldUtils.Gen(pos, new ModShapes.All(biomeShape), Actions.Chain([
            new Modifiers.OnlyWalls(WallID.JungleUnsafe, WallID.JungleUnsafe2, WallID.JungleUnsafe3, WallID.JungleUnsafe4),
            new Actions.PlaceWall(WallID.MudUnsafe),
            new Modifiers.Dither(0.99D),
            new Actions.PlaceWall(WallID.TopazUnsafe),
        ]));
        
        

        // WorldUtils.Gen(pos, new ModShapes.InnerOutline(tileMask), new Actions.SetTile(TileID.MushroomGrass, true));
        // //moss it up
        // WorldUtils.Gen(pos, new ModShapes.OuterOutline(tileMask, true), Actions.Chain([
        //     new Actions.SetTile(TileID.ArgonMoss, true)
        // ]));
        return true;
    }
}