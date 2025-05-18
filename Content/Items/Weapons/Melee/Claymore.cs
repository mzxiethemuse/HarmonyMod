using System;
using HarmonyMod.Asset;
using HarmonyMod.Content.Clusters.GoblinArmy;
using HarmonyMod.Content.Clusters.GoblinArmy.Weapons;
using HarmonyMod.Content.Dust;
using HarmonyMod.Content.Dust.BurstDatas;
using HarmonyMod.Core.Graphics;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Items.Weapons.Melee;

public class Claymore : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 54;
        Item.height = 54;
        Item.damage = 23;
        Item.knockBack = 4f;
        Item.useTime = 46;
        Item.useAnimation = 46;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.autoReuse = true;
        Item.rare = ItemRarityID.Blue;
        Item.crit = 6;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.holdStyle = ItemHoldStyleID.HoldHeavy;
        Item.UseSound = SoundID.Item1;
        Item.DamageType = DamageClass.Melee;
        Item.scale = 2f;
        Item.shoot = ModContent.ProjectileType<ClaymoreSwing>();
        Item.shootSpeed = 2f;

        base.SetDefaults();
    }

    public override bool? UseItem(Player player)
    {
        if (player.altFunctionUse == 2 && player.whoAmI == Main.myPlayer)
        {
            
            player.velocity.X += 7.5f * player.direction;
            var proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<ClaymoreSwing>(), 10, 14.5f, player.whoAmI);

            if (proj.ModProjectile is ClaymoreSwing projectile)
            {
                (projectile.startRotation, projectile.endRotation) = (projectile.endRotation, projectile.startRotation);
            }
            else
            {
                return false;
            }
        }

        return null;
    }
    

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity,
        int type,
        int damage, float knockback)
    {
        if (player.altFunctionUse != 2)
        {
            player.GetModPlayer<SwingPlayer>().swing *= -1;
            var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback,
                player.whoAmI);
            // if (proj.ModProjectile is ClaymoreSwing projectile && player.GetModPlayer<SwingPlayer>().swing == 1)
            // {
            //     (projectile.startRotation, projectile.endRotation) = (projectile.endRotation, projectile.startRotation);
            // }}
        }
        return false;

    }

    public override bool AltFunctionUse(Player player)
    {
        return true;
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(RecipeGroupID.IronBar, 25);
        recipe.AddIngredient(RecipeGroupID.Wood, 12);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }
}

public class ClaymoreSwing : SwordSwing
{
    public override SwordSwingType SwingType => SwordSwingType.Basic;

    public override string Texture => "HarmonyMod/Content/Items/Weapons/Melee/Claymore";
    public override int Width => 54;
    public override int Height => 54;
    public override bool Friendly => true;
    public override int Lifetime => 46;

    public override float StartRotation => -MathF.PI / 2;
    public override float EndRotation => MathF.PI / 1.5f;
    public override bool TwoHanded => true;
    //
    // public override bool DrawTrail => true;
    //
    // public override TrailData Trail => new TrailData(Trails.BasicColorLerp(Color.WhiteSmoke * 0.4f, Color.Transparent), Trails.BasicWidthLerp(1, 36), "LightDisc");

    public override float GetLerpValue(float n)
    {
        return Easing.OutQuint(n);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        for (int i = 0; i < Main.rand.Next(1,4); i++)
        {
            Burst.SpawnBurst(Main.rand.NextVector2FromRectangle(target.Hitbox), Color.WhiteSmoke * 0.5f,
                new InBurst(Assets.VFXStar[3], 16, 18));
        }
        base.OnHitNPC(target, hit, damageDone);
    }
    
    public override void SetStaticDefaults()
    {

        ProjectileID.Sets.TrailingMode[this.Type] = 3;
        ProjectileID.Sets.TrailCacheLength[this.Type] = 34;

        base.SetStaticDefaults();
    }

    public override void ComboLogic(ref int combo)
    {
        if (combo > 2)
        {
            (startRotation, endRotation) = (StartRotation, EndRotation);
            endRotation = startRotation + (MathF.PI * 2);
            combo = 0;
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        // Trails.DrawTrail(Projectile.oldPos, Projectile.oldRot, Projectile.Size, Trails.BasicColorLerp(Color.GhostWhite * (Projectile.timeLeft / (float)Lifetime), Color.Transparent), Trails.BasicWidthLerp(1, 12), 2f, 0.5f, "LightDisc");
        return true;
    }
}