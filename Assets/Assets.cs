using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace HarmonyMod.Assets;

public class Assets : ModSystem
{

    public override void Load()
    {
        if (!Main.dedServ)
        {
            foreach (var str in MiscShaders)
            {
                GameShaders.Misc["HarmonyMod:" + str] = new MiscShaderData(ModContent.Request<Effect>(AssetDirectory.Assets + AssetDirectory.Effects + str ), "Pass1");

            }
        }
        base.Load();
    }
    public static string[] MiscShaders =
    {
        "Basic",
        "GuardShader",
        "ForestGradient",
        "Outline",
        "BasicTrail"
    };
    
    //    public static readonly Asset<Texture2D> name = ModContent.Request<Texture2D>(AssetDirectory.Glow + "file" );

    public static readonly Asset<Texture2D> PlaceholderCube = ModContent.Request<Texture2D>(AssetDirectory.Placeholders + "GenericItem" );
    public static readonly Asset<Texture2D> Fuck = ModContent.Request<Texture2D>(AssetDirectory.Placeholders + "Fuck" );
    public static readonly Asset<Texture2D> Whiteball = ModContent.Request<Texture2D>(AssetDirectory.Placeholders + "whiteball" );

    
    public static readonly Asset<Texture2D> VFXCircle = ModContent.Request<Texture2D>(AssetDirectory.Glow + "Explosion" );
    public static readonly Asset<Texture2D> VFXCircle2 = ModContent.Request<Texture2D>(AssetDirectory.Glow + "Explosion2" );
    public static readonly Asset<Texture2D> VFXCircleBlurred = ModContent.Request<Texture2D>(AssetDirectory.Glow + "ExplosionBlurred" );
    public static readonly Asset<Texture2D>[] VFXSmoke = FillTextureListArrayThing(AssetDirectory.Glow + "smoke", 10);
    public static readonly Asset<Texture2D>[] VFXStar = FillTextureListArrayThing(AssetDirectory.Glow + "star", 9);
    public static readonly Asset<Texture2D>[] VFXMagic = FillTextureListArrayThing(AssetDirectory.Glow + "magic", 5);
    public static readonly Asset<Texture2D>[] VFXLight = FillTextureListArrayThing(AssetDirectory.Glow + "light", 3);
    public static readonly Asset<Texture2D>[] VFXScorch = FillTextureListArrayThing(AssetDirectory.Glow + "scorch", 3);

    public static readonly SoundStyle BloodSneeze = new SoundStyle(AssetDirectory.Sound + "baby_brim_1");
    public static readonly SoundStyle SickleSwing = new SoundStyle(AssetDirectory.Sound + "SwordSound-old1");



    private static Asset<Texture2D>[] FillTextureListArrayThing(string name, int count)
    {
        var o = new Asset<Texture2D>[count];
        for (int i = 0; i < count; i++)
        {
            string str = (i + 1).ToString();
            if ((i + 1) < 10) str = "0" + str;
            o[i] = ModContent.Request<Texture2D>(name + "_" + str );
        }

        return o;
    }
}
