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
    public bool Bezoar = false;

    public override void ResetEffects()
    {
        Bezoar = false;
        StellarGut = false;
    }

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        modifiers.ModifyHurtInfo += (ref Player.HurtInfo info) =>
        {
            if (StellarGut && info.Damage < 5 && !Player.immune && !info.Cancelled)
            {
                DustEmitter.Emit(DustID.ManaRegeneration, Player.position, Player.width, Player.height, 10);
                SoundEngine.PlaySound(SoundID.NPCDeath13.WithPitchOffset(1.5f), Player.Center);

                SoundEngine.PlaySound(SoundID.Item3.WithPitchOffset(0.3f), Player.Center);
                info.Cancelled = true;
                
                Player.immune = true;
                Player.AddImmuneTime(ImmunityCooldownID.General, 60);
                Player.UpdateImmunity();
                var manaAmt = (int)MathF.Max(info.Damage, 5);
                Player.ManaEffect(manaAmt);
                Player.statMana = (int)MathF.Min(Player.statManaMax2, Player.statMana + manaAmt);
                
            }
        };
        
    }

    public override void UpdateEquips()
    {
        if (Bezoar && Player.statLife < 100)
        {
            if (Player.CheckMana(3))
            {
                Terraria.Dust.NewDust(Player.position, Player.width, Player.height, DustID.ManaRegeneration);
                Player.statMana -= 3;
                if (Player.statLife + 1 < Player.statLifeMax2)
                {
                    Player.statLife += 1;
                }
            }
        }
    }
    
    
}