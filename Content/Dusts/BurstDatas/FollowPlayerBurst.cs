using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace HarmonyMod.Content.Dusts.BurstDatas;

public class FollowPlayerBurst : BurstData
{
    public int owner;
    public FollowPlayerBurst(Asset<Texture2D> texture, float duration, float radius, int owner) : base(texture, duration, radius) {this.owner = owner;}

    public override void AI(ref Terraria.Dust dust)
    {
        dust.position = Main.player[owner].Center + new Vector2(0, Main.player[owner].gfxOffY);
    }
}