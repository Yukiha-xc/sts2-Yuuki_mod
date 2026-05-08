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
using MegaCrit.Sts2.Core.Models;
using yuuki.Scripts.Powers;

namespace yuuki.Scripts.Cards;

[Pool(typeof(YukiPool))]
public class Reunion : YukiCardModel
{
    public Reunion() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true) { }

    public override bool UsesEmpathy => true;

    public override string PortraitPath => "res://yuuki/images/cards/OCHIBA_e04b2.png";

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(1, base.Owner);

        var empathyEnemies = base.CombatState.Enemies.Where(e => e.IsAlive && e.HasPower<EmpathyPower>()).ToList();
        
        if (empathyEnemies.Count > 0)
        {
            foreach (var enemy in empathyEnemies)
            {
                await PowerCmd.Remove<EmpathyPower>(enemy);
            }
            
            int extraEnergy = (int)base.DynamicVars["Energy"].BaseValue;
            await PlayerCmd.GainEnergy(extraEnergy, base.Owner);
        }

        await Cmd.Wait(0.25f);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Energy"].UpgradeValueBy(1m);
    }
}

