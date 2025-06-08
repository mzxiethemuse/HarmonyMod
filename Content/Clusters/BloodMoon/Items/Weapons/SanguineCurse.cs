using HarmonyMod.Assets;
using HarmonyMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.BloodMoon.Items.Weapons;

public class SanguineCurse : ModBuff {
    public override string Texture => AssetDirectory.Placeholders + "GenericDebuff";

    public override void Update(NPC npc, ref int buffIndex)
    {
        if (Main.rand.NextBool(4))
        {
            Terraria.Dust.NewDustDirect(npc.position, npc.width, npc.height,
                ModContent.DustType<Lightspot>(), 0, 0.7f, 100,
                Color.Red, 0.9f).velocity.X = 0;
        }
    }
}