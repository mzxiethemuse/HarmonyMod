using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Core;

public class HarmonyPlayer : ModPlayer
{
    public int TimeSinceYVelocityZero = 0;
    public int TimeSinceLastHurt = 0;
    public bool communionEquipped;

    public override void ResetEffects()
    {
        communionEquipped = false;
    }

    public override void UpdateEquips()
    {
        base.UpdateEquips();
    }

    public override void PostUpdate()
    {

        TimeSinceYVelocityZero++;
        TimeSinceLastHurt++;
        
        if (Player.velocity.Y == 0)
        {
            TimeSinceYVelocityZero = 0;
        }

    }

    public override void OnHurt(Player.HurtInfo info)
    {
        if (communionEquipped)
        {
            SoundEngine.PlaySound(SoundID.Shatter.WithPitchOffset(1.15f), Player.Center);
        }
        TimeSinceLastHurt = 0;
    }
    
    
}