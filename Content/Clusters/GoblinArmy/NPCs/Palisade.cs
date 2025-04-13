using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.GoblinArmy.NPCs;

public class Palisade : ModNPC
{
    public override void SetDefaults()
    {
        NPC.width = 142;
        NPC.height = 84;
        NPC.friendly = false;
        NPC.damage = 40;
        NPC.defense = 30;
        NPC.lifeMax = 200;
        NPC.knockBackResist = 0f;

        NPC.DeathSound = SoundID.Shatter.WithPitchOffset(0.33f);
    }

    public override void AI()
    {
        NPC.velocity.X = 0;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        for (int i = 0; i < 4; i++)
        {
            Terraria.Dust.NewDust(NPC.position, 142, 84, DustID.WoodFurniture);
        }

        if (NPC.life <= 0)
        {
            GoblinEngineer.buildSmoke.Emit(NPC.position, 142, 84, 30);
        }
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
    {
        target.AddBuff(BuffID.Bleeding, 60 * 8);
    }
}