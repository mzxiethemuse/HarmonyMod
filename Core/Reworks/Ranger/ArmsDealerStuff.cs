using HarmonyMod.Content.Items.Accessories.Ranger;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HarmonyMod.Core.Reworks.Ranger
{
    public class ArmsDealerStuff : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.ArmsDealer;
        public override void ModifyShop(NPCShop shop)
        {
            shop.Add(ModContent.ItemType<GunGrip>(), Condition.PreHardmode);
            shop.Add(ModContent.ItemType<ExtendedMag>(), Condition.DownedEowOrBoc);
            shop.Add(ModContent.ItemType<StabilizerGrip>(), Condition.DownedEowOrBoc);


            base.ModifyShop(shop);
        }
    }
}
