using BaseLib.Abstracts;
using Godot;

namespace yuuki.Scripts;


public class YukiPool : CustomCardPoolModel
{
    
    public override string Title => "YUUKI";

    
    public override string? TextEnergyIconPath => "res://yuuki/images/ui/energy_icon_small.png";
    
    public override string? BigEnergyIconPath => "res://yuuki/images/ui/energy_icon_medium.png";

    
    public override Color DeckEntryCardColor => new(0.5f, 1.0f, 1.0f);

    
    public override Color ShaderColor => new(0.4f, 0.9f, 1.0f);

    
    public override bool IsColorless => false;
}
