using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace HarmonyMod.Content.UI;

public class ThrowingPreview : ModSystem
{
    public static Texture2D tex = ModContent.Request<Texture2D>("HarmonyMod/Content/UI/Track").Value;
    private static int Fuck = 4;

    /*public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex != -1) {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "HarmonyMod: ThrowingOverlay",
                delegate
                {
                    if (Main.dedServ) return true;
                    Main.spriteBatch.Draw(TextureAssets.Sun2.Value, Main.LocalPlayer.Center - Main.screenPosition, Color.White);
    
                    if (Main.LocalPlayer.HeldItem.DamageType == DamageClass.Throwing && Main.LocalPlayer.HeldItem.shoot != ProjectileID.None)
                    {
    
                        Vector2 position = Main.LocalPlayer.Center; 
                        Vector2 dir = Main.LocalPlayer.Center.DirectionTo(Main.MouseWorld) * Main.LocalPlayer.HeldItem.shootSpeed;
                        Vector2 velocity = dir;
                        // now we need to draw the actual trail
                        
                        for (int i = 0; i < 60 / Fuck; i++)
                        {
    
                            Main.spriteBatch.Draw(TextureAssets.Sun2.Value, position - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                            // Main.NewText(position - Main.screenPosition);
                            velocity.Y += 0.7f * Fuck;
                            position += velocity * Fuck;
                        }
                    }
                    return true;
                },
                InterfaceScaleType.UI));
        }
    }*/
}