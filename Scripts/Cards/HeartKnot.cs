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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class HeartKnot : YukiCardModel
{
    public HeartKnot() : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, true) { }

    public override bool UsesEmpathy => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(18m, ValueProp.Move),
        new EnergyVar(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var empathyEnemies = base.CombatState.Enemies.Where(e => e != null && e.IsAlive && e.HasPower<EmpathyPower>()).ToList();
        int count = empathyEnemies.Count;

        
        foreach (var enemy in empathyEnemies)
        {
            await PowerCmd.Remove<EmpathyPower>(enemy);
        }

        
        int totalTriggers = 1 + count;

        for (int i = 0; i < totalTriggers; i++)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .TargetingAllOpponents(base.CombatState)
                .Execute(choiceContext);

            await PlayerCmd.GainEnergy(1, base.Owner);
        }

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(4m);
    }
}
