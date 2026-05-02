using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Commands;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class HomingSnowball : YukiCardModel
{
    public HomingSnowball() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true) { }

    public override bool UsesSnowCrystals => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8m, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (YukiCrystalSystem.CurrentCrystals > 5 && cardPlay.Target != null)
        {
            await PowerCmd.Apply<VulnerablePower>(
                cardPlay.Target, 
                1m, 
                Owner.Creature, 
                this, 
                false
            );
        }

        await MegaCrit.Sts2.Core.Commands.DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
