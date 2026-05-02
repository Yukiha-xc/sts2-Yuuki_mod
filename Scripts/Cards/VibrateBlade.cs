using MegaCrit.Sts2.Core.Commands.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class VibrateBlade : YukiCardModel
{
    public VibrateBlade() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, true) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(7m, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        AttackCommand attack = await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState)
            .Execute(choiceContext);

        
        decimal blockToGain = attack.Results.Sum(r => r.UnblockedDamage);

        if (blockToGain > 0)
        {
            await CreatureCmd.GainBlock(base.Owner.Creature, blockToGain, ValueProp.Move, cardPlay);
        }

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}


