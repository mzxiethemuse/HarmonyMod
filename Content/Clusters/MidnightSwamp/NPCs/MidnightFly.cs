using HarmonyMod.Core.Graphics;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace HarmonyMod.Content.Clusters.MidnightSwamp.NPCs;

public class MidnightFly : ComplexNPC
{
    public override void SetStaticDefaults()
    {
        NPCID.Sets.TrailingMode[Type] = 2;
        NPCID.Sets.TrailCacheLength[Type] = 14;
        base.SetStaticDefaults();
    }

    public override void SetDefaults()
    {
        NPC.width = 12;
        NPC.height = 6;
        NPC.aiStyle = -1;
        NPC.lifeMax = 4;
        NPC.friendly = false;
        NPC.damage = 0;
        NPC.noGravity = true;
        base.SetDefaults();
    }

    public override void AI()
    {
        if (Main.rand.NextBool(4))
        {
            NPC.velocity = Main.rand.NextVector2CircularEdge(0.2f, 0.2f);

        }
        base.AI();
    }
    //
    // public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    // {
    //     Trails.DrawTrail(NPC.oldPos, NPC.oldRot, Color.Azure, NPC.Size, 1, 8, -3f, 1f, "RainbowRod");
    //     return true;
    // }
}