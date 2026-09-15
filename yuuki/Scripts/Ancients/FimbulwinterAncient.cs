using BaseLib.Abstracts;
using BaseLib.Utils;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace yuuki.Scripts.Ancients;

public class FimbulwinterAncient : CustomAncientModel
{
    // 选项按钮颜色
    public override Color ButtonColor => new Color(0.7f, 0.85f, 1.0f, 0.5f);
    // 对话框颜色
    public override Color DialogueColor => new Color(0.5f, 0.7f, 1.0f);

    // 仅在第二幕出现
    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 2;
    }
    
    // 鍦烘櫙璺緞
    public override string? CustomScenePath => "res://yuuki/scenes/ancients/fimbulwinter.tscn";
    
    // 鍥炬爣璺緞
    public override string? CustomMapIconPath => "res://yuuki/images/ancients/fimbulwinter_icon.png";
    public override string? CustomMapIconOutlinePath => "res://yuuki/images/ancients/fimbulwinter_icon.png";
    public override string? CustomRunHistoryIconPath => "res://yuuki/images/ancients/fimbulwinter_icon.png";
    public override string? CustomRunHistoryIconOutlinePath => "res://yuuki/images/ancients/fimbulwinter_icon.png";

    protected override OptionPools MakeOptionPools { get; } = new OptionPools(
        MakePool(AncientOption<MoonstoneRelic>(1), AncientOption<MusicBoxRelic>(1), AncientOption<WishTagRelic>(1)),
        MakePool(AncientOption<MageDiaryRelic>(1), AncientOption<PancakesRelic>(2), AncientOption<SoraLunchboxRelic>(2)),
        MakePool(AncientOption<NightWatchRelic>(2), AncientOption<BitterChocolateRelic>(1), AncientOption<GiftBoxRelic>(1))
    );



    public FimbulwinterAncient() : base(true, false)
    {
    }
}
