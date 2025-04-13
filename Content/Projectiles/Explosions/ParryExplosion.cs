using HarmonyMod.Assets;
using Terraria;

namespace HarmonyMod.Content.Projectiles.Explosions;

public class ParryExplosion : Explosion
{
    public override string Texture => AssetDirectory.Glow + "Explosion";

    public override void PostAI()
    {
        Projectile.Center = Main.player[Projectile.owner].Center;
    }
}