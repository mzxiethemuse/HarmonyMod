using System.Collections.Generic;
using HarmonyMod.Content.Clusters.MidnightSwamp.NPCs;
using HarmonyMod.Content.Clusters.MidnightSwamp.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace HarmonyMod.Content.Clusters.MidnightSwamp;

public class SwampSpawns : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => true;

    public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
    {

        Player player = spawnInfo.Player;
        Point floorPos = new Point(spawnInfo.PlayerFloorX, spawnInfo.PlayerFloorY);
        Tile standingOn = Framing.GetTileSafely(floorPos);
        //lets implement our own tile checker, fuck it, man
        if (Main.UnderworldLayer > spawnInfo.PlayerFloorY && spawnInfo.PlayerFloorY > Main.worldSurface)
        {
            
            if (IsTileRoughlyInSwamp(floorPos.X, floorPos.Y))
            {
                pool[ModContent.NPCType<MidnightFly>()] = 3f;
                pool[ModContent.NPCType<StellarToad>()] = 1.5f;

            }
        }
    }

    public override void EditSpawnRange(Player player, ref int spawnRangeX, ref int spawnRangeY, ref int safeRangeX, ref int safeRangeY)
    {
        if (IsTileRoughlyInSwamp((int)(player.Center.X / 16), (int)(player.Center.Y / 16)))
        {
        //     spawnRangeX /= 2;
        //     spawnRangeY /= 2;
            safeRangeX = 1;
            safeRangeY = 1;
        }
    }

    // public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
    // {
    //     if (IsTileRoughlyInSwamp((int)(player.Center.X / 16), (int)(player.Center.Y / 16)))
    //     {
    //         spawnRate /= 2;
    //         maxSpawns += 10;
    //     }
    // }

    public bool IsTileRoughlyInSwamp(int i, int j)
    {
        Ref<int> mudCount = new Ref<int>(0);
        WorldUtils.Gen(new Point(i, j), new Shapes.Circle(25), Actions.Chain([
            new Modifiers.OnlyTiles((ushort)ModContent.TileType<StarMud>()),
            new Actions.Scanner(mudCount)
        ]));
        return (mudCount.Value > 36);
    }
}