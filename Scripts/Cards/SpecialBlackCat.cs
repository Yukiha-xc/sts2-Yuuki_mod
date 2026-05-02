using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using yuuki.Scripts.Powers;
using BaseLib.Abstracts;
using BaseLib.Utils;
using yuuki.Scripts;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SpecialBlackCat : YukiCardModel
{
    public SpecialBlackCat() : base(1, CardType.Skill, CardRarity.Token, TargetType.Self, true) 
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal, CardKeyword.Exhaust];

    public override string PortraitPath => "res://yuuki/images/cards/BlackCat.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new IntVar("Reduction", 6m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal reduction = base.DynamicVars["Reduction"].BaseValue;
        
        
        var power = await PowerCmd.Apply<BlackCatPower>(base.Owner.Creature, 2m, base.Owner.Creature, this);
        if (power != null)
        {
            power.ReductionAmount = reduction;
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Reduction"].UpgradeValueBy(3m);
    }
}

