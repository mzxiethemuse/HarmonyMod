using System;
using HarmonyMod.Content.Clusters.BloodMoon.Projectiles;
using HarmonyMod.Content.Dusts;
using HarmonyMod.Content.Dusts.BurstDatas;
using HarmonyMod.Core.Graphics;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.BloodMoon.Items.Weapons;

public class ExecutionerAxe : ModItem
{
    public override void SetDefaults()
    {
        
        Item.Size = new(52, 46);
        
        Item.DamageType = DamageClass.Melee;
        Item.damage = 24;
        Item.crit = 9;
        Item.knockBack = 6f;
        Item.UseSound = SoundID.Item1;
        
        // is this coal or gem? you decide
        (Item.useTime, Item.useAnimation) = (60, 60);
        Item.shoot = ModContent.ProjectileType<ExecutionerAxeSwing>();
        Item.shootSpeed = 1f;
        
        Item.useStyle = ItemUseStyleID.Swing;
        Item.holdStyle = ItemHoldStyleID.None;

        
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.autoReuse = true;
        
        
        Item.axe = 9;
        
        base.SetDefaults();
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<BloodPathogen>(), 6);
        recipe.AddIngredient(ItemID.TungstenBar, 12);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
        
        recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<BloodPathogen>(), 6);
        recipe.AddIngredient(ItemID.SilverBar, 12);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
        base.AddRecipes();
    }
}


public class ExecutionerAxeSwing : SwordSwing
{
    public override string Texture => "HarmonyMod/Content/Clusters/BloodMoon/Items/Weapons/ExecutionerAxe";

    public override SwordSwingType SwingType => SwordSwingType.Basic;
    public override int Width => 52;
    public override int Height => 46;
    
    public override bool Friendly => true;
    public override int Lifetime => 52;

    public override float StartRotation => -MathF.PI / 2  - MathHelper.PiOver4;
    public override float EndRotation => MathF.PI / 3f  + MathHelper.PiOver4;
    public override bool TwoHanded => true;

    public override bool DrawTrail => true;

    public override TrailData Trail => new TrailData(Trails.BasicColorLerp(Color.WhiteSmoke * 0.6f, Color.Transparent),
        Trails.BasicWidthLerp(15, 30), "RainbowRod");

    public override void SetStaticDefaults()
    {

        ProjectileID.Sets.TrailingMode[this.Type] = 3;
        ProjectileID.Sets.TrailCacheLength[this.Type] = 54;

        base.SetStaticDefaults();
    }
    public override float GetLerpValue(float n)
    {
        return Easing.OutQuint(Easing.OutQuad(n));
    }


    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        for (int i = 0; i < 23; i++)
        {
            Vector2 vel = Main.rand.NextVector2Circular(4, 4);
            Terraria.Dust.NewDust(target.position, target.width, target.height, DustID.Blood, vel.X, vel.Y);
        }

        if (hit.Crit || target.life <= 0)
        {
            // for (int i = 0; i < Main.rand.Next(1,3); i++)
            // {
            //     var proj = Projectile.NewProjectileDirect(target.GetSource_OnHurt(Projectile), target.Center, Vector2.Zero,
            //         ModContent.ProjectileType<LeperFlesh>(), 5, 3f);
            //     proj.friendly = true;
            //     
            // }
            
            for (int i = 0; i < 6; i++)
            {
                var d2 = Terraria.Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    ModContent.DustType<HemoptysisDust>(), owner.direction * 1.5f, 0f, 100,
                    new Color(110, 10, 10, 255), 1.2f);
                d2.velocity *= 0.3f;
            }
                
            for (int i = 0; i < 8; i++)
            {
                var d2 = Terraria.Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    ModContent.DustType<Lightspot>(), owner.direction, 0f, 100,
                    Color.Red * 1f, 1.2f);
                d2.velocity *= 0.3f;
            }
            
            
            Burst.SpawnBurst(target.Center, Color.DarkRed * 0.8f, new InBurst(Assets.Assets.VFXCircle, 60, 60f));
            
        }
        base.OnHitNPC(target, hit, damageDone);
    }
    
    public override void ComboLogic(ref int combo)
    {
        if (combo > 2)
        {
            Projectile.damage += 5;
            (startRotation, endRotation) = (startRotation - MathHelper.PiOver4, endRotation + MathHelper.PiOver4);
            combo = 0;
        }
    }
    
    
}