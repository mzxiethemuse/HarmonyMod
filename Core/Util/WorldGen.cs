using System.Collections.Generic;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace HarmonyMod.Core.Util;

public class WorldGenSystem : ModSystem
{
    private static List<WorldGenTask> addedtasks;

    public override void Load()
    {
        addedtasks = new List<WorldGenTask>();
    }

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        foreach (var task in addedtasks)
        {
            int stepIndex = tasks.FindIndex(genpass => genpass.Name.Equals(task.PlaceToInsert));
            if (stepIndex != -1) {
                tasks.Insert(stepIndex + 1, task);
            }
        }

    }

    public static void AddTask(WorldGenTask task)
    {
        addedtasks.Add(task);
    }
}

public abstract class WorldGenTask : GenPass {
    
    /// <summary>
    /// consult https://github.com/tModLoader/tModLoader/wiki/Vanilla-World-Generation-Steps
    /// </summary>
    public virtual string PlaceToInsert { get;}
    
    protected WorldGenTask(string name, double loadWeight) : base(name, loadWeight)
    {
    }

    public virtual void Apply(GenerationProgress? progress = null, GameConfiguration? gameConfiguration = null)
    {
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        Apply(progress, configuration);
    }
}