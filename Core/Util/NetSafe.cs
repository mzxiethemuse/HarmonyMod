using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace HarmonyMod.Core.Util;

public class NetSafe
{
    //probably never used
    public NPC SpawnNPC(IEntitySource source, Vector2 position,int type, float ai0 = 0, float ai1 = 0, float ai2 = 0, float ai3 = 0)
    {
        if (Main.dedServ)
        {
            return Terraria.NPC.NewNPCDirect(source, (int)position.X, (int)position.Y, type, 0, ai0, ai1, ai2, ai3);
        }
        else return null;
    }

    public void SpawnProjectile()
    {
        // if (Main.dedServ)
        // {
        //     Projectile.NewProjectileDirect()
        // }
    }
}