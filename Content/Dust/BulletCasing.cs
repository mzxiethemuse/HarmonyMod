using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Dust
{
    public class BulletCasing : ModDust

    {
        public override void OnSpawn(Terraria.Dust dust)
        {
        }
        public override bool Update(Terraria.Dust dust)
        {

            return false;
        }

        public override bool PreDraw(Terraria.Dust dust)
        {
            return false;
        }

        //public static Terraria.Dust SpawnBulletCasing(Vector2 position)
        //{
        //    return Terraria.Dust.NewDustDirect(position, 0, 0, ModContent.DustType<BulletCasing>());
        //}
    }
}
