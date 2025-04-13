using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace HarmonyMod.Assets;

public class AssetDirectory : ModSystem
{
    
    public override void Load()
    {
        if (!Main.dedServ)
        {
            foreach (var str in shaders)
            {
                GameShaders.Misc["HarmonyMod:" + str] = new MiscShaderData(ModContent.Request<Effect>(Assets + Effects + str, AssetRequestMode.ImmediateLoad), "Pass1");

            }
           
        }
        base.Load();
    }

    // is this design system stolen from SLR? completely~
    // love u scalar xoxoxooxoxoxoxoxoxoxxoooxooxKil lMe Now
    public static string Assets = "HarmonyMod/Assets/";
    public static string Effects = "Effects/Compiler/";
    public static string Glow = Assets + "Glow/";
    public static string Placeholders = Assets + "Placeholder/";

    public static string[] shaders =
    {
        "Basic",
        "GuardShader"
    };
}