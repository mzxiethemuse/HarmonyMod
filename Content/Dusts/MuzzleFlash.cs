﻿using Terraria.ModLoader;
// using ScaffoldLib.Glow;

namespace HarmonyMod.Content.Dusts
{
    public class MuzzleFlash : ModDust

    {
        public override void OnSpawn(Terraria.Dust dust)
        {
            dust.customData = 0f;
        }
        public override bool Update(Terraria.Dust dust)
        {
            if (dust.customData is float Wow) {
                dust.customData = Wow += 0.1f;
            }
            dust.position += dust.velocity;

            return false;
        }

        public override bool PreDraw(Terraria.Dust dust)
        {
            // GlowDrawer.DrawGlow(Main.spriteBatch, dust.position, Color.OrangeRed, (float)dust.customData, (2f - (float)dust.customData * 2), "KenneyStarHD", 0f);
            return false;
        }
    }
}
