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
public class SnowFairyChant : YukiCardModel
{
    
    public SnowFairyChant() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, true) { }

    public override bool UsesSnowCrystals => true;

    public override string PortraitPath => "res://yuuki/images/cards/YUKI_e03a6.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<SnowFairyChantPower>("MagicNumber", 1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            foreach (var tip in base.ExtraHoverTips)
            {
                yield return tip;
            }
            
            yield return HoverTipFactory.FromPower<SnowFairyChantPower>();
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        await PowerCmd.Apply<SnowFairyChantPower>(choiceContext, base.Owner.Creature, 
            base.DynamicVars["MagicNumber"].BaseValue, 
            base.Owner.Creature, 
            this
        );
        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["MagicNumber"].UpgradeValueBy(1m);
    }
}

