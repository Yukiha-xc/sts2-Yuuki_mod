using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SnowEmpathy : YukiCardModel
{
    
    public SnowEmpathy() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self, true) { }

    public override bool UsesSnowCrystals => true;
    public override bool UsesEmpathy => true;

    public override string PortraitPath => "res://yuuki/images/cards/YUKI_e14b.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            foreach (var tip in base.ExtraHoverTips)
            {
                yield return tip;
            }
            
            yield return HoverTipFactory.FromPower<SnowEmpathyPower>();
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        await PowerCmd.Apply<SnowEmpathyPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this);
        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        AddKeyword(CardKeyword.Innate);
    }
}

