using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace HarmonyMod.Content.Clusters.GoblinArmy;

public class GoblinWarfareSystem : ModSystem
{
    public int GoblinWarsFought = -1;
    
    public override void Load()
    {
        On_Main.StartInvasion += On_MainOnStartInvasion;

        base.Load();
    }

    private void On_MainOnStartInvasion(On_Main.orig_StartInvasion orig, int type)
    {
        GlobalGoblin.deadGoblins.Clear();
        orig.Invoke(type);
        if (type == 1)
        {
            GoblinWarsFought++;
        }
    }

    public override void SaveWorldData(TagCompound tag)
    {
        tag.Add("HarmonyMod:WarsFought", GoblinWarsFought);
        base.SaveWorldData(tag);
    }

    public override void LoadWorldData(TagCompound tag)
    {
        GoblinWarsFought = tag.GetInt("HarmonyMod:WarsFought");
        base.LoadWorldData(tag);
    }

    public override void PostUpdateInvasions()
    {
        
        base.PostUpdateInvasions();
    }
    
}