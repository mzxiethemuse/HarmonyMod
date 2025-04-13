using System.Collections.Generic;

namespace HarmonyMod.Core.Graphics;

public class AnimationManager
{
    // AnimationLayers contain AnimationLayers
    
}

public class AnimationLayer
{
    public virtual void Draw()
    {
        
    }
}

public class AnimationCompositeLayer : AnimationLayer
{
    
}

public class AnimationFrameLayer : AnimationLayer
{
    
}