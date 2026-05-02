using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class ResonanceImpact : YukiCardModel
{
    public ResonanceImpact() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true) { }

    public override bool UsesEmpathy => true;
    
    public override int CapacityOverload => 1;

    public override string PortraitPath => "res://yuuki/images/cards/BG081_125.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(13m, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;

        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        if (cardPlay.Target.HasPower<EmpathyPower>())
        {
            await PowerCmd.Remove<EmpathyPower>(cardPlay.Target);
            CardModel clone = CreateClone();
            clone.EnergyCost.SetThisCombat(0);
            await CardPileCmd.AddGeneratedCardToCombat(clone, PileType.Discard, true);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
