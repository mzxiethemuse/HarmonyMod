using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Core.Reworks.Ranger
{
    public class RangerProj : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.Name.ToLower().Contains("bullet");
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.player[projectile.owner].GetModPlayer<RangerPlayer>().flameBullets == true)
            {
                target.AddBuff(BuffID.OnFire, 120);
            }

        }

    }
}
