using System;

namespace HarmonyMod.Content.Clusters.MidnightSwamp.NPCs;

public struct Transmutoadtion
{
    public Transmutoadtion(int outputType, bool free = true, Func<bool>? condition = null)
    {
        output = outputType;
        alwaysAvailable = free;
        if (condition != null)
        {
            this.condition = condition;
        }
        else
        {
            condition = () => false;
        }
    }
    
    public int output { get; }
    public bool alwaysAvailable { get; }
    public Func<bool> condition { get; }
}