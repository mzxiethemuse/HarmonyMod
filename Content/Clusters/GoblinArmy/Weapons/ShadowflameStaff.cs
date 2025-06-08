using HarmonyMod.Content.Dusts;
using HarmonyMod.Content.Projectiles;
using HarmonyMod.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.GoblinArmy.Weapons;

public class ShadowflameStaff : ModItem
{
    // public override string Texture => AssetDirectory.Placeholders + "GenericItem";

    public override void SetStaticDefaults()
    {
        Item.staff[Type] = true; 
    }

    public override void SetDefaults()
    {
        Item.width = 48;
        Item.height = 52;
        Item.mana = 11;
        Item.useTime = 25;
        Item.useAnimation = 25;
        // Item.holdStyle = ItemHoldStyleID.HoldHeavy;
        Item.useStyle = ItemUseStyleID.Shoot;

        Item.shoot = ModContent.ProjectileType<ShadowflameBolt>();
        Item.shootSpeed = 8f;
        
        Item.DamageType = DamageClass.Magic;
        Item.damage = 13;
        Item.knockBack = 4f;
        
        Item.UseSound = SoundID.Item8.WithPitchOffset(1.2f);
    }
    
        // public override void HoldStyle(Player player, Rectangle heldItemFrame)
        // {
        //     if (player.whoAmI == Main.myPlayer)
        //     {
        //         player.itemLocation = Vector2.Zero;
        //         player.itemRotation = player.Center.DirectionTo(Main.MouseWorld).ToRotation();
        //     }
        // }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<KnarledWood>(), 20);
        recipe.AddIngredient(ItemID.Amethyst, 4);

        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}

public class ShadowflameBolt : ModProjectile
{
    public override string Texture => $"Terraria/Images/NPC_{NPCID.ChaosBall}";

    public static DustEmitter burst = new DustEmitter(DustID.Shadowflame);
    
    public override void SetStaticDefaults()
    {
        burst.SetVelocitySpread(new Vector2(1, 1));
        burst.SetScaleRange(0.5f, 1);
        
        ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        ProjectileID.Sets.TrailCacheLength[Projectile.type] = 45;
    }

    public override void SetDefaults()
    {
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.timeLeft = 600;
        Projectile.penetrate = 5;
        Projectile.aiStyle = 29;
        Projectile.width = 16;
        Projectile.height = 16;
        Projectile.tileCollide = true;
    }
    

    public override Color? GetAlpha(Color lightColor)
    {
        return Color.White * 0.5f;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Trails.DrawTrail(Projectile.oldPos, Projectile.oldRot, Projectile.Size, Trails.BasicColorLerp(Color.BlueViolet, Color.BlueViolet), Trails.BasicWidthLerp(4f, 28f), -2.8f, 1.7f, "RainbowRod");

        return true;
    }

    public override void PostAI()
    {
        for (int i = 0; i < 2; i++)
        {
            var dustt = Terraria.Dust.NewDustDirect(Projectile.position, 8, 8, DustID.Shadowflame, 0, 0, 100, default, 1f);
            dustt.velocity *= 0.1f;

        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        SoundEngine.PlaySound(SoundID.Item15.WithPitchOffset(1f).WithVolumeScale(0.5f), Projectile.position);

        // Explosion.SpawnExplosion<Explosion>(Projectile.Center, 0, 0f, Projectile.owner, 20f - Projectile.penetrate * 4, 23, Color.DarkViolet, 30);
        Projectile.penetrate--;
        Projectile.velocity = oldVelocity;
        // Main.NewText(Projectile.penetrate);
        return (Projectile.penetrate == 0);
    }

    public override void OnKill(int timeLeft)
    {
        burst.Emit(Projectile.position, 16, 16, 32);
        SoundEngine.PlaySound(SoundID.Item110, Projectile.position);
        Burst.SpawnBurst(Assets.Assets.VFXStar[2], Projectile.Center, Color.BlueViolet, 36f, 44);
        // Explosion.SpawnExplosion<MagicExplosion>(Projectile.Center, 0, 0f, Projectile.owner, 36f, 44, Color.BlueViolet, 30);
    }
    
    Color TrailColor(float progress) => Color.Lerp(Color.White * 0.5f, Color.Violet.MultiplyRGBA(new Color (1, 1, 1, 0.5f)), progress);
}