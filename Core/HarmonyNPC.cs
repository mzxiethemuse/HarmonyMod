using HarmonyMod.Content.Clusters.BloodMoon.Items.Weapons;
using HarmonyMod.Content.Clusters.BloodMoon.Projectiles;
using Microsoft.Xna.Framework;
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
        if (npc.HasBuff<SanguineCurse>() &&
            (modifiers.DamageType == DamageClass.Melee || modifiers.DamageType == DamageClass.Ranged || modifiers.DamageType == DamageClass.Throwing))
        {
            modifiers.FinalDamage *= 1.1f;
        }
    }

    public override void OnKill(NPC npc)
    {
        if (npc.HasBuff<SanguineCurse>())
        {
            for (int i = 0; i < Main.rand.Next(3,11); i++)
            {
                var proj = Projectile.NewProjectileDirect(npc.GetSource_Death(), npc.Center, Vector2.Zero,
                    ModContent.ProjectileType<LeperFlesh>(), 5, 3f);
                proj.friendly = true;
            }
        }

        base.OnKill(npc);
    }
    
}