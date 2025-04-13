using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace HarmonyMod.Content.UI.SummonSlotDisplay;

public class SummonSlotUIState : UIState
{
    public override void OnInitialize()
    {
        UIText text = new UIText("SUMMONSLOTS!! AAAA!!!");
        
        text.HAlign = 0.5f;
        text.VAlign = 0.5f;
        Append(text);
    }
}