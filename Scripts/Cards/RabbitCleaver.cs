using System;
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

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class RabbitCleaver : YukiCardModel
{
    private int _reduction = 0;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(15m, ValueProp.Move),
        new IntVar("Decrease", 3m)
    ];

    public RabbitCleaver() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, true) { }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
            .Execute(choiceContext);

        
        _reduction += base.DynamicVars["Decrease"].IntValue;
        UpdateDamage();

        await Cmd.Wait(0.25f);
    }

    private void UpdateDamage()
    {
        decimal baseDmg = IsUpgraded ? 17m : 15m;
        base.DynamicVars.Damage.BaseValue = Math.Max(0, baseDmg - _reduction);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}


