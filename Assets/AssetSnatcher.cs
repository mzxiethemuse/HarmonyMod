using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace HarmonyMod.Assets;

public class Assets : ModSystem
{
    
    
    
    public static Dictionary<string, Asset<Texture2D>> Textures = new();
    
    public override void Load()
    {
        LoadPath("Placeholder", placeholder);
        LoadPath("Glow", glow);

    }

    private void LoadPath(string path, string[] names)
    {
        foreach (string file in names)
        {
            Textures[file] = Mod.Assets.Request<Texture2D>("Assets/" + path + "/" + file, AssetRequestMode.ImmediateLoad);
        }
    }

    public static string prefix = "HarmonyMod/Assets/";
    public static string[] placeholder = ["GenericItem"];
    public static string[] glow = [
        "Explosion", 
        "ExplosionBlurred",
        "smoke_01",
        "smoke_02",
        "smoke_03",
        "smoke_04",
        "smoke_05",
        "smoke_05",
        "smoke_06",
        "smoke_07",
        "smoke_08",
        "smoke_09",
        "light_01",
        "light_02",
        "light_03",
        "star_03",

    "whiteball"];

}