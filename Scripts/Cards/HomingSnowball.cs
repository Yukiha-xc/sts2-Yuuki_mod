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
        new DamageVar(8m, ValueProp.Move),
        new DynamicVar("Vulnerable", 1m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var target = cardPlay.Target;
        if (target == null) return;

        
        if (YukiCrystalSystem.CurrentCrystals > 4)
        {
            await PowerCmd.Apply<VulnerablePower>(choiceContext, target, 
                base.DynamicVars["Vulnerable"].BaseValue, 
                Owner.Creature, 
                this, 
                false
            );
        }

        
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        
        base.DynamicVars["Vulnerable"].UpgradeValueBy(1m);
    }
}

