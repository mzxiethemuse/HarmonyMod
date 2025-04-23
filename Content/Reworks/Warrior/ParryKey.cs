using Terraria;
using Terraria.ModLoader;

namespace HarmonyMod.Content.Reworks.Warrior;

public class ParryKey : ModSystem
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

