using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Core.Util;

public abstract class BaseWhipProjectile : ModProjectile
{
	public virtual Vector2 HandOrigin => new Vector2(5, 8);
	public virtual Vector2 HandleSize => new Vector2(10, 26);
	public virtual int Segment1Height => 16;
	public virtual int Segment2Height => 16;
	public virtual int Segment3Height => 16;
public virtual int HeadLength => 18;
    /*
    Yes i did steal from ExampleMod
    Yes I don't have a job
    Yes ts ts pmo pmo 
    */
    public override void SetStaticDefaults() {
        // This makes the projectile use whip collision detection and allows flasks to be applied to it.
        ProjectileID.Sets.IsAWhip[Type] = true;
    }

    public override void SetDefaults() {
        // This method quickly sets the whip's properties.
        Projectile.DefaultToWhip();

        // use these to change from the vanilla defaults
        // Projectile.WhipSettings.Segments = 20;
        // Projectile.WhipSettings.RangeMultiplier = 1f;
    }
    
    public override bool PreDraw(ref Color lightColor) {
			List<Vector2> list = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, list);

			// DrawLine(list);

			//Main.DrawWhip_WhipBland(Projectile, list);
			// The code below is for custom drawing.
			// If you don't want that, you can remove it all and instead call one of vanilla's DrawWhip methods, like above.
			// However, you must adhere to how they draw if you do.

			SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Vector2 pos = list[0];

			for (int i = 0; i < list.Count - 1; i++) {
				// These two values are set to suit this projectile's sprite, but won't necessarily work for your own.
				// You can change them if they don't!
				Rectangle frame = new Rectangle(0, 0, (int)HandleSize.X, (int)HandleSize.Y); // The size of the Handle (measured in pixels)
				Vector2 origin = HandOrigin; // Offset for where the player's hand will start measured from the top left of the image.
				float scale = 1;

				// These statements determine what part of the spritesheet to draw for the current segment.
				// They can also be changed to suit your sprite.
				if (i == list.Count - 2) {
					// This is the head of the whip. You need to measure the sprite to figure out these values.
					frame.Y = (int)HandleSize.Y + Segment1Height + Segment2Height + Segment3Height; // Distance from the top of the sprite to the start of the frame.
					frame.Height = HeadLength; // Height of the frame.

					// For a more impactful look, this scales the tip of the whip up when fully extended, and down when curled up.
					Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
					float t =  Projectile.ai[0] / timeToFlyOut;
					scale = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
				}
				else if (i > 10) {
					// Third segment
					frame.Y = (int)HandleSize.Y + Segment1Height + Segment2Height;
					frame.Height = Segment3Height;
				}
				else if (i > 5) {
					// Second Segment
					frame.Y = (int)HandleSize.Y + Segment1Height;// + Segment2Y + Segment3Y;
					frame.Height = Segment2Height;
				}
				else if (i > 0) {
					// First Segment
					frame.Y = (int)HandleSize.Y;
					frame.Height = Segment1Height;
				}

				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is used to correct rotation.
				Color color = Lighting.GetColor(element.ToTileCoordinates());

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

				pos += diff;
			}
			return false;
		}
    
    
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
			Projectile.damage = (int)(Projectile.damage * 0.5f); // Multihit penalty. Decrease the damage the more enemies the whip hits.
		}
}