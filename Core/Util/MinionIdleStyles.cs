using Microsoft.Xna.Framework;
using Terraria;

namespace HarmonyMod.Core.Util;

public static class MinionIdleStyles
{
    public static Vector2 IdlePositionLineup(this Projectile projectile, Player owner, float yOffset, float xOffsetFlat, float xOffsetMult)
    {
        Vector2 idle = owner.Center;
        idle.Y += yOffset;
        idle.X += (xOffsetFlat + projectile.minionPos * xOffsetMult) * -owner.direction;
        return idle;
    }
    
    
    public static Vector2 IdlePositionCircle(this Projectile projectile, Player owner)
    {
        var offset = (new Vector2(-70, -30).RotatedBy(0.6 * projectile.minionPos));
        offset.X *= owner.direction;
        return owner.Center + offset;
    }
}