using Microsoft.Xna.Framework;
using Terraria;

namespace HarmonyMod.Content.Dust.BurstDatas;

public class FollowPlayerBurst : BurstData
{
    public int owner;
    public FollowPlayerBurst(string texture, float duration, float radius, int owner) : base(texture, duration, radius) {this.owner = owner;}

    public override void AI(ref Terraria.Dust dust)
    {
        dust.position = Vector2.Lerp(dust.position, Main.player[owner].Center, 0.9f);
    }
}