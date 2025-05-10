using HarmonyMod.Content.Clusters.BloodMoon.Items.Weapons;
using Terraria;
using Terraria.ModLoader;

namespace HarmonyMod.Core;

public class HarmonyNPC : GlobalNPC
{
    public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
    {
        OnHurt(npc, hit);
    }

    public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        OnHurt(npc, hit);
    }

    public void OnHurt(NPC npc, NPC.HitInfo hit)
    {

    }

    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {
        if (npc.HasBuff<HemoptysisCurse>() &&
            (modifiers.DamageType == DamageClass.Melee || modifiers.DamageType == DamageClass.Ranged))
        {
            modifiers.FinalDamage *= 1.2f;
        }
    }
}