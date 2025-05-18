using Terraria;

namespace HarmonyMod.Content.Reworks.Warrior;

public class ParryActions
{
    public static void PBoost(Player player, Projectile projectile)
    {
        if (Main.myPlayer == player.whoAmI)
        {
            
            projectile.velocity = player.DirectionTo(Main.MouseWorld) * 20 + (player.velocity * 0.7f);
            projectile.damage += 2;
        }
    }
    
}