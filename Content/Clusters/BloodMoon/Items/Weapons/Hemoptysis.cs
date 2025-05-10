using HarmonyMod.Asset;
using HarmonyMod.Content.Dust;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.BloodMoon.Items.Weapons;

public class Hemoptysis : ModItem
{
    public override string Texture => AssetDirectory.Placeholders + "GenericItem";
    
    public override void SetDefaults()
    {
        Item.Size = new Vector2(26, 26);
        Item.DamageType = DamageClass.Magic;
        Item.damage = 25;
        Item.useTime = 60;
        Item.useAnimation = 60;
        Item.channel = true;
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.shoot = ModContent.ProjectileType<HemoptysisSneeze>();
        Item.shootSpeed = 0.1f;
        Item.useStyle = ItemUseStyleID.HiddenAnimation;
        base.SetDefaults();
    }


    public override Color? GetAlpha(Color lightColor)
    {
        return Color.PaleVioletRed;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient<BloodPathogen>(12);
        recipe.Register();
    }
}

public class HemoptysisSneeze : ModProjectile
{
    public override string Texture => AssetDirectory.Placeholders + "GenericItem";

    public override void SetDefaults()
    {
        Projectile.hide = true;
        Projectile.Size = new Vector2(70, 30);
        Projectile.timeLeft = 200;
        Projectile.DamageType = DamageClass.Magic;
        base.SetDefaults();
    }

    public override void AI()
    {
        Projectile.friendly = false;
        Player owner = Main.player[Projectile.owner];
        Projectile.Center = owner.Center + new Vector2(Projectile.Size.X / 2 * owner.direction, -8);
        if (owner.channel)
        {
            if (Main.rand.NextBool((int)(60 - Projectile.ai[0]) / 4))
            {
                Terraria.Dust.NewDustDirect(owner.position, owner.width, owner.height, ModContent.DustType<HemoptysisDust>(), 0,
                    0.7f, 100, Color.Red).velocity.X = 0.01f;
            }

            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 55)
            {
                SoundEngine.PlaySound(Assets.BloodSneeze.WithPitchOffset(0.7f), Projectile.Center);
                

                DustEmitter.Emit(DustID.Blood, Projectile.position, Projectile.width, Projectile.height, 15).ForEach((
                    dust =>
                    {
                        dust.velocity = new Vector2(1 * owner.direction, -1f);
                    }));
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
                Projectile.friendly = true;
                if (Projectile.ai[0] > 55)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                
            }
        } else Projectile.Kill();
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(ModContent.BuffType<HemoptysisCurse>(), 60*6);
    }
}

public class HemoptysisCurse : ModBuff {
    public override string Texture => AssetDirectory.Placeholders + "GenericDebuff";

    public override void Update(NPC npc, ref int buffIndex)
    {
        if (Main.rand.NextBool(4))
        {
            Terraria.Dust.NewDustDirect(npc.position, npc.width, npc.height,
                ModContent.DustType<Lightspot>(), 0, 0.7f, 100,
                Color.Red, 0.9f).velocity.X = 0;
        }
    }
}