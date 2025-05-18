using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Items.Weapons.Throwing;

public class ObsidianCaltrops : ModItem
{
    public override void SetDefaults()
    {
        Item.useTime = 30;
        Item.useAnimation = 30;

        Item.damage = 22;
        Item.DamageType = DamageClass.Throwing;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.autoReuse = true;
        Item.width = 28;
        Item.consumable = true;
        Item.maxStack = 999;
        Item.height = 24;
        Item.UseSound = SoundID.Item1;
        Item.shootSpeed = 9.5f;
        Item.ArmorPenetration = 4;
        Item.shoot = ModContent.ProjectileType<ObsidianCaltropProjectile>();

    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type,
        int damage, float knockback)
    {
        float dist = 1000 - player.Center.Distance(Main.MouseWorld);
        double angle = dist * -0.0005f;
        double peepee = angle;
        for (int i = 0; i < 3; i++)
        {
            Projectile.NewProjectile(source, position, velocity.RotatedBy(peepee), type, damage / 4, knockback, player.whoAmI);
            peepee -= angle;
        }

        return false;
    }
    
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.Obsidian, 2);
        recipe.AddTile(TileID.Hellforge);
        recipe.Register();
    }
}

public class ObsidianCaltropProjectile : ModProjectile 
{
    // public override void SetStaticDefaults()
    // {
    //     WarriorParry.parryActions.Add(ModContent.ProjectileType<ObsidianCaltropProjectile>(),
    //         (player, projectile) =>
    //         {
    //             if (Main.myPlayer == player.whoAmI)
    //             {
    //                 projectile.velocity = player.DirectionTo(Main.MouseWorld) * 20;
    //                 projectile.damage += 3;
    //                 projectile.timeLeft = 600;
    //                 projectile.ai[2] = 69f;
    //             }
    //         }
    //         );
    // }

    public override void SetDefaults()
    {
        Projectile.width = 28;
        Projectile.height = 24;
        Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
        Projectile.usesIDStaticNPCImmunity = true;
        Projectile.idStaticNPCHitCooldown = 20;
        Projectile.penetrate = 6;
        Projectile.timeLeft = 600;
        Projectile.friendly = true;
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        // Projectile.velocity = oldVelocity * new Vector2(0.4f, -0.4f);
        return false;
    }

    public override void PostAI()
    {
        if (Projectile.ai[2] == 69f && Projectile.velocity.Length() > 2f)
        {
            Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0, 0, 0, default(Color), 0.8f);
        }
    }

    public override void OnKill(int timeLeft)
    {
        for (int i = 0; i < 8; i++)
        {
            Terraria.Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Obsidian);
        }
    }
    
    
}