using System;
using HarmonyMod.Assets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.MidnightSwamp.Tiles;

public class StarMud : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileMerge[Type][TileID.Mud] = true;
        TileID.Sets.Mud[Type] = true;
        // i barely know what half of these do, to be honest
        // TileID.Sets.OreMergesWithMud[Type] = true;

        DustType = DustID.MagicMirror;
        
        AddMapEntry(Color.DeepSkyBlue * 0.7f);
    }


    public override void FloorVisuals(Player player)
    {
        if (!player.IsStandingStillForSpecialEffects && Main.rand.NextBool(4))
        {
            var dust = Terraria.Dust.NewDustPerfect(player.Center + new Vector2(0, player.height / 2), DustID.YellowStarDust);
            dust.velocity = new Vector2(Main.rand.NextFloat(-0.2f,0.2f), -1);
        }
    }

    public override void RandomUpdate(int i, int j)
    {
        Terraria.Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.YellowStarDust, 0, -1);

    }

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (closer && Main.rand.NextBool(247))
        {
            RandomUpdate(i, j);
        }
    }
}

public class StarMudItem : ModItem
{
    public override string Texture => AssetDirectory.Placeholders + "ExampleBlock";

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.StarMud>());
        Item.width = 12;
        Item.height = 12;
        
    }
}