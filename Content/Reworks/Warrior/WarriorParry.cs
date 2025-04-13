using Terraria;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Reworks.Warrior;

public class WarriorParry : ModSystem
{
    public static ModKeybind parryKeybind;

    public override void Load()
    {
        parryKeybind = KeybindLoader.RegisterKeybind(Mod, "Parry", "G");
    }

    public override void Unload()
    {
        base.Unload();
    }
}

public class ParryNPC : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public void OnParried(NPC npc)
    {
        
    }
}