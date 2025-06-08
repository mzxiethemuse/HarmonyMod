using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Core.Reworks.Ranger;

class FuckYourChlorophyteBullets : GlobalProjectile
{
    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.ChlorophyteBullet;

    public override void SetDefaults(Projectile entity)
    {
        base.SetDefaults(entity);
        entity.damage = 1;
        entity.extraUpdates = 0;
            
            
    }

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        projectile.velocity *= 3;
        base.OnSpawn(projectile, source);
    }
}