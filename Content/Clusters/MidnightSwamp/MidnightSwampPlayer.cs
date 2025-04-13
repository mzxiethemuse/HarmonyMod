using System;
using HarmonyMod.Content.Dust;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Clusters.MidnightSwamp;

public class MidnightSwampPlayer : ModPlayer
{
    public bool StellarGut = false;

    public override void ResetEffects()
    {
        StellarGut = false;
    }

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        modifiers.ModifyHurtInfo += (ref Player.HurtInfo info) =>
        {
            if (StellarGut && info.Damage < 10 && !Player.immune)
            {
                DustEmitter.Emit(DustID.ManaRegeneration, Player.position, Player.width, Player.height, 10);
                SoundEngine.PlaySound(SoundID.NPCDeath13.WithPitchOffset(1.5f), Player.Center);

                SoundEngine.PlaySound(SoundID.Item3.WithPitchOffset(0.3f), Player.Center);
                info.Cancelled = true;
                
                Player.immune = true;
                Player.AddImmuneTime(ImmunityCooldownID.General, 60);
                Player.UpdateImmunity();
                Player.ManaEffect((int)MathF.Max(info.Damage, 5));
            }
        };
        
    }
    
}