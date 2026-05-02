using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class SnowResonance : YukiCardModel
{
    public SnowResonance() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true) { }

    public override bool UsesEmpathy => true;
    public override bool UsesSnowCrystals => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(9m, ValueProp.Move),
        new YukiCrystalVar(1m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Attack", base.Owner.Character.AttackAnimDelay);

        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        if (cardPlay.Target != null && cardPlay.Target.HasPower<EmpathyPower>())
        {
            YukiCrystalSystem.AddCrystals((int)base.DynamicVars[YukiCrystalVar.Key].BaseValue);
        }

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
