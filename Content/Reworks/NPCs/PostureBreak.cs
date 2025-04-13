using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Reworks.NPCs;

public class PostureBreak : GlobalNPC
{
    // public override bool InstancePerEntity => true;
    //
    // private int postureThresh;
    // private int posture;
    // private bool postureBroken;
    //
    // public override void OnSpawn(NPC npc, IEntitySource source)
    // {
    //     postureThresh = npc.life * npc.defense;
    //     posture = 0;
    //     postureBroken = false;
    // }
    //
    // public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
    // {
    //     
    //     OnHurt(npc, player, hit);
    // }
    //
    // public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    // {
    //     OnHurt(npc, Main.player[projectile.owner], hit);
    // }
    //
    // public void OnHurt(NPC npc, Player player, NPC.HitInfo hit)
    // {
    //     if (posture > postureThresh)
    //     {
    //         postureBroken = true;
    //     } else if (posture <= 0)
    //     {
    //         postureBroken = false;
    //     }
    //
    //     if (postureBroken)
    //     {
    //         postureBroken = false;
    //     }
    // }
    //
    // public override void PostAI(NPC npc)
    // {
    //     posture--;
    //     if (postureBroken)
    //     {
    //         posture--;
    //     }
    // }
}