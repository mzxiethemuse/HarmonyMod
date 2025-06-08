using HarmonyMod.Content.Dusts;
using HarmonyMod.Content.Projectiles;
using HarmonyMod.Core.Graphics;
using HarmonyMod.Core.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.GoblinArmy.NPCs;

public class GoblinSuicideBomber : ComplexNPC
{
    public static DustEmitter smokeEmitter = new DustEmitter(ModContent.DustType<FancySmoke>(), 3f, 4f,
        new Vector2(0.5f, 0.5f), new Color(20, 20, 20, 255), Color.Black, 2);

    public static DustEmitter blink = new DustEmitter(DustID.Flare, 0.5f, 1f, new Vector2(0.5f, 1f), Color.Red);
    public override void SetDefaults()
    {
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;

        Acceleration = 0.1f;
        JumpHeight = 8f;
        NPC.lifeMax = 50;
        NPC.defense = 10;
        NPC.width = 28;
        NPC.height = 44;
        NPC.damage = 0;
        
        base.SetDefaults();
    }
    
    /*public override void SetStaticDefaults()
    {
        NPCID.Sets.BelongsToInvasionGoblinArmy[Type] = true;
    }*/
    
    

    public override void AI()
    {
        if (State == 0)
        {
            NPC.TargetClosest(true);
            var playerPos = GetTarget().Center;
            Walk(playerPos, 6f, true, 30f);
            if (NPC.Center.Distance(playerPos) < 50f)
            {
                State = 1;
                Timer = 0;
            }
        } else if (State == 1)
        {
            NPC.ai[2] += 1;
            Decelerate();
            Timer++;
            if (Timer % (30 - NPC.ai[2]) == 0)
            {
                blink.Emit(NPC.position, NPC.width, NPC.height, 4);
                SoundEngine.PlaySound(SoundID.MaxMana.WithPitchOffset(1.5f));
            }
            if (Timer > 30)
            {
                // smokeEmitter.Emit(DustID.Smoke, NPC.position, NPC.width, NPC.height, 30);
                foreach (Terraria.Dust dust in smokeEmitter.Emit(NPC.position, NPC.width, NPC.height, 30))
                {
                    dust.customData = Main.rand.Next(1, 6);
                }
                CameraUtils.AddScreenshakeModifier(NPC.Center, Vector2.UnitY, 3f, 2, 40);

                SoundEngine.PlaySound(SoundID.Item62);
                SoundEngine.PlaySound(SoundID.Item14);
                Burst.SpawnBurst(Assets.Assets.VFXCircle, NPC.Center, Color.Red, 100f, 50);
                Hitbox.SpawnHitbox(NPC.GetSource_FromAI(), NPC.Center, 50, 50, 50,22, NPC.whoAmI, false, true);

                // Explosion.SpawnExplosion<Explosion>(NPC.GetSource_FromAI(), NPC.Center, 50, 3f, Main.myPlayer, 100f, 50, Color.OrangeRed, 30, false, 0.45f);
                NPC.StrikeInstantKill();
            }
        }

        base.AI();
    }
}