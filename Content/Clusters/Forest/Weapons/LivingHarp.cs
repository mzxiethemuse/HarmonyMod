using System;
using System.Linq;
using HarmonyMod.Assets;
using HarmonyMod.Content.Clusters.Forest.Projectiles;
using HarmonyMod.Core.Graphics;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.Forest.Weapons;

public class LivingHarp : ModItem
{
    
    // creates strings at the mouse cursor. projectiles and players that pass through the strings strum them
    // when strings are strummed, they release a small amount of healing
    //enemies that pass through the strings take some damage
    
    // strings; render a line, offset it with a sin wave, add some fun coloring, everyones happy
    
    public override string Texture => AssetDirectory.Placeholders + "GenericItem";

    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 24;
        Item.damage = 9;
        
        // summon damage affects healing, magic damage affects.. damage
        Item.DamageType = DamageClass.Magic;
        
        Item.useTime = 47;
        Item.useAnimation = 47;
        Item.holdStyle = ItemHoldStyleID.HoldGuitar;
        Item.useStyle = ItemUseStyleID.Guitar;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.autoReuse = true;
        Item.mana = 10;
        Item.channel = true; 
        Item.InterruptChannelOnHurt = true;
    }

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            Projectile.NewProjectile(player.GetSource_ItemUse(Item),
                Main.MouseWorld + Main.rand.NextVector2Circular(120, 40), Vector2.Zero,
                ModContent.ProjectileType<LifeString>(), Item.damage, 2f, player.whoAmI);
        }
        return true;
    }
}

public class LifeString : ModProjectile
{
    public static Asset<Texture2D> HarpNutTexture = ModContent.Request<Texture2D>(AssetDirectory.Content + "Clusters/Forest/Weapons/HarpNut");
    public Vector2 corner => Projectile.position + Projectile.Size;
    
    // time until can be strummed again
    public float strumTime
    {
        get => Projectile.ai[1]; 
        set => Projectile.ai[1] = value;
    }
    public float offset
    {
        get => Projectile.ai[2];
        set => Projectile.ai[2] = value;
    }

    public override string Texture => AssetDirectory.Placeholders + "Fuck";

    public override void SetDefaults()
    {
        Projectile.penetrate = -1;
        Projectile.damage = 20;
        Projectile.width = 24;
        Projectile.height = 24 * 7;
        Projectile.timeLeft = 900;
        Projectile.friendly = true;
        Projectile.hostile = true;
    }

    public override void AI()
    {
        strumTime -= 1;
        strumTime *= 0.97f;
        var n = 0;
        foreach (var proj in Main.projectile)
        {
            if (proj.active)
            {
                if (proj.type != Projectile.type)
                {
                    if (Projectile.Colliding(Projectile.Hitbox, proj.Hitbox))
                    {
                        strumTime = 60f;
                        break;
                    }
                }
                // else if (proj.whoAmI != Projectile.whoAmI && proj.timeLeft > 25 && Projectile.timeLeft > 25)
                // {
                //     n++;
                //     if (n > 3 && proj.ai[0] == 1)
                //     {
                //         foreach (var proj2 in Main.projectile.Where(proj2 =>
                //                      proj2.active && proj2.type == Projectile.type))
                //         {
                //             proj2.ai[0]--;
                //         }
                //         proj.timeLeft = 25;
                //     }
                // }
            }
        }
    }

    public override void OnSpawn(IEntitySource source)
    {
        var startpos = Projectile.position;
        Projectile.width = Main.rand.Next(25, 37);
        Projectile.height = Main.rand.Next(81, 181);
        Projectile.Center = startpos + Projectile.Size / 2f;
        strumTime = 60f;
        offset = Main.rand.Next(0, Projectile.width);
        var count = Main.projectile.Count(projectile => projectile.type == Projectile.type && projectile.active && projectile.timeLeft > 25);
        Projectile.ai[0] = count;
        if (count == 4)
        {
            foreach (var proj in Main.projectile.Where(projectile => projectile.type == Projectile.type && projectile.active && projectile.timeLeft > 25))
            {
                proj.ai[0]--;
                if (proj.ai[0] == 0)
                {
                    proj.timeLeft = 25;
                }
            }
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D pixel = TextureAssets.FishingLine.Value;
        Rectangle placeholder = pixel.Bounds;
        Vector2 pos1 = Projectile.position + new Vector2(offset, 0);
        Vector2 pos2 = corner - new Vector2(offset, 0);
        Vector2 Penis = HarpNutTexture.Size();
        float alpha = MathF.Min((900 - Projectile.timeLeft) / 15f, 1);
        float alpha2 = MathF.Min(Projectile.timeLeft / 20f, 1);
        float finalAlpha = MathF.Min(alpha, alpha2);
        


        for (int i = 0; i < 100; i++)
        {
            float strum = MathF.Max(0, MathF.Min(strumTime, 60));
            float a = i / 100f;
            
            
            var angle = pos1.DirectionTo(pos2).ToRotation();
            var pos3 = Vector2.Lerp(pos1, pos2, a) + Penis + new Vector2(0, MathF.Sin(i * strum / 220f + (float)Main.timeForVisualEffects) * strum / 4 * LerpUtils.Window(a)).RotatedBy(angle);
            Main.spriteBatch.Draw(pixel, 
                (pos3 - Main.screenPosition), 
                new Rectangle(0, 0, 6, 2),
                 Color.Lerp(Color.LimeGreen, Color.ForestGreen, LerpUtils.RectSin(LerpUtils.PhaseShift(a, Projectile.timeLeft / 100f), 1 + strum / 90)) * 0.6f * finalAlpha,
                angle,
                pixel.Size() / 2, 
                1.5f,
                SpriteEffects.None,
                1f);
        }
        
        // stupid dumb centering shit on this bro im gonn KMS
        Main.spriteBatch.Draw(HarpNutTexture.Value, pos1 - Main.screenPosition + new Vector2(12, 6), null, Color.White * finalAlpha);//, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        Main.spriteBatch.Draw(HarpNutTexture.Value, pos2 - Main.screenPosition + new Vector2(12, 6), null, Color.White * finalAlpha);//, 0f, HarpNutTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
        return false;
    }

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
    {
        strumTime = 120;
        for (int i = 0; i < Main.rand.Next(3,7); i++)
        {
            Projectile.NewProjectile(
                Projectile.GetSource_FromAI(),
                Projectile.Center,
                new Vector2(Main.rand.NextFloat(-1.5f, 1.51f), -4.5f),
                ModContent.ProjectileType<Horb>(),
                2,
                0f,
                Projectile.owner
            );
        }
        modifiers.Cancel();
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        strumTime = 60;
    }

    public override bool CanHitPlayer(Player target) => strumTime <= 0;

    public override bool? CanHitNPC(NPC target) => strumTime <= 0;
}
