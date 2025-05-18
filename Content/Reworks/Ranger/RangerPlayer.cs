
using Microsoft.Xna.Framework.Input;
using System;
using HarmonyMod.Core.Util;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Color = Microsoft.Xna.Framework.Color;

namespace HarmonyMod.Content.Reworks.Ranger
{
    public class RangerPlayer : ModPlayer
    {

        // defaults for overall balacning
        public static float globalRecoilDefault = 1.5f;
        public static float globalRecoilPower = 0.4f;
        public static float globalRecoilDecay = 25;
        public static float eyeattachmentspeedreq = 10f;

        // global recoil value
        public float globalRecoilMod = 2; // how much recoil guns give you
        public float recoilY = 0;
        public float recoilX = 0;
        public float recoilDecay = 20; // how fast recoil decays/stops (how sudden the recoil is)
        public float recoilPower = 0.4f; // how much recoil is applied per frame (how much recoil is applied, and how quickly)

        //bonuses
        public float globalRecoilBonus = 0;
        public float recoilDecayBonus = 0;
        public float recoilPowerBonus = 0;
        public float ammoConsumeChance = 0;
        public float horizontalRecoilMod = 0;
        //special flags
        public bool flameBullets;
        public bool drawNerveSpeed;

        public override void PostItemCheck()
        {
            //shoot those guns!
            Item item = Player.HeldItem;
            if (item != null && Player.ItemAnimationJustStarted)
            {
                if (isProbablyAGun(item))
                {

                    OnGunFired(item);

                }

            }
        }
        public override void PreUpdate()
        {


            if (recoilY > 0 || recoilX > 0)
            {
                recoilY *= 1 - recoilDecay / 100;
                recoilX *= 1 - recoilDecay / 100;

                MouseState mouseState = Mouse.GetState();
                Mouse.SetPosition(mouseState.X + (int)(recoilX * recoilPower), mouseState.Y - (int)(recoilY * recoilPower));
                recoilY = MathF.Max(recoilY, 0);
                if (recoilY < 0.01) recoilY = 0;
                recoilX = MathF.Max(recoilX, 0);
                if (recoilX < 0.01) recoilX = 0;
            }

        }

        public void OnGunFired(Item gun)
        {
            float Yamount = gun.useAnimation * globalRecoilMod + Main.rand.NextFloat(-gun.knockBack, gun.knockBack);
            float Xamount = gun.knockBack * Main.rand.NextFloat(-gun.knockBack, gun.knockBack) * globalRecoilMod * horizontalRecoilMod;
            if (Main.MouseWorld.X > Player.Center.X + 50 && Main.MouseWorld.X < Player.Center.X - 50) Xamount *= 2;
            recoilY += Yamount;
            recoilX += Xamount;
            recoilY = MathUtility.AttenuateKindaMaybeIdkWhatToCallThisFunction(recoilY, 60);
            recoilX = MathUtility.AttenuateKindaMaybeIdkWhatToCallThisFunction(recoilX, 60);

            // draw muzzleflash
            //float dirToMouse = (Main.MouseWorld - Player.Center).ToRotation();
            //Vector2 flashpos = Player.Center + new Vector2(MathF.Max(gun.width, 50) * gun.scale, 0).RotatedBy(dirToMouse);
            //Dust flash = Dust.NewDustDirect(flashpos - new Vector2(3, 3), 2, 2, ModContent.DustType<MuzzleFlash>());
            //flash.velocity = Player.velocity + Main.rand.NextVector2Circular(1,1);


        }

        public override void ResetEffects()
        {
            flameBullets = false;
            drawNerveSpeed = false;
            
            
            horizontalRecoilMod = 1;
            recoilDecayBonus = 1;
            globalRecoilBonus = 1;
            recoilPowerBonus = 1;
            ammoConsumeChance = 0;
            base.ResetEffects();
        }

        public override void PostUpdateEquips()
        {
            recoilPower = globalRecoilPower;
            globalRecoilMod = globalRecoilDefault;
            recoilDecay = globalRecoilDecay;
            
            globalRecoilBonus = MathF.Max(globalRecoilBonus, 0.2f);
            recoilPower = globalRecoilPower * recoilPowerBonus;
            globalRecoilMod = globalRecoilDefault * globalRecoilBonus;
            recoilDecay = globalRecoilDecay * recoilDecayBonus;
            
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            return (Main.rand.NextFloat() > ammoConsumeChance);
            
        }

        public static bool isProbablyAGun(Item item)
        {
            return item.damage > 0 && item.DamageType == DamageClass.Ranged && (item.useAmmo == AmmoID.Bullet || item.useAmmo == AmmoID.Rocket || item.useAmmo == AmmoID.Gel || item.useAmmo == AmmoID.Snowball);
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (drawNerveSpeed)
            {
                if (Main.rand.NextBool(3))
                {
                    Terraria.Dust.NewDust(Player.position, Player.width, Player.height, DustID.RedTorch);
                }
                
            }
        }

        // twas debuggin
        //public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        //{
        //    GlowDrawer.DrawGlow(Main.spriteBatch, Player.Center, Color.OrangeRed, 1f, 1f, "KenneyCircleHD", 0f);

        //}
    }
}
